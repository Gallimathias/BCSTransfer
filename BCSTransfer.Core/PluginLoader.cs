using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    internal class PluginLoader : IDisposable, IPluginLoader
    {
        public Dictionary<string, PluginInformation> Plugins { get; }

        private readonly ITypeContainer typeContainer;

        public PluginLoader(ITypeContainer typeContainer)
        {
            this.typeContainer = typeContainer;
            Plugins = new Dictionary<string, PluginInformation>();
        }


        public async Task LoadPlugins(Dictionary<string, string> plugins)
        {
            foreach (KeyValuePair<string, string> pluginData in plugins)
            {
                var assembly = Assembly.LoadFile(pluginData.Value);
                IEnumerable<Type> pluginProviderTypes = assembly
                     .GetTypes()
                     .Where(t => typeof(IPluginProvider).IsAssignableFrom(t));

                foreach (Type providerType in pluginProviderTypes)
                {
                    var provider = Activator.CreateInstance(providerType) as IPluginProvider;
                    var fullNameSpace = $"{pluginData.Key}.{provider.PluginNamespace}";

                    provider.Register(typeContainer, fullNameSpace);
                    await provider.Initalize();

                    Plugins.Add(fullNameSpace, new PluginInformation()
                    {
                        Assembly = assembly,
                        Namespace = fullNameSpace,
                        Provider = provider
                    });
                }
            }
        }

        public void Dispose()
        {
            foreach (PluginInformation item in Plugins.Values)
            {
                var task = Task.Run(item.UnInitalize);

                try
                {
                    task.Wait();
                }
                catch (Exception)
                {
                }

                item.Dispose();
            }

            Plugins.Clear();
        }

        public class PluginInformation : IDisposable
        {
            public string Namespace { get; set; }
            public Assembly Assembly { get; set; }
            public IPluginProvider Provider { get; set; }

            public Task UnInitalize()
                => Provider.UnInitalize();

            public void Dispose()
            {
                Provider.Dispose();

                Assembly = null;
                Provider = null;
            }
        }
    }
}
