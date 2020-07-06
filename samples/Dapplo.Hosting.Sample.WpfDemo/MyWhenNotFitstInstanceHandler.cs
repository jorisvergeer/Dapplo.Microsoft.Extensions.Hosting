// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Dapplo.Microsoft.Extensions.Hosting.AppServices;
using JKang.IpcServiceFramework.Client;

namespace Dapplo.Hosting.Sample.WpfDemo
{
    public class MyWhenNotFirstInstanceHandler : IWhenNotFirstInstanceHandler
    {
        private readonly ILogger<MyWhenNotFirstInstanceHandler> logger;
        private readonly IIpcClientFactory<IInterProcessService> ipcClientFactory;
        private readonly IHostEnvironment hostEnvironment;

        public MyWhenNotFirstInstanceHandler(
            ILogger<MyWhenNotFirstInstanceHandler> logger,
            IIpcClientFactory<IInterProcessService> ipcClientFactory,
            IHostEnvironment hostEnvironment)
        {
            this.logger = logger;
            this.ipcClientFactory = ipcClientFactory;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task WhenNotFirstInstanceAsync()
        {
            // This is called when an instance was already started, this is in the second instance
            logger.LogWarning("Application {0} already running.", hostEnvironment.ApplicationName);

            // Notyfy other instance
            var client = ipcClientFactory.CreateClient("client1");
            await client.InvokeAsync(x => x.ActivateOldInstance());
        }
    }
}
