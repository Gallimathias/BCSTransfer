using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.PretixModel
{
    public class Orders : PagedList<Order>
    {
        public Orders(PretixClient pretixClient, PagedListResponse<Order> currentPage) : base(pretixClient, currentPage)
        {
        }
    }
}
