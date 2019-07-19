using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public interface IApiClient
    {
        bool Authenticatet { get; }
        string BaseAddress { get; }

        Task<bool> TryAuthenticate();
    }
}