using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineBot
{
    public partial class Client
    {
        private readonly string _channelId;
        private readonly string _channelSecret;
        private readonly HttpClient _httpClient;
        private readonly string _mid;


        public Client(string channelId, string channelSecret, string mid)
        {
            _channelId = channelId;
            _channelSecret = channelSecret;
            _mid = mid;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Endpoint.TrialBotApi);
            _httpClient.DefaultRequestHeaders.Add(HeaderName.ChannelId, channelId);
            _httpClient.DefaultRequestHeaders.Add(HeaderName.ChannelSecret, channelSecret);
            _httpClient.DefaultRequestHeaders.Add(HeaderName.TrustedUserWithAcl, mid);
            //_httpClient.DefaultRequestHeaders.Add("X-Line-ChannelToken", channelToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<ResponseContent> InternalSendMessage<TMessage>(TMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await _httpClient.PostAsync(Api.Event, c);
            res.EnsureSuccessStatusCode();

            var content = await res.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseContent>(content);
            return result;
        }

        internal Task<ResponseContent> SendSingleMessage(string[] to, SingleMessageContent content)
        {
            var message = new SingleMessage
            {
                To = to,
                ToChannel = ChannelId.SendingChannelId,
                EventType = EventType.SendingMessage,
                Content = content
            };

            return InternalSendMessage(message);
        }

        internal Task<ResponseContent> SendMultipleMessage(string[] to, MultipleMessageContent content)
        {
            var message = new MultipleMessage
            {
                To = to,
                ToChannel = ChannelId.SendingChannelId,
                EventType = EventType.SendingMultipleMessages,
                Content = content
            };

            return InternalSendMessage(message);
        }
    }
}