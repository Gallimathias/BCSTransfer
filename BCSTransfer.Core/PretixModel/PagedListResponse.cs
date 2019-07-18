using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core.PretixModel
{
    public abstract class PagedListResponse
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public string Detail { get; set; }
    }
    public class PagedListResponse<T> : PagedListResponse
    {
        public T[] Results { get; set; }

        public PagedListResponse()
        {
            Results = new T[0];
        }
    }
}
