using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BCSTransfer.Core.KlickTippModel;
using Newtonsoft.Json;
using NLog;

namespace BCSTransfer.Core
{
    public class KlickTippClient : ApiClient
    {
        public override string BaseAddress => "https://www.klick-tipp.com/api/";

        public int TagId { get; }
        public int ListId { get; }

        public KlickTippClient(int tagId, int listId) : base(LogManager.GetCurrentClassLogger())
        {
            TagId = tagId;
            ListId = listId;
        }

        public override Task<bool> TryAuthenticate()
        {
            var ex = new NotSupportedException();
            logger.Fatal(ex, $"{nameof(TryAuthenticate)} with no parameters is not supported");
            throw ex;
        }
        public async Task<bool> TryAuthenticate(string username, string password)
        {
            logger.Debug("Try to authenticate at klick-tipp");

            var dic = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            using (var urlEncodedContent = new FormUrlEncodedContent(dic))
            using (var response = await httpClient.PostAsync($"account/login", urlEncodedContent))
            {
                Authenticatet = response.StatusCode == HttpStatusCode.OK;

                var content = await response.Content.ReadAsStringAsync();                

                if (Authenticatet)
                {
                    var authResponse = JsonConvert.DeserializeObject<AuthResponse>(content);
                    httpClient.DefaultRequestHeaders.Add("Cookie", $"{authResponse.SessionName}={authResponse.SessionId}");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    logger.Trace(content);
                    logger.Trace(response.ReasonPhrase);
                    logger.Error(new UnauthorizedAccessException("Http response returns 401 Unauthorized"));
                }
                else
                {
                    logger.Trace(content);
                    logger.Error(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));
                }
            }

            return Authenticatet;
        }

        public async Task Logout()
        {
            logger.Debug("Try to log out at klick-tipp");

            using (var response = await httpClient.PostAsync($"account/logout", null))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    logger.Error(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));
            }
        }

        public async Task<bool> TryTagUserByMail(string attendeeEmail)
        {
            logger.Debug("Try to tag user at klick-tipp");

            if (string.IsNullOrWhiteSpace(attendeeEmail))
                return false;

            attendeeEmail = Uri.EscapeUriString(attendeeEmail);

            var dic = new Dictionary<string, string>
            {
                { "email", attendeeEmail },
                { "tagids", TagId.ToString() }
            };

            using (var urlEncodedContent = new FormUrlEncodedContent(dic))
            using (var response = await httpClient.PostAsync($"subscriber/tag", urlEncodedContent))
            {
                var result = response.StatusCode == HttpStatusCode.OK;

                if (!result)
                    logger.Warn(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));

                return result;
            }
        }

        public async Task CreateSubscriber(Subscriber subscriber)
        {
            logger.Debug("Try to create user at klick-tipp");
            subscriber.TagId = TagId;
            subscriber.ListId = ListId;

            using (var urlEncodedContent = new FormUrlEncodedContent(subscriber.ToDictionary()))
            using (var response = await httpClient.PostAsync($"subscriber", urlEncodedContent))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    logger.Error(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));
            }
        }
    }
}

