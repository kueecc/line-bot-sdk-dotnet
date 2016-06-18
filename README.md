# line-bot-sdk-dotnet

SDK of the LINE BOT API Trial for .NET


## Installation ##

line-bot-sdk-dotnet is available as a NuGet package. You can install it using the NuGet Package Console window:

```
PM> Install-Package LineBot
```

## Configuration ##

```csharp
var client = new LineBot.Client(<Channel Id>, <Channel Secret>, <MID>);
```


## Usage ##

### Sending messages ###

Send a text message, image, video, audio, location, or sticker to the mids.

- [https://developers.line.me/bot-api/api-reference#sending_message](https://developers.line.me/bot-api/api-reference#sending_message)

```csharp
	// send text
	var res = await client.SendText("<target user's MID>", "Hello, world!");

	// send image
	var res = await client.SendImage("<target user's MID>", "http://example.com/image.jpg", "http://example.com/image_preview.jpg");

	// send video
	var res = await client.SendVideo("<target user's MID>", "http://example.com/video.mp4", "http://example.com/image_preview.jpg");

	// send audio
	var res = await client.SendAudio("<target user's MID>", "http://example.com/audio.mp3", 2000);

	// send location
	var res = await client.SendLocation("<target user's MID>", "location label", "tokyo shibuya-ku", 35.661777, 139.704051);

	// send sticker
	var res = await client.SendSticker("<target user's MID>", 1, 1, 100);
```

### Sending multiple messages ###

The `multiple_message` method allows you to use the _Sending multiple messages API_.

- [https://developers.line.me/bot-api/api-reference#sending_multiple_messages](https://developers.line.me/bot-api/api-reference#sending_multiple_messages)

```csharp
	var res = client.NewMultipleMessage()
		.AddText("Hello,")
		.AddText("world!")
		.AddImage("http://example.com/image.jpg", "http://example.com/image_preview.jpg")
		.AddVideo("http://example.com/video.mp4", "http://example.com/image_preview.jpg")
		.AddAudio("http://example.com/audio.mp3", 2000)
		.AddLocation("Location label", "tokyo shibuya-ku", 35.61823286112982, 139.72824096679688)
		.AddSticker(1, 1, 100)
		.Send("<target user's MID>");
```

### Sending rich messages ###

The `rich_message` method allows you to use the _Sending rich messages API_.

- [https://developers.line.me/bot-api/api-reference#sending_rich_content_message](https://developers.line.me/bot-api/api-reference#sending_rich_content_message)

```csharp
	var res = client.NewRichMessage(1040)
		.SetAction("MANGA", "manga", "https://store.line.me/family/manga/en")
		.SetListener("MANGA", 0, 0, 520, 520)
		.SetAction("MUSIC", "music", "https://store.line.me/family/music/en")
		.SetListener("MUSIC", 520, 0, 520, 520)
		.Send("<target user's MID>", "https://example.com/rich-image/foo", "This is a alt text.");
```

### Receiving messages ###

The following utility method allows you to easily process messages sent from the BOT API platform via a Callback URL.

- [https://developers.line.me/bot-api/api-reference#receiving_messages](https://developers.line.me/bot-api/api-reference#receiving_messages)

```csharp
	client.On("https://*:8443/callback/", results =>
	{
	    foreach (var result in results.Results)
	    {
	        var content = result.Content();
	        if (content.IsMessage && content.IsText)
	        {
	            var text = content.TextContent();
	            client.SendText(content.From, text.Text);
	        }
	    }
	});
```



### Getting user profile information ###

You can retrieve the user profile information by specifying the mid.

- [https://developers.line.me/bot-api/api-reference#getting_user_profile_information](https://developers.line.me/bot-api/api-reference#getting_user_profile_information)

```csharp
	client.On("https://*:8443/callback/", results =>
	{
	    foreach (var result in results.Results)
	    {
	        var content = result.Content();
	        var profile = client.GetUserProfile(content.From).Result;
		    Console.WriteLine($"{profile.Contacts[0].DisplayName}");
	    }
	});
```
