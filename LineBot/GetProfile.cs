using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LineBot
{
    public partial class Client
    {
        public async Task<UserProfile> GetUserProfile(string[] mids)
        {
            var query = "mids=" + string.Join(",", mids);
            string uri = $"{Api.Profile}?{query}";

            var res = await _httpClient.GetAsync(uri);
            res.EnsureSuccessStatusCode();

            var body = await res.Content.ReadAsStringAsync();
            var profile = JsonConvert.DeserializeObject<UserProfile>(body);
            return profile;
        }

        public Task<UserProfile> GetUserProfile(string mid)
        {
            return GetUserProfile(new[] { mid });
        }

    }

    public class ContactInfo
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("pictureUrl")]
        public string PictureUrl { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }
    }

    public class UserProfile
    {
        [JsonProperty("contacts")]
        public List<ContactInfo> Contacts { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("display")]
        public int Display { get; set; }
    }
}