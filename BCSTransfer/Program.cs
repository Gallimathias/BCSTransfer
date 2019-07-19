using BCSTransfer.Core;
using BCSTransfer.Core.PretixModel;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTransfer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileInfo = new FileInfo(Path.Combine(".", "config.json"));

            if (!fileInfo.Exists)
            {
                File.WriteAllText(fileInfo.FullName, JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
                return;
            }

            Configuration configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(fileInfo.FullName));

            var config = new LoggingConfiguration();

            config.AddRule(LogLevel.FromString(configuration.LogLevel), LogLevel.Fatal, target: new ColoredConsoleTarget("BCSTransfer.logconsole"));
            config.AddRule(LogLevel.FromString(configuration.LogLevel), LogLevel.Fatal, target: new FileTarget("BCSTransfer.logfile")
            {
                FileName = $"./logs/BCSTransfer-{DateTime.Now.ToString("ddMMyy_hhmmss")}.log"
            });

            LogManager.Configuration = config;

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Start initialization");

            var pretix = new PretixClient(configuration.Token);
            var klickTipp = new KlickTippClient(configuration.TagId, configuration.ListId);

            var pretixTask = Task.Run(async () => await pretix.TryAuthenticate());

            var klickTippTask = Task.Run(async () => await klickTipp.TryAuthenticate(configuration.KlickTippUsername, configuration.KlickTippPassword));

            try
            {
                pretixTask.Wait();

                if (!pretixTask.Result)
                {
                    logger.Warn("Pretix authentication failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Pretix authentication failed: " + ex.Message);
                return;
            }

            Organizer organisation = pretix.Organisations.First(o => o.Slug == configuration.OrganisationSlug);
            Event pretixEvent = pretix.GetEvents(organisation).Result.First(e => e.Slug == configuration.EventSlug);

            try
            {
                klickTippTask.Wait();

                if (!klickTippTask.Result)
                {
                    logger.Warn("klickTipp authentication failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "klickTipp authentication failed: " + ex.Message);
                return;
            }

            var transferClient = new TransferClient(pretix, klickTipp)
            {
                Event = pretixEvent,
                Organizer = organisation,
                TwitterQuestionId = configuration.TwitterQuestionId
            };

             var semaphore = new SemaphoreSlim(0, 1);
            Console.CancelKeyPress += (s, e) =>
            {
                logger.Info("Stop application");

                try
                {
                    transferClient.Stop();
                    var task = Task.Run(klickTipp.Logout);
                    task.Wait();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Exception on closing application");
                }
                Thread.Sleep(100);
                semaphore.Release();
            };

            logger.Info("Initialization successfull");

            var transferTask = Task.Run(async () =>
            {
                await transferClient.Start();
            });

            semaphore.Wait();
            semaphore.Dispose();
        }
    }
}
