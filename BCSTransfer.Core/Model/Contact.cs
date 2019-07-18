using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.Model
{
    public class Contact : Entity
    {
        public int? PretixId { get; internal set; }
        public int? PretixPositionId { get; internal set; }
        public string PretixOrder { get; internal set; }
        public bool TagedByKlickTipp { get; internal set; }
        public string TwitterHandel { get; internal set; }
    }
}
