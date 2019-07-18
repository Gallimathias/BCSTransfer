using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.KlickTippModel
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "session_name")]
        public string SessionName { get; set; }
        [JsonProperty(PropertyName = "sessid")]
        public string SessionId { get; set; }
    }
}
