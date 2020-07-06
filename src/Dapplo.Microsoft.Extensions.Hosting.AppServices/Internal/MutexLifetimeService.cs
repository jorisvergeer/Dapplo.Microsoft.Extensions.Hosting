// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices.Internal
{
    /// <summary>
    /// This maintains the mutex lifetime
    /// </summary>
    internal class MutexLifetimeService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IHostApplicationLifetime hostApplicationLifetime;
        private readonly IMutexBuilder mutexBuilder;
        private readonly IServiceProvider provider;
        private ResourceMutex resourceMutex;

        public MutexLifetimeService(ILogger<MutexLifetimeService> logger, IHostEnvironment hostEnvironment, IHostApplicationLifetime hostApplicationLifetime, IMutexBuilder mutexBuilder, IServiceProvider provider)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
            this.hostApplicationLifetime = hostApplicationLifetime;
            this.mutexBuilder = mutexBuilder;
            this.provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.resourceMutex = ResourceMutex.Create(null, this.mutexBuilder.MutexId, this.hostEnvironment.ApplicationName, this.mutexBuilder.IsGlobal);

            this.hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            if (!this.resourceMutex.IsLocked)
            {
                var task = this.mutexBuilder.WhenNotFirstInstance?.Invoke(this.hostEnvironment, this.logger, this.provider);

                this.logger.LogDebug("Application {0} already running, stopping application.", this.hostEnvironment.ApplicationName);
                this.hostApplicationLifetime.StopApplication();

                if (task != null)
                {
                    await task;
                }
            }
        }

        private void OnStopping()
        {
            this.logger.LogInformation("OnStopping has been called, closing mutex.");
            this.resourceMutex.Dispose();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
