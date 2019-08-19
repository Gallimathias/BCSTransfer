using BCSTransfer.Core;
using BCSTransfer.Core.Model;
using BCSTransfer.Core.PretixModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTransfer.Runtime
{
    public class BCSTransferService : IDisposable
    {
        private ITransferClient transferClient;
        private readonly SemaphoreSlim semaphore;
        private readonly ITypeContainer typeContainer;
        private Logger logger;
        private KlickTippClient klickTipp;
        private Configuration configuration;
        private IPluginLoader pluginLoader;

        public BCSTransferService()
        {
            semaphore = new SemaphoreSlim(1, 1);
            typeContainer = TypeContainer.Get<ITypeContainer>();
        }

        public async Task Run(CancellationToken token)
        {
            token.Register(() =>
            {
                var task = Task.Run(Stop);
                task.Wait();
            });

            await MockPluginManager.Load(TypeContainer.Get<ITypeContainer>());

            Start();
            logger.Info("Start initialization");

            var result = await Initalization();

            if (!result)
                return;

            logger.Info("Initialization successfull");

            await transferClient.Start();
            await Task.Delay(-1, token);
        }

        private async Task<bool> Initalization()
        {
            var pretix = new PretixClient(configuration.Token);
            klickTipp = new KlickTippClient(configuration.TagId, configuration.ListId);
            typeContainer.Register<IPretixClient>(pretix);
            typeContainer.Register<IKlickTippClient>(klickTipp);

            try
            {
                var pretixTask = await pretix.TryAuthenticate();

                if (!pretixTask)
                {
                    logger.Warn("Pretix authentication failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Pretix authentication failed: " + ex.Message);
                return false;
            }

            Organizer organisation = pretix.Organisations.First(o => o.Slug == configuration.OrganisationSlug);
            Event pretixEvent = pretix.GetEvents(organisation).Result.First(e => e.Slug == configuration.EventSlug);

            try
            {
                var klickTippTask = await klickTipp.TryAuthenticate(configuration.KlickTippUsername, configuration.KlickTippPassword);

                if (!klickTippTask)
                {
                    logger.Warn("klickTipp authentication failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "klickTipp authentication failed: " + ex.Message);
                return false;
            }

            transferClient = typeContainer.Get<ITransferClient>();
            transferClient.Event = pretixEvent;
            transferClient.Organizer = organisation;
            return true;
        }

        public async Task Stop()
        {
            logger.Info("Stop application");

            try
            {
                transferClient.Stop();
                await klickTipp.Logout();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception on closing application");
            }

            await MockPluginManager.Unload();

            await Task.Delay(100);
        }

        public void Dispose()
        {
            semaphore.Dispose();
            typeContainer.Dispose();
        }

        private void Start()
        {
            Startup.Register(typeContainer);

            if (!TryGetFromFile<Dictionary<string, string>>(Path.Combine(".", "init.json"), out var initPaths))
                initPaths = new Dictionary<string, string>();

            if (initPaths.TryGetValue("Plugins", out var pluginPath))
            {
                pluginLoader = typeContainer.Get<IPluginLoader>();

                if (TryGetFromFile<Dictionary<string, string>>(pluginPath, out var plugins))
                    pluginLoader.LoadPlugins(plugins);
            }

            if (initPaths.TryGetValue("Database", out var dbPath))
            {
                var dbProvider = typeContainer.Get<IDatabaseProvider>();
                var dbContext = dbProvider.GetDatabaseContext(dbPath);
                typeContainer.Register(dbContext);
                var database = typeContainer.Get<IDatabase>();
                database.EnsureCreated();

                configuration = Configuration.FromDatabase(database);
            }
            else if (initPaths.TryGetValue("Configuration", out var configPath))
            {
                if (!TryGetFromFile(configPath, out configuration))
                    return;
            }
            else if (!TryGetFromFile(Path.Combine(".", "config.json"), out configuration))
            {
                return;
            }

            typeContainer.Register<IConfiguration>(configuration);

            var config = new LoggingConfiguration();

            config.AddRule(LogLevel.FromString(configuration.LogLevel), LogLevel.Fatal, target: new ColoredConsoleTarget("BCSTransfer.logconsole"));
            config.AddRule(LogLevel.FromString(configuration.LogLevel), LogLevel.Fatal, target: new FileTarget("BCSTransfer.logfile")
            {
                FileName = $"./logs/BCSTransfer-{DateTime.Now.ToString("ddMMyy_hhmmss")}.log"
            });

            LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();

            AppDomain.CurrentDomain.UnhandledException += (s, e) => logger.Fatal("Fatal Exception occures crash", e.ExceptionObject);
        }

        private bool TryGetFromFile<T>(string path, out T value) where T : new()
        {
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                File.WriteAllText(fileInfo.FullName,
                    JsonConvert.SerializeObject(new T(), Formatting.Indented));

                value = default;
                return false;
            }

            value = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileInfo.FullName));
            return true;
        }
    }
}

