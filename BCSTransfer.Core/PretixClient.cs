using BCSTransfer.Core.PretixModel;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public class PretixClient : ApiClient, IPretixClient
    {
        public string Token { private get; set; }
        public Organizer[] Organisations { get; private set; }
        public override string BaseAddress => "https://pretix.eu/api/v1/";

        public PretixClient(string token) : base(LogManager.GetCurrentClassLogger())
        {
            Token = token;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
        }

        public override async Task<bool> TryAuthenticate()
        {
            logger.Debug("Try to authenticate at pretix");
            using (var response = await httpClient.GetAsync("organizers/"))
            {
                Authenticatet = response.StatusCode == HttpStatusCode.OK;

                var organizers = JsonConvert.DeserializeObject<Organizers>(await response.Content.ReadAsStringAsync());

                if (Authenticatet)
                {
                    Organisations = organizers.Results;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    logger.Trace(response.ReasonPhrase);
                    logger.Error(new UnauthorizedAccessException("Http response returns 401 Unauthorized"));
                }
                else
                {
                    logger.Error(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));
                }

            }

            return Authenticatet;
        }
        public async Task<bool> TryAuthenticate(string token)
        {
            Token = token;
            return await TryAuthenticate();
        }

        public async Task<Orders> GetOrders(Organizer organizer, Event @event)
        {
            var list = await GetList<PagedListResponse<Order>>($"organizers/{organizer.Slug}/events/{@event.Slug}/orders/");

            if (list == null)
                list = new PagedListResponse<Order>();

            return new Orders(this, list);
        }

        public async Task<Events> GetEvents(Organizer organizer)
        {
            var list = await GetList<PagedListResponse<Event>>($"organizers/{organizer.Slug}/events/");

            if (list == null)
                list = new PagedListResponse<Event>();

            return new Events(this, list);
        }

        public async Task<T> GetNext<T>(T listResponse) where T : PagedListResponse
        {
            logger.Trace($"Get Next list of type {listResponse.GetType().Name}");

            if (string.IsNullOrWhiteSpace(listResponse.Next))
                return default;

            return await GetList<T>(listResponse.Next);
        }

        public async Task<T> GetPrevious<T>(T listResponse) where T : PagedListResponse
        {
            logger.Trace($"Get Previous list of type {listResponse.GetType().Name}");

            if (string.IsNullOrWhiteSpace(listResponse.Previous))
                return default;

            return await GetList<T>(listResponse.Previous);
        }

        public async Task<T> GetList<T>(string url) where T : PagedListResponse
        {
            if (string.IsNullOrWhiteSpace(url))
                return default;

            logger.Trace($"Get list => {url}");

            try
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        logger.Warn($"GetList failed: {response.StatusCode}: {response.ReasonPhrase}");
                        return default;
                    }

                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} in GetList: " + ex.Message);
                return default;
            }
        }
    }
}
