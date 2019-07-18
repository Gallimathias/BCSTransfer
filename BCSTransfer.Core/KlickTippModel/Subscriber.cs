using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.KlickTippModel
{
    public class Subscriber
    {
        public string Email { get; set; }
        public string TwitterHandel { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int ListId { get; set; }
        public int TagId { get; set; }

        public string ToUrlQuery()
                  => $"email={Uri.EscapeUriString(Email)}&" +
                     $"listid={ListId}&tagid={TagId}&" +
                     $"{Uri.EscapeUriString("fields[fieldFirstName]")}={Uri.EscapeUriString(FirstName)}&" +
                     $"{Uri.EscapeUriString("fields[fieldLastName]")}={Uri.EscapeUriString(LastName)}&" +
                    (string.IsNullOrWhiteSpace(TwitterHandel) ? "" : $"{Uri.EscapeUriString("fields[field52215]")}={TwitterHandel}");

        public Dictionary<string, string> ToDictionary()
        {
            var dic = new Dictionary<string, string>()
            {
                { "email", Email },
                { "listid", ListId.ToString() },
                { "tagid", TagId.ToString() },
                { "fields[fieldFirstName]", FirstName },
                { "fields[fieldLastName]", LastName },
                { "", "" },
            };

            if (!string.IsNullOrWhiteSpace(TwitterHandel))
                dic.Add("fields[field52215]", TwitterHandel);

            return dic;
        }
    }
}
