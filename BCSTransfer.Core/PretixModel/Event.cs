using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.PretixModel
{
    public class Event
    {
        public Dictionary<string, string> Name { get; set; }
        public string Slug { get; set; }
        public bool Live { get; set; }
        public bool Testmode { get; set; }
        public string Currency { get; set; }
        [JsonProperty(PropertyName = "date_from")]
        public DateTime? DateFrom { get; set; }
        [JsonProperty(PropertyName = "date_to")]
        public DateTime? DateTo { get; set; }
        [JsonProperty(PropertyName = "date_admission")]
        public object DateAdmission { get; set; }
        [JsonProperty(PropertyName = "is_public")]
        public bool? IsPublic { get; set; }
        [JsonProperty(PropertyName = "presale_start")]
        public DateTime? PresaleStart { get; set; }
        [JsonProperty(PropertyName = "presale_end")]
        public DateTime? PresaleEnd { get; set; }
        public object Location { get; set; }
        [JsonProperty(PropertyName = "has_subevents")]
        public bool HasSubevents { get; set; }
        [JsonProperty(PropertyName = "meta_data")]
        public MetaData MetaData { get; set; }
        public string[] Plugins { get; set; }
    }
}
