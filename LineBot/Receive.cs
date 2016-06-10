using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace LineBot
{
    public partial class Client
    {
        public ReceivedResults CreateReceivesFromJson(string body)
        {
            var results = JsonConvert.DeserializeObject<ReceivedResults>(body);
            return results;
        }

        public bool ValidateSignature(byte[] rawBody, string signature)
        {
            var sha = new HMACSHA256(Encoding.UTF8.GetBytes(_channelSecret));
            var hash = sha.ComputeHash(rawBody);
            return signature == Convert.ToBase64String(hash);
        }
    }

    public class ReceivedResults
    {
        [JsonProperty("result")]
        public ReceivedResult[] Results { get; set; }
        
        [JsonIgnore]
        public Exception Exception { get; set; }
    }

    public class ReceivedResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("fromChannel")]
        public int FromChannel { get; set; }

        [JsonProperty("to")]
        public string[] To { get; set; }

        [JsonProperty("toChannel")]
        public int ToChannel { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("content")]
        public RawContent_ RawContent { get; set; }

        public ReceivedContent Content()
        {
            var result = new ReceivedContent
            {
                Parent = this,
                Id = RawContent.Id,
                From = RawContent.From,
                CreatedTime = RawContent.CreatedTime,
                To = RawContent.To,
                ToType = RawContent.ToType,
                IsOperation = EventType == LineBot.EventType.ReceivingOperation,
                IsMessage = EventType == LineBot.EventType.ReceivingMessage,
                OpType = RawContent.OpType,
                ContentType = RawContent.ContentType
            };

            return result;
        }

        public class RawContent_
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("contentType")]
            public ContentType ContentType { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("createdTime")]
            public long CreatedTime { get; set; }

            [JsonProperty("to")]
            public string[] To { get; set; }

            [JsonProperty("toType")]
            public RecipientType ToType { get; set; }

            [JsonProperty("contentMetadata")]
            public Dictionary<string, string> ContentMetaData { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("location")]
            public MessageContentLocation Location { get; set; }

            [JsonProperty("revision")]
            public int Revision { get; set; }

            [JsonProperty("opType")]
            public OpType OpType { get; set; }

            [JsonProperty("params")]
            public string[] Params { get; set; }
        }
    }

    public class ReceivedContent
    {
        public ReceivedResult Parent { get; set; }
        public string Id { get; set; }
        public string From { get; set; }
        public long CreatedTime { get; set; }
        public string[] To { get; set; }
        public RecipientType ToType { get; set; }
        public bool IsOperation { get; set; }
        public bool IsMessage { get; set; }
        public OpType OpType { get; set; }
        public ContentType ContentType { get; set; }

        public bool IsText => ContentType == ContentType.Text;
        public bool IsImage => ContentType == ContentType.Image;
        public bool IsVideo => ContentType == ContentType.Video;
        public bool IsAudio => ContentType == ContentType.Audio;
        public bool IsLocation => ContentType == ContentType.Location;
        public bool IsSticker => ContentType == ContentType.Sticker;
        public bool IsContact => ContentType == ContentType.Contact;


        public ReceivedTextContent TextContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Text)
            {
                throw new InvalidContentTypeException();
            }

            var result = new ReceivedTextContent
            {
                ReceivedContent = this,
                Text = Parent.RawContent.Text
            };

            return result;
        }

        public ReceivedImageContent ImageContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Image)
            {
                throw new InvalidContentTypeException();
            }

            var result = new ReceivedImageContent
            {
                ReceivedContent = this
            };

            return result;
        }

        public ReceivedVideoContent VideoContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Video)
            {
                throw new InvalidContentTypeException();
            }

            var result = new ReceivedVideoContent
            {
                ReceivedContent = this
            };

            return result;
        }

        public ReceivedAudioContent AudioContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Audio)
            {
                throw new InvalidContentTypeException();
            }

            var audlen = int.Parse(Parent.RawContent.ContentMetaData["AUDLEN"]);

            var result = new ReceivedAudioContent
            {
                ReceivedContent = this,
                Duration = audlen
            };

            return result;
        }

        public ReceivedLocationContent LocationContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Location)
            {
                throw new InvalidContentTypeException();
            }

            var result = new ReceivedLocationContent
            {
                ReceivedContent = this,
                Text = Parent.RawContent.Text,
                Title = Parent.RawContent.Location.Title,
                Address = Parent.RawContent.Location.Address,
                Latitude = Parent.RawContent.Location.Latitude,
                Longitude = Parent.RawContent.Location.Longitude
            };

            return result;
        }

        public ReceivedStickerContent StickerContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Sticker)
            {
                throw new InvalidContentTypeException();
            }

            var stkPkgId = int.Parse(Parent.RawContent.ContentMetaData["STKPKGID"]);
            var stkId = int.Parse(Parent.RawContent.ContentMetaData["STKID"]);
            var stkVer = int.Parse(Parent.RawContent.ContentMetaData["STKVER"]);

            var result = new ReceivedStickerContent
            {
                ReceivedContent = this,
                PakcageId = stkPkgId,
                Id = stkId,
                Version = stkVer
            };

            return result;
        }

        public ReceivedContactContent ContactContent()
        {
            if (!IsMessage)
            {
                throw new InvalidEventTypeException();
            }

            if (ContentType != ContentType.Contact)
            {
                throw new InvalidContentTypeException();
            }

            var result = new ReceivedContactContent
            {
                ReceivedContent = this,
                Mid = Parent.RawContent.ContentMetaData["mid"],
                DisplayName = Parent.RawContent.ContentMetaData["displayName"]
            };

            return result;
        }

        public ReceivedOperation OperationContent()
        {
            if (!IsOperation)
            {
                throw new InvalidEventTypeException();
            }

            var result = new ReceivedOperation
            {
                ReceivedContent = this,
                Revision = Parent.RawContent.Revision,
                Params = Parent.RawContent.Params
            };

            return result;
        }
    }

    public class ReceivedTextContent
    {
        public ReceivedContent ReceivedContent { get; set; }
        public string Text { get; set; }
    }

    public class ReceivedImageContent
    {
        public ReceivedContent ReceivedContent { get; set; }
    }

    public class ReceivedVideoContent
    {
        public ReceivedContent ReceivedContent { get; set; }
    }

    public class ReceivedAudioContent
    {
        public ReceivedContent ReceivedContent { get; set; }
        public int Duration { get; set; }
    }

    public class ReceivedLocationContent
    {
        public ReceivedContent ReceivedContent { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ReceivedStickerContent
    {
        public ReceivedContent ReceivedContent { get; set; }
        public int PakcageId { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
    }

    public class ReceivedContactContent
    {
        public ReceivedContent ReceivedContent { get; set; }
        public string Mid { get; set; }
        public string DisplayName { get; set; }
    }

    public class ReceivedOperation
    {
        public ReceivedContent ReceivedContent { get; set; }
        public int Revision { get; set; }
        public string[] Params { get; set; }
    }
}