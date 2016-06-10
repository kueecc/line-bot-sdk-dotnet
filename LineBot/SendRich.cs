using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineBot
{
    public partial class Client
    {
        public RichMessageRequest NewRichMessage(int height)
        {
            return new RichMessageRequest(this, height);
        }
    }

    public class RichMessageRequest
    {
        private readonly Dictionary<string, RichMessageAction> _actions = new Dictionary<string, RichMessageAction>();
        private readonly Client _client;
        private readonly int _height;
        private readonly List<RichMessageListener> _listeners = new List<RichMessageListener>();

        public RichMessageRequest(Client client, int height)
        {
            _client = client;
            _height = height;
        }

        public RichMessageRequest SetAction(string name, string text, string linkUri)
        {
            _actions[name] = new RichMessageAction
            {
                Type = "web",
                Text = text,
                Params = new Dictionary<string, string>
                {
                    ["linkUri"] = linkUri
                }
            };

            return this;
        }

        public RichMessageRequest SetListener(string actionName, int x, int y, int width, int height)
        {
            _listeners.Add(new RichMessageListener
            {
                Type = "touch",
                Action = actionName,
                Params = new[] {x, y, width, height}
            });

            return this;
        }

        public Task<ResponseContent> Send(string[] to, string imageUrl, string altText)
        {
            var markup = new RichMessageMarkup
            {
                Canvas = new RichMessageCanvas
                {
                    Width = 1040,
                    Height = _height,
                    InitialScene = "scene1"
                },
                Images = new Dictionary<string, RichMessageImage>
                {
                    ["image1"] = new RichMessageImage
                    {
                        X = 0,
                        Y = 0,
                        W = 1040,
                        H = _height
                    }
                },
                Actions = _actions,
                Scenes = new Dictionary<string, RichMessageScene>
                {
                    ["scene1"] = new RichMessageScene
                    {
                        Draws = new List<RichMessageSceneImage>
                        {
                            new RichMessageSceneImage
                            {
                                Image = "image1",
                                X = 0,
                                Y = 0,
                                W = 1040,
                                H = _height
                            }
                        },
                        Listeners = _listeners
                    }
                }
            };

            var markupJson = JsonConvert.SerializeObject(markup);

            return _client.SendSingleMessage(to, new SingleMessageContent
            {
                ContentType = ContentType.RichMessage,
                ToType = RecipientType.User,
                ContentMetaData = new Dictionary<string, string>
                {
                    ["DOWNLOAD_URL"] = imageUrl,
                    ["SPEC_REV"] = "1",
                    ["ALT_TEXT"] = altText,
                    ["MARKUP_JSON"] = markupJson
                }
            });
        }

        public Task<ResponseContent> Send(string to, string imageUrl, string altText)
        {
            return Send(new[] {to}, imageUrl, altText);
        }
    }

    internal class RichMessageMarkup
    {
        [JsonProperty("canvas")]
        public RichMessageCanvas Canvas { get; set; }

        [JsonProperty("images")]
        public Dictionary<string, RichMessageImage> Images { get; set; }

        [JsonProperty("actions")]
        public Dictionary<string, RichMessageAction> Actions { get; set; }

        [JsonProperty("scenes")]
        public Dictionary<string, RichMessageScene> Scenes { get; set; }
    }

    internal class RichMessageCanvas
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("initialScene")]
        public string InitialScene { get; set; }
    }

    internal class RichMessageImage
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("w")]
        public int W { get; set; }

        [JsonProperty("h")]
        public int H { get; set; }
    }

    internal class RichMessageAction
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, string> Params { get; set; }
    }

    internal class RichMessageListener
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("params")]
        public int[] Params { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }

    internal class RichMessageScene
    {
        [JsonProperty("draws")]
        public List<RichMessageSceneImage> Draws { get; set; }

        [JsonProperty("listener")]
        public List<RichMessageListener> Listeners { get; set; }
    }

    internal class RichMessageSceneImage
    {
        [JsonProperty("h")] public int H;

        [JsonProperty("image")] public string Image;

        [JsonProperty("w")] public int W;

        [JsonProperty("x")] public int X;

        [JsonProperty("y")] public int Y;
    }
}