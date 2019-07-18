using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.PretixModel
{
    public class Events : PagedList<Event>
    {
        public Events(PretixClient pretixClient, PagedListResponse<Event> currentPage) : base(pretixClient, currentPage)
        {
        }
    }
}
