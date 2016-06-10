using System;
using Newtonsoft.Json;

namespace Echo
{
    class Program
    {
        static void Main(string[] args)
        {
            string channelId = Environment.GetEnvironmentVariable("CHANNEL_ID");
            string channelSecret = Environment.GetEnvironmentVariable("CHANNEL_SECRET");
            string mid = Environment.GetEnvironmentVariable("CHANNEL_MID");

            Console.WriteLine($"CHANNEL_ID: {channelId}, CHANNEL_SECRET: {channelSecret}, CHANNEL_MID: {mid}");

            var client = new LineBot.Client(channelId, channelSecret, mid);
            client.On("https://*:8443/callback/", results =>
            {
                if (results.Exception != null)
                {
                    Console.WriteLine(results.Exception);
                    return;
                }

                foreach (var result in results.Results)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(result));

                    try
                    {
                        var content = result.Content();
                        if (content.IsMessage)
                        {
                            if (content.IsText)
                            {
                                var text = content.TextContent();
                                if (text.Text == "me")
                                {
                                    var profile = client.GetUserProfile(content.From).Result;
                                    if (profile.Count > 0)
                                    {
                                        client.SendText(content.From, $"Hi! {profile.Contacts[0].DisplayName}!");
                                    }
                                }
                                else
                                {
                                    client.SendText(content.From, text.Text);
                                }
                            }
                            else if (content.IsSticker)
                            {
                                var sticker = content.StickerContent();
                                client.SendSticker(content.From, sticker.Id, sticker.PakcageId, sticker.Version);
                            }
                            else if (content.IsLocation)
                            {
                                var location = content.LocationContent();
                                client.SendLocation(content.From, location.Title, location.Address, location.Latitude, location.Longitude);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });

            Console.ReadKey();
        }
    }
}
