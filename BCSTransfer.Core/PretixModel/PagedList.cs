using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTransfer.Core.PretixModel
{
    public class PagedList<T> : IEnumerable<T>
    {
        public List<T> Items { get; set; }

        private PagedListResponse<T> currentPage;
        private readonly PretixClient client;

        public int Count => currentPage.Count;
        public int Aviable => currentPage.Count - currentPage.Results.Length;

        public PagedList(PretixClient pretixClient, PagedListResponse<T> currentPage)
        {
            this.currentPage = currentPage;
            client = pretixClient;
            Items = new List<T>(currentPage.Count);
            Items.AddRange(currentPage.Results);
        }

        public async Task<T[]> GetNext()
        {
            if (Aviable < 1)
                return new T[0];

            var newList = await client.GetNext(currentPage);

            if (newList == null)
                return currentPage.Results;

            currentPage = newList;
            Items.AddRange(currentPage.Results);
            return currentPage.Results;
        }

        public IEnumerator<T> GetEnumerator()
            => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() 
            => new Enumerator(this);

        private struct Enumerator : IEnumerator<T>
        {
            private readonly PagedList<T> pagedList;
            private int index;

            public Enumerator(PagedList<T> pagedList)
            {
                this.pagedList = pagedList;
                index = -1;
            }

            public T Current => pagedList.Items[index];

            object IEnumerator.Current => pagedList.Items[index];

            public bool MoveNext()
            {
                index++;

                if (index > pagedList.Items.Count - 1)
                {
                    var list = pagedList;
                    var task = Task.Run(async () => await list.GetNext());
                    task.Wait();
                }

                return index < pagedList.Count;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose()
            {
                Reset();
            }
        }
    }
}
