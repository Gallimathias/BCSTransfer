using System.Threading.Tasks;
using BCSTransfer.Core.PretixModel;

namespace BCSTransfer.Core
{
    public interface IPretixClient
    {
        string BaseAddress { get; }
        Organizer[] Organisations { get; }
        string Token { set; }

        Task<Events> GetEvents(Organizer organizer);
        Task<T> GetList<T>(string url) where T : PagedListResponse;
        Task<T> GetNext<T>(T listResponse) where T : PagedListResponse;
        Task<Orders> GetOrders(Organizer organizer, Event @event);
        Task<T> GetPrevious<T>(T listResponse) where T : PagedListResponse;
        Task<bool> TryAuthenticate();
        Task<bool> TryAuthenticate(string token);
    }
}