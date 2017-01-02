using Newtonsoft.Json;

namespace Automation.Models
{
    public class Book
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("read")]
        public bool Read { get; set; }
    }
}
