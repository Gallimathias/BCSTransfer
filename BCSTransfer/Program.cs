using BCSTransfer.Core;
using BCSTransfer.Core.PretixModel;
using BCSTransfer.Runtime;
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
            using (var cancelationTokenSource = new CancellationTokenSource())
            using (var service = new BCSTransferService())
            {
                Console.CancelKeyPress += (s, e) => cancelationTokenSource.Cancel();
                var task = Task.Run(() => service.Run(cancelationTokenSource.Token));
                task.Wait();
            }
        }
    }
}
