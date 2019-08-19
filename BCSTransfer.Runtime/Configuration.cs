using BCSTransfer.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Runtime
{
    public class Configuration
    {
        public string Token { get; set; }
        public int ListId { get; set; }
        public int TagId { get; set; }
        public string KlickTippPassword { get; set; }
        public string KlickTippUsername { get; set; }
        public string OrganisationSlug { get; set; }
        public string EventSlug { get; set; }
        public int TwitterQuestionId { get; set; }
        public string LogLevel { get; set; }

        public Configuration()
            => LogLevel = "Debug";

        public void StoreIn(IDatabase database)
        {
            AddOrUpdate(database, nameof(Token), Token);           
            AddOrUpdate(database, nameof(ListId), ListId.ToString());           
            AddOrUpdate(database, nameof(TagId), TagId.ToString());           
            AddOrUpdate(database, nameof(KlickTippPassword), KlickTippPassword);           
            AddOrUpdate(database, nameof(KlickTippUsername), KlickTippUsername);           
            AddOrUpdate(database, nameof(OrganisationSlug), OrganisationSlug);           
            AddOrUpdate(database, nameof(EventSlug), EventSlug);           
            AddOrUpdate(database, nameof(TwitterQuestionId), TwitterQuestionId.ToString());           
            AddOrUpdate(database, nameof(LogLevel), LogLevel);           
        }

        private void AddOrUpdate(IDatabase database, string name, string value)
        {
            var setting = database.Settings.Find(name);

            if (setting == null)
                database.Settings.Add(new Setting { Key = name, Value = value });
            else
                setting.Value = value;
        }

        internal static Configuration FromDatabase(IDatabase database) => new Configuration
        {
            Token = database.Settings.Find(nameof(Token)).Value,
            ListId = int.Parse(database.Settings.Find(nameof(ListId)).Value),
            TagId = int.Parse(database.Settings.Find(nameof(TagId)).Value),
            KlickTippPassword = database.Settings.Find(nameof(KlickTippPassword)).Value,
            KlickTippUsername = database.Settings.Find(nameof(KlickTippUsername)).Value,
            OrganisationSlug = database.Settings.Find(nameof(OrganisationSlug)).Value,
            EventSlug = database.Settings.Find(nameof(EventSlug)).Value,
            TwitterQuestionId = int.Parse(database.Settings.Find(nameof(TwitterQuestionId)).Value),
            LogLevel = database.Settings.Find(nameof(LogLevel)).Value,
        };

    }
}
