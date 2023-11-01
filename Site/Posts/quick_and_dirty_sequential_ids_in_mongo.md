;;;
{
    "title": "Quick & Dirty Sequential IDs in MongoDB",
    "description": "Mongo doesn't natively support generating sequential IDs. Here's a quick & dirty solution to get you up and going if you need sequential IDs.",
    "date": "1 November 2023",
    "contents": false,
    "hero": "photo-1534078362425-387ae9668c17",
    "related": [
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
        { "title": "Giscus is Awesome", "description": "I can add comments to my statically generated blog? Using GitHub Discussions?? For Free??? And it works????", "fileName": "giscus_is_awesome" }
    ]
}
;;;

That Mongo doesn't natively support sequential IDs is one of the many knocks against it. Sure, you _should_ be using GUID IDs in Mongo, but suppose you're working on a microservices conversion and you have a legacy mainframe that needs to be able to know what your objects are? If you're content just using Atlas, you can create a counter collection and add a trigger for auto-incrementing IDs [fairly easily](https://www.mongodb.com/basics/mongodb-auto-increment).

Suppose however that you can't use a pure Atlas solution - you'll need to implement this logic yourself in your own code. If you happen to be working in a microservices environment you have concurrency concerns - there might be multiple shards of your database and/or multiple replicas of your microservice.

I had to implement this, so I'll write it down in case it might help you. I'll provide a solution in Go, which should be able to translate fairly well to any other language you might be using.

# Updating a counter collection

As a prerequisite, ensure you have the Mongo driver:

```plaintext
go get go.mongodb.org/mongo-driver/mongo
```

Just as Mongo's tutorial for Atlas recommends, we'll implement a counter collection. This collection will contain one document per "kind" of ID we need to generate. If you have just one object that needs sequential IDs, then you'll only have one document in this collection. We'll represent this collection document with a struct. It only needs one field, `sequence`, which will represent the latest ID generated:

```go
type MongoCounterDocument struct {
    sequence int `bson:"sequence"`
}
```

The ID of each document in the collection should be a string you hardcode or keep in a settings file (such as `"personIdCounter"`), and doesn't need to be in the document struct. Instead, we'll encapsulate that in a generator struct along with a reference to the collection:

```go
type MongoIdGenerator struct {
    counterCollection *mongo.Collection
    counterDocumentId string
}
```

To implement the functionality to generate the next ID, we'll use the `FindOneAndUpdate` operation to increment `sequence` and return the new ID to us. We can specify a couple options here: we can upsert the document so that it will be created automatically if one isn't there for us (useful for integration tests), and we can specify that we want the operation to read and return us a copy of the document _after_ the update has taken place.

```go
func (generator *MongoIdGenerator) GetNextId() (int, error) {
    filter := bson.M{"_id": m.counterDocumentId}
    update := bson.M{"$inc": bson.M("sequence": 1)}
    options := options.FindOneAndUpdate().SetUpsert(true).SetReturnDocument(options.After)

    var updatedDocument MongoIdCounter

    err := m.counterCollection.FindOneAndUpdate(context.TODO(), filter, update, options).Decode(&updatedDocument)
    if err != nil {
        return 0, errors.New("Unable to update Mongo id counter collection.")
    }

    return updatedDocument.sequence, nil
}
```

`FindOneAndUpdate` is atomic and shouldn't have any concurrency concerns so long as you **do not shard the counter collection**.

# But I don't want to have to hit Mongo every time I want a new id

Wow, you and I think alike, I didn't either. To get around this, we can have our app generate multiple IDs each time it hits Mongo and use these IDs until it runs out locally.

With this approach you have the concern that if your app is spinning up and tearing down too frequently, you'll start losing IDs in the mix. There are various strategies to mitigate this, such as retrieving a small number of IDs from Mongo each time or persisting the cache of IDs, but I'm not going to get into those here.

We'll add `nextId` and `maxId` properties to the generator object, as well as an increment field to specify how many IDs we should generate each time:

```go
type MongoIdGenerator struct {
    counterCollection *mongo.Collection
    counterDocumentId string
    incrementBy       int
    nextId            int
    maxId             int
}
```

We'll add a func to instantiate this at startup. It'll be important that your app only has one of these objects per "kind" of ID you need to generate:

```go
func SetupMongoIdGenerator(collection *mongo.Collection, documentId string) *MongoIdGenerator {
    return $MongoIdGenerator{
        counterCollection   : collection,
        counterDocumentId   : documentId,
        // Adjust this up or down depending on how many IDs you want to generate at once:
        incrementBy         : 25,
        nextId              : 0,
        maxId               : 0
    }
}
```

And we can update our `GetNextId` function to consult Mongo or not if `nextId` equals `maxId`:

```go
func (generator *MongoIdGenerator) GetNextId() (int, error) {
    if generator.nextId == generator.maxId {
        filter := bson.M{"_id": m.counterDocumentId}
        update := bson.M{"$inc": bson.M("sequence": generator.incrementBy)}
        options := options.FindOneAndUpdate().SetUpsert(true).SetReturnDocument(options.After)

        var updatedDocument MongoIdCounter

        err := m.counterCollection.FindOneAndUpdate(context.TODO(), filter, update, options).Decode(&updatedDocument)
        if err != nil {
            return 0, errors.New("Unable to update Mongo id counter collection.")
        }

        generator.nextId = updatedDocument.sequence - incrementBy
        generator.maxId = updatedDocument.sequence
    }

    generator.nextId += 1
    return generator.nextId, nil
}
```

We do have a concurrency concern here though - we want to ensure `nextId` and `maxId` are only being accessed one at a time. We can use a mutex in the generator for this. Update the generator:

```go
type MongoIdGenerator struct {
    counterCollection *mongo.Collection
    counterDocumentId string
    incrementBy       int
    nextId            int
    maxId             int
    mutex             sync.Mutex
}
```

And add the following two to the beginning of `GetNextId`:

```go
generator.mutex.Lock()
defer generator.mutex.Unlock()
```

That should be that! Here's the final code all together:

```go
type MongoCounterDocument struct {
    sequence int `bson:"sequence"`
}

type MongoIdGenerator struct {
    counterCollection *mongo.Collection
    counterDocumentId string
    incrementBy       int
    nextId            int
    maxId             int
    mutex             sync.Mutex
}

func SetupMongoIdGenerator(collection *mongo.Collection, documentId string) *MongoIdGenerator {
    return $MongoIdGenerator{
        counterCollection   : collection,
        counterDocumentId   : documentId,
        incrementBy         : 25,
        nextId              : 0,
        maxId               : 0
    }
}

func (generator *MongoIdGenerator) GetNextId() (int, error) {
    generator.mutex.Lock()
    defer generator.mutex.Unlock()

    if generator.nextId == generator.maxId {
        filter := bson.M{"_id": m.counterDocumentId}
        update := bson.M{"$inc": bson.M("sequence": generator.incrementBy)}
        options := options.FindOneAndUpdate().SetUpsert(true).SetReturnDocument(options.After)

        var updatedDocument MongoIdCounter

        err := m.counterCollection.FindOneAndUpdate(context.TODO(), filter, update, options).Decode(&updatedDocument)
        if err != nil {
            return 0, errors.New("Unable to update Mongo id counter collection.")
        }

        generator.nextId = updatedDocument.sequence - incrementBy
        generator.maxId = updatedDocument.sequence
    }

    generator.nextId += 1
    return generator.nextId, nil
}
```
