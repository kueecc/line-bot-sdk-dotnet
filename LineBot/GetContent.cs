using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LineBot
{
    public partial class Client
    {
        private async Task<MessageContentResponse> InternalGetMessageContent(ReceivedContent content, bool isPreview)
        {
            string url;
            if (isPreview)
            {
                url = Api.Preview.Replace("<messageId>", content.Id);
            }
            else
            {
                url = Api.Content.Replace("<messageId>", content.Id);
            }

            var res = await _httpClient.GetAsync(url);

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return await Task.FromResult<MessageContentResponse>(null);
            }

            var stream = await res.Content.ReadAsStreamAsync();

            var disposition = res.Content.Headers.GetValues("Content-Disposition").First();
            var filename = new ContentDisposition(disposition).FileName;

            var result = new MessageContentResponse
            {
                Content = stream,
                FileName = filename
            };

            return result;
        }

        public Task<MessageContentResponse> GetMessageContent(ReceivedContent content)
        {
            return InternalGetMessageContent(content, false);
        }

        public Task<MessageContentResponse> GetMessageContentPreview(ReceivedContent content)
        {
            return InternalGetMessageContent(content, true);
        }
    }


    public class MessageContentResponse
    {
        public Stream Content { get; set; }

        public string FileName { get; set; }
    }
}