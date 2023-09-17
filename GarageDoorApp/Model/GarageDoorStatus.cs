using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorApp.Model
{
    public class GarageDoorStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isOpen")]
        public int IsOpen { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
