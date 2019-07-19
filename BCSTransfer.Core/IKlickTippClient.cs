using System.Threading.Tasks;
using BCSTransfer.Core.KlickTippModel;

namespace BCSTransfer.Core
{
    public interface IKlickTippClient
    {
        string BaseAddress { get; }
        int ListId { get; }
        int TagId { get; }

        Task CreateSubscriber(Subscriber subscriber);
        Task Logout();
        Task<bool> TryAuthenticate();
        Task<bool> TryAuthenticate(string username, string password);
        Task<bool> TryTagUserByMail(string attendeeEmail);
    }
}