using System.Collections.Generic;
using System.Threading.Tasks;

namespace LineBot
{
    public partial class Client
    {
        public MultipleMessageRequest NewMultipleMessage()
        {
            return new MultipleMessageRequest(this);
        }
    }


    public class MultipleMessageRequest
    {
        private readonly Client _client;
        private readonly List<SingleMessageContent> _messages = new List<SingleMessageContent>();

        public MultipleMessageRequest(Client client)
        {
            _client = client;
        }

        public MultipleMessageRequest AddText(string text)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Text,
                Text = text
            });

            return this;
        }

        public MultipleMessageRequest AddImage(string imageUrl, string previewUrl)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Image,
                OriginalContentUrl = imageUrl,
                PreviewImageUrl = previewUrl
            });

            return this;
        }

        public MultipleMessageRequest AddVideo(string videoUrl, string previewUrl)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Video,
                OriginalContentUrl = videoUrl,
                PreviewImageUrl = previewUrl
            });

            return this;
        }


        public MultipleMessageRequest AddAudio(string audioUrl, int duration)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Audio,
                OriginalContentUrl = audioUrl,
                ContentMetaData = new Dictionary<string, string>
                {
                    ["AUDLEN"] = duration.ToString()
                }
            });

            return this;
        }


        public MultipleMessageRequest AddLocation(string title, string address, double latitute, double longitude)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Location,
                Text = title,
                Location = new MessageContentLocation
                {
                    Title = title,
                    Address = address,
                    Latitude = latitute,
                    Longitude = longitude
                }
            });

            return this;
        }


        public MultipleMessageRequest AddSticker(string stkId, string stkPkgId, string stkVer)
        {
            _messages.Add(new SingleMessageContent
            {
                ContentType = ContentType.Sticker,
                ContentMetaData = new Dictionary<string, string>
                {
                    ["STKID"] = stkId,
                    ["STKPKGID"] = stkPkgId,
                    ["STKVER"] = stkVer
                }
            });

            return this;
        }

        public Task<ResponseContent> Send(string[] to)
        {
            return _client.SendMultipleMessage(to, new MultipleMessageContent
            {
                MessageNotified = 0,
                Messages = _messages
            });
        }

        public Task<ResponseContent> Send(string to)
        {
            return Send(new[] {to});
        }

    }
}