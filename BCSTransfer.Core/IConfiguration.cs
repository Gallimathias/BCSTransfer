using BCSTransfer.Core.Model;

namespace BCSTransfer.Core
{
    public interface IConfiguration
    {
        string EventSlug { get; set; }
        string KlickTippPassword { get; set; }
        string KlickTippUsername { get; set; }
        int ListId { get; set; }
        string LogLevel { get; set; }
        string OrganisationSlug { get; set; }
        int TagId { get; set; }
        string Token { get; set; }
        int TwitterQuestionId { get; set; }

        void StoreIn(IDatabase database);
    }
}