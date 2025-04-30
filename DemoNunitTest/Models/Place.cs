using Newtonsoft.Json;

namespace DemoNunitTest.Models
{
    public class Place
    {
        [JsonProperty("place name")]
        public string PlaceName { get; set; }

        public string State { get; set; }

        public string StateAbbreviation { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
