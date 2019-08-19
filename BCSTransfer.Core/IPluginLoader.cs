using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public interface IPluginLoader
    {
        Dictionary<string, PluginLoader.PluginInformation> Plugins { get; }

        void Dispose();
        Task LoadPlugins(Dictionary<string, string> plugins);
    }
}