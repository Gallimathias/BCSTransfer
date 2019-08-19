using BCSTransfer.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BCSTransfer.Runtime
{
    public static class MockPluginManager
    {
        private static readonly ConcurrentBag<IPluginProvider> providers;

        static MockPluginManager()
        {
            providers = new ConcurrentBag<IPluginProvider>();
        }

        public static async Task Load(ITypeContainer typeContainer)
        {
            var provider = new Sqlite.PluginProvider();
            provider.Register(typeContainer, "Mock." + provider.PluginNamespace);
            await provider.Initalize();
            providers.Add(provider);
        }

        public static async Task Unload()
        {
            foreach (var provider in providers)
               await provider.UnInitalize();
        }
    }
}
