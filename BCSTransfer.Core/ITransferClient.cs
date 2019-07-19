using System.Threading.Tasks;
using BCSTransfer.Core.PretixModel;

namespace BCSTransfer.Core
{
    public interface ITransferClient
    {
        Event Event { get; set; }
        Organizer Organizer { get; set; }
        int TwitterQuestionId { get; set; }

        string GetTwitterName(Answer answer);
        Task Start();
        void Stop();
    }
}