using Newtonsoft.Json;

namespace GarageDoorModels
{
    public record GarageDoorStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isOpen")]
        public int IsOpen { get; set; }

        [JsonProperty("_ts")]
        public long TimestampSeconds { private get; set; }

        public DateTime Timestamp
        {
            get
            {
                var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds(TimestampSeconds).UtcDateTime;
                return TimeZoneInfo.ConvertTimeFromUtc(
                    utcTimestamp,
                    TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            }
        }
    }
}
