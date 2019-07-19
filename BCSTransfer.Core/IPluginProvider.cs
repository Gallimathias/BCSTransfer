using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public interface IPluginProvider : IDisposable
    {
        string PluginNamespace { get; }

        void Register(ITypeContainer typeContainer, string pluginNamespace);

        Task Initalize();        
        Task UnInitalize();
    }
}
