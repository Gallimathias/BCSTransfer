using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public abstract class ApiClient
    {
        public abstract string BaseAddress { get;  }
        public bool Authenticatet { get; protected set; }

        protected readonly HttpClient httpClient;
        protected readonly Logger logger;

        public ApiClient(Logger logger)
        {
            this.logger = logger;

            Authenticatet = false;
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(BaseAddress)
            };
        }

        public abstract Task<bool> TryAuthenticate();


    }
}