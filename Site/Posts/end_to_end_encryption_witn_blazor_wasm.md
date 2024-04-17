;;;
{
	"title": "Roll Your Own End-to-End Encryption in Blazor WASM",
	"description": "Using the SubtleCrypto API to get simple end-to-end encryption for a collaborative Blazor WASM app.",
	"date": "17 April 2024",
	"contents": true,
	"hero": "photo-1572435555646-7ad9a149ad91",
    "related": [
        { "title": "Write Your Own RDBMS Versioned Migration Boilerplate", "description": "Versioned migrations are an essential tool for systems using an RDBMS, and it's no work at all to start your applications the right way with this pattern.", "fileName": "write_your_own_rdbms_versioned_migration_boilerplate" },
        { "title": "Deploying ASP.NET 7 Projects with Railway", "description": "Railway is a startup cloud infrastructure provider that has gained traction for being easy to use and cheap for hobbyists. Let's get a .NET 7 Blazor WASM app up and running with it!", "fileName": "deploying_aspdotnet_7_projects_with_railway" },
        { "title": "Quick & Dirty Sequential IDs in MongoDB", "description": "Mongo doesn't natively support generating sequential IDs. Here's a quick & dirty solution to get you up and going if you need sequential IDs.", "fileName": "quick_and_dirty_sequential_ids_in_mongo" }
    ]
}
;;;

Just the other day I released [my free planning poker app](https://ian.wold.guru/Posts/free_planning_poker.html) and there was one thing missing that I really wanted to have: end-to-end encryption. Sure, folks aren't going to be entering sensitive information into a planning poker tool, right? Well, _probably_ not at least, right? Maybe it's more the principle of the matter, but I like that this application's server will never be in a position to even accidentally have that info.

Collaborative applications which support users interacting in real-time are pretty common, and Blazor WASM seems to want to support this use case pretty well. Microsoft's documentation walks you through a real-time chat application, and there's no end of tutorials showing how ot use it with SignalR. I'll probably add to that noise in future. One thing that's unfortunately absent is encryption.

Microsoft's own libraries are complicated and/or don't work on WASM (I'm still very unclear on the state of this - please comment if you know something), and I did not have any success with third-party libraries for Blazor. Do I really want to import a third party package for this though? These "small" Nuget packages are a dime a dozen around Blazor, and most of them are poorly maintained.

After some tinkering this was an area I decided to roll my own, and I'm glad I did. The [SubtleCrypto](https://developer.mozilla.org/en-US/docs/Web/API/SubtleCrypto) API supported by most browsers (all the important ones) is actually quite a good encryption provider, and it's easy to set up JS interop with Blazor. This solution clocks in at 58 lines of JS and 11 lines of C#, so I can copy and paste between projects and tailor however I need for each one.

# The Simple Case

In my case, I only need to encrypt a few short strings. A title and a name - that's it! In fact, the solution I need is going to be almost exactly the same as [Excalidraw's encryption](https://blog.excalidraw.com/end-to-end-encryption/). In the case of my planning poker app, a user who creates a planning session will share their URL with their other participants, who can then join the session at that URL. That URL contains the session ID, and also the encryption key exactly as Excalidraw does.

A note on the key, then: it is passed in the URL in the hash, so a URL for my application looks like `https://freeplanningpoker.io/session/2ae188d8#key=9aac428b962ead5a678b13f7`. This is important, as the hash never makes it to the server.

Because the key is stored in the URL, I can access it from the page's JS, so I don't really need to know about it from C# except when I first generate it, so that I can redirect the user to the correct session page. This can simplify my C# API down quite a bit:

```csharp
async Task<string> GetEncryptionKeyAsync();
async Task<string> EncryptAsync(string value);
async Task<string> DecryptAsync(string value);
```

I don't want to make things excessively complicated, especially for this case. Here then I want to write a bit of JavaScript with which these two methods can interop, and let the JavaScript worry about parsing the key from the URL and storing that.

## Handling the Key

I encourage you to read the documentation on the SubtleCrypto API, it's quite straightforward. I'm going to use the AES-GCM algorithm, and we can use `window.crypto.subtle.generateKey` for this:

```js
window.encryptionKey = await window.crypto.subtle.generateKey(
    { name: "AES-GCM", length: 128 }, // The algorithm
    true,                             // The key is extractable - important!
    ["encrypt", "decrypt"]            // This key is used for both directions
);
```

Note that I'm storing the key at `window.encryptionKey` - this is how I'm sorting the key on the JS side rather than the C# side. Not sure if putting it in `window` is ideal, I tend to avoid JavaScript when I can. _insert skill issue meme here..._

This creates a key object with a bunch of properties that SubtleCrypto uses, but we need to get a simple string to be able to pass through in our URL. For this purpose we can call `exportKey` and encode it with `jwk`:

```js
return (await window.crypto.subtle.exportKey("jwk", window.encryptionKey)).k
```

We can wrap these up into [a `getEncryptionKey` function](https://gist.github.com/IanWold/8e337a22ce7eb386b65bb77948eebd66#file-encryption-js-L20) and call this from C#, assuming we have injected `IJSRuntime jsRuntime` into our class:

```csharp
public async Task<string> GetEncryptionKeyAsync() =>
    await jsRuntime.InvokeAsync<string>("getEncryptionKey") ?? /* Do something for null case */;
```

## Getting the Key from the URL

When a new participant visits the link they were given by the session organizer, I need to get the key out of the URL and store it in `window.encryptionKey`, where my encrypt and decrypt functions are going to expect it. When the window loads, it's simple to get the key out of the hash:

```js
const objectKey = window.location.hash.slice("#key=".length);
```

And while there's a fair amount of fluff here, it's also simple to reconstruct the key (this comes right from Excalidraw's implementation):

```js
window.encryptionKey = await window.crypto.subtle.importKey(
    "jwk",
    {
        k: objectKey,
        alg: "A128GCM",                     // The algorithm
        ext: true,                          // Extractable
        key_ops: ["encrypt", "decrypt"],    // For both operations
        kty: "oct",                         // Not entirely sure...
    },
    { name: "AES-GCM", length: 128 },       // Still the same algorithm
    true,                                   // Still extractable
    ["encrypt", "decrypt"]                  // Still for both operations
); 
```

And this can all be stuffed nicely into [a DOMContentLoaded event handler](https://gist.github.com/IanWold/8e337a22ce7eb386b65bb77948eebd66#file-encryption-js-L1).

## Encrypting

Now the fun part! This is _almost_ as simple as calling `window.crypto.subtle.encrypt`. As it happens, this `encrypt` function expects to deal with `ArrayBuffer` objects intead of strings. Fair enough, but we have to do a bit of wrapping around the whole deal. The first thing is to transform the string to an `ArrayBuffer`, and that's easy enough with `TextEncoder`:

```js
const encodedValue = new TextEncoder().encode(stringValue);
```

We can use this value and the encryption key that we've saved in order to encrypt the value:

```js
const encrypted = await window.crypto.subtle.encrypt(
    { name: "AES-GCM", iv: new Uint8Array(12) },        // Use empty IV
    window.encryptionKey,                               // The key we saved
    encodedValue
);
```

We now have another `ArrayBuffer` in `encrypted`, but we want to translate that into a string before we send it up. For this, I'm going to use a method [described by David Myers](https://davidmyers.dev/blog/a-practical-guide-to-the-web-cryptography-api) to get a base64 string from the `ArrayBuffer`:

```js
return window.btoa(String.fromCharCode.apply(null, new Uint8Array(encrypted)));
```

In all, this makes for a very tidy, very simple [encrypt function](https://gist.github.com/IanWold/8e337a22ce7eb386b65bb77948eebd66#file-encryption-js-L32).

And then another simple call from C#:

```csharp
public async Task<string> EncryptAsync(string value) =>
    await jsRuntime.InvokeAsync<string>("encrypt", value) ?? /* Do something for null case */;
``` 

## Decrypting

In this case now, I need to turn my base64 string _back_ into an array buffer, decrypt it, and then turn the resulting `ArrayBuffer` back into a string to send up to the C#. The case of parsing the base64 string comes straight from David Myers again:

```js
const bValue = window.atob(base64Value)
const buffer = new ArrayBuffer(bValue.length)
const bufferView = new Uint8Array(buffer)

for (let i = 0; i < bValue.length; i++) {
    bufferView[i] = bValue.charCodeAt(i)
}
```

That's a little unfortunate but 6 lines isn't so terrible. The `buffer` variable can now be passed into `window.crypto.subtle.decrypt`:

```js
const decrypted = await window.crypto.subtle.decrypt(
    { name: "AES-GCM", iv: new Uint8Array(12) },        // Same algorithm, empty IV
    window.encryptionKey,                               // The key we saved
    buffer
);
```

And just as we initially used `TextEncoder` to get the string from C# into an `ArrayBuffer`, we'll use `TextDecoder` to go the opposite direction:

```js
return new TextDecoder().decode(new Uint8Array(decrypted));
```

Instantiating a new `Uint8Array` turns the `ArrayBuffer` into an array, which is what `TextDecoder` wants.

This is an equally simple invocation from C#:

```csharp
public async Task<string> DecryptAsync(string value) =>
    await jsRuntime.InvokeAsync<string>("decrypt", value) ?? /* Do something for null case */;
```

# The More Complicated Cases

I'm really quite happy with the solution I have here for simple strings. More importantly, I'm a fan of small-code roll-your-own solutions because they give flexibility and customization. When I carry this code over to other projects, there might be (surely will be) different requirements that will require changes here. I want to demonstrate a couple of cases for which this code can easily be extended.

## Encrypting Whole Objects

I struggle to think of a case where JSON serialization isn't the answer here. The good deal is that this can be achieved either in C# or JS.

If you're handling serialization in C#, you can leave the JS as-is and add all the logic to `EncryptAsync` and `DecryptAsync`:

```csharp
using System.Text.Json;

public async Task<string> EncryptAsync<TValue>(TValue value)
{
    var serialized = JsonSerializer.Serialize(value);
    return await jsRuntime.InvokeAsync<string>("encrypt", serialized) ?? /* Do something for null case */;
}

public async Task<TValue> DecryptAsync<TValue>(string value)
{
    var serialized = await jsRuntime.InvokeAsync<string>("decrypt", value) ?? /* Do something for null case */;

    try
    {
        return await JsonSerializer.Deserialize<TValue>(serialized) ?? /* Do something for null case */;
    }
    catch
    {
        /* Do something for error case */
    }
}
```

This seems like the best case to me as I'm only chaging one part of the code, but depending on your use case you might want to do the serialization in JS. In this case you still need some more error handling in C#.

```js
async function encrypt(value) {
    const stringValue = JSON.stringify(value);
    // Same as previous implementation
}

async function decrypt(value) {
    // Same as previous implementation

    return JSON.parse(decryptedStringValue);
}
```

`IJSRuntime.InvokeAsync` is generic, so we can cast right to the type we want, but we do need to handle errors. This does a JSON parse itself, so I'm not certain if this is more performant in most cases. That said, when we're doing the JSON serialization in JS, we don't need to modify our `EncryptAsync` method from the first pass.

```csharp
public async Task<TValue> DecryptAsync<TValue>(string value)
{
    try
    {
        return await jsRuntime.InvokeAsync<TValue>("decrypt", value) ?? /* Do something for null case */;
    }
    catch
    {
        /* Do something for error case */
    }
}
```

## Using an IV

In my simple case, I did not use an IV ([initialization vector](https://en.wikipedia.org/wiki/Initialization_vector)). Well, I should say that I used an IV with all zeroes. There are plenty of use cases for which you do want this though.

Luckily, SubtleCrypto does have an easy way to generate these, but the catch is that we want to create a different IV per encryption, so it's no good to create a static IV at the same time that we create the encryption key. Therefore, the IV needs to ride alongside the encrypted value. An easy way to achieve this would be to generate the IV before encryption, the encrypt the value with this IV and create the base64 string, then create an object holding both this value and the IV, then serialize that object and base64 encode it. This would then allow the deserialization to extract the IV before deserializing the value.

This is relatively straightforward to implement just in JS with `window.crypto.getRandomValues`, assuming you don't care about the IV in C#:

```js
async function encrypt(value) {
    const iv = window.crypto.getRandomValues(new Uint8Array(12)); // Generate the IV
    const encrypted = window.btoa(String.fromCharCode.apply(null, new Uint8Array(
        await window.crypto.subtle.encrypt(
            { name: "AES-GCM", iv: iv }, // Use the IV
            window.encryptionKey,
            new TextEncoder().encode(value)
        )
    )));

    const values = {
        value: encrypted,
        iv: iv
    };

    return window.btoa(JSON.stringify(values));
}

async function decrypt(value) {
    const values = JSON.parse(window.atob(value)); // Reverse the process

    const bValue = window.atob(values.value)       // Get the encrypted value
    const buffer = new ArrayBuffer(bValue.length)
    const bufferView = new Uint8Array(buffer)

    for (let i = 0; i < bValue.length; i++) {
        bufferView[i] = bValue.charCodeAt(i)
    }
    
    return new TextDecoder().decode(new Uint8Array(
        await window.crypto.subtle.decrypt(
            { name: "AES-GCM", iv: values.iv },    // Use the new IV
            window.encryptionKey,
            buffer
        )
    ));
}
```
