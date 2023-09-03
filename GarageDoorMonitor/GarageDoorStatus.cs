using Newtonsoft.Json;

namespace GarageDoorMonitor
{
    public record GarageDoorStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isOpen")]
        public int IsOpen { get; set; }
    }
}
