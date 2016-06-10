using System.Collections.Generic;
using System.Threading.Tasks;

namespace LineBot
{
    public partial class Client
    {
        public Task<ResponseContent> SendText(string[] to, string text)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Text,
                ToType = RecipientType.User,
                Text = text
            });
        }

        public Task<ResponseContent> SendText(string to, string text)
        {
            return SendText(new[] {to}, text);
        }

        public Task<ResponseContent> SendImage(string[] to, string imageUrl, string previewUrl)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Image,
                ToType = RecipientType.User,
                OriginalContentUrl = imageUrl,
                PreviewImageUrl = previewUrl
            });
        }

        public Task<ResponseContent> SendImage(string to, string imageUrl, string previewUrl)
        {
            return SendImage(new[] {to}, imageUrl, previewUrl);
        }

        public Task<ResponseContent> SendVideo(string[] to, string videoUrl, string previewUrl)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Video,
                ToType = RecipientType.User,
                OriginalContentUrl = videoUrl,
                PreviewImageUrl = previewUrl
            });
        }

        public Task<ResponseContent> SendVideo(string to, string videoUrl, string previewUrl)
        {
            return SendVideo(new[] {to}, videoUrl, previewUrl);
        }

        public Task<ResponseContent> SendAudio(string[] to, string audioUrl, int duration)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Audio,
                ToType = RecipientType.User,
                OriginalContentUrl = audioUrl,
                ContentMetaData = new Dictionary<string, string>
                {
                    ["AUDLEN"] = duration.ToString()
                }
            });
        }

        public Task<ResponseContent> SendAudio(string to, string audioUrl, int duration)
        {
            return SendAudio(new[] {to}, audioUrl, duration);
        }

        public Task<ResponseContent> SendLocation(string[] to, string title, string address, double latitude,
            double longitude)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Location,
                ToType = RecipientType.User,
                Text = title,
                Location = new MessageContentLocation
                {
                    Title = title,
                    Address = address,
                    Latitude = latitude,
                    Longitude = longitude
                }
            });
        }

        public Task<ResponseContent> SendLocation(string to, string title, string address, double latitude,
            double longitude)
        {
            return SendLocation(new[] {to}, title, address, latitude, longitude);
        }

        public Task<ResponseContent> SendSticker(string[] to, int stkId, int stkPkgId, int stkVer)
        {
            return SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.Sticker,
                ToType = RecipientType.User,
                ContentMetaData = new Dictionary<string, string>
                {
                    ["STKID"] = stkId.ToString(),
                    ["STKPKGID"] = stkPkgId.ToString(),
                    ["STKVER"] = stkVer.ToString()
                }
            });
        }

        public Task<ResponseContent> SendSticker(string to, int stkId, int stkPkgId, int stkVer)
        {
            return SendSticker(new[] {to}, stkId, stkPkgId, stkVer);
        }
    }
}