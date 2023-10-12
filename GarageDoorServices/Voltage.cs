using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GarageDoorServices
{
    public struct Voltage
    {
        public Voltage()
        {
            TimeStamp = default;
            Value = 0;
        }

        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
