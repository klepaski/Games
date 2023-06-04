using System;
using Newtonsoft.Json;

namespace task6x.Models
{
    public class Data
    {
        [JsonProperty("startX")]
        public float StartX { get; set; }
        [JsonProperty("startY")]
        public float StartY { get; set; }
        [JsonProperty("endX")]
        public float EndX { get; set; }
        [JsonProperty("endY")]
        public float EndY { get; set; }
    }
}
