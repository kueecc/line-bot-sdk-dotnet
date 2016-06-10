using System.Collections.Generic;
using Newtonsoft.Json;

namespace LineBot
{
    public class ResponseContent
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("messasgeId")]
        public string MessageId { get; set; }

        [JsonProperty("failed")]
        public string[] Failed { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class ErrorResponseContent
    {
        [JsonProperty("statusCode")]
        public string Code { get; set; }

        [JsonProperty("statusMessage")]
        public string Message { get; set; }
    }

    internal class SingleMessage
    {
        [JsonProperty("to")]
        public string[] To { get; set; }

        [JsonProperty("toChannel")]
        public string ToChannel { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("content")]
        public SingleMessageContent Content { get; set; }
    }

    internal class MultipleMessage
    {
        [JsonProperty("to")]
        public string[] To { get; set; }

        [JsonProperty("toChannel")]
        public string ToChannel { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("content")]
        public MultipleMessageContent Content { get; set; }
    }

    internal class SingleMessageContent
    {
        [JsonProperty("contentType")]
        public ContentType ContentType { get; set; }


        [JsonProperty("toType", NullValueHandling = NullValueHandling.Ignore)]
        public RecipientType? ToType { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("originalContentUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalContentUrl { get; set; }

        [JsonProperty("previewImageUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string PreviewImageUrl { get; set; }

        [JsonProperty("contentMetadata", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> ContentMetaData { get; set; }

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public MessageContentLocation Location { get; set; }
    }

    internal class MultipleMessageContent
    {
        [JsonProperty("messageNotified", NullValueHandling = NullValueHandling.Ignore)]
        public int? MessageNotified { get; set; }

        [JsonProperty("messages")]
        public List<SingleMessageContent> Messages { get; set; }
    }

    public class MessageContentLocation
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}