// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Dapplo.Hosting.Sample.WpfDemo
{
    public class InterProcessService : IInterProcessService
    {
        private readonly ILogger<InterProcessService> logger;
        private readonly MainWindow mainWindow;

        public InterProcessService(ILogger<InterProcessService> logger, MainWindow mainWindow)
        {
            this.logger = logger;
            this.mainWindow = mainWindow;
        }

        public void ActivateOldInstance()
        {
            this.mainWindow.Dispatcher.Invoke(() => this.mainWindow.Activate());
            this.logger.LogInformation("Activated by other instance");
        }
    }
}
