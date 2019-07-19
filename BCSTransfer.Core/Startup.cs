using System;
using System.Collections.Generic;
using System.Text;

namespace BCSTransfer.Core
{
    public static class Startup
    {
        public static void Register(ITypeContainer typeContainer)
        {
            typeContainer.Register<TransferClient, TransferClient>(InstanceBehaviour.Singleton);
            typeContainer.Register<ITransferClient, TransferClient>(InstanceBehaviour.Singleton);
        }
    }
}
