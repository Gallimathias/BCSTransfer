using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer
{
    public class Configuration
    {
        public string Token { get; set; }
        public int ListId { get; set; }
        public int TagId { get; set; }
        public string KlickTippPassword { get; set; }
        public string KlickTippUsername { get; set; }
        public string OrganisationSlug { get; set; }
        public string EventSlug { get; set; }
        public int TwitterQuestionId { get; set; }
        public string LogLevel { get; set; }

        public Configuration()
        {
            LogLevel = "Debug";
        }
    }
}
