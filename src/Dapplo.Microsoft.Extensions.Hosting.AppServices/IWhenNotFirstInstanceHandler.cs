// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Dapplo.Microsoft.Extensions.Hosting.AppServices
{
    /// <summary>
    /// This is the interface for IWhenNotFirstInstanceHandler
    /// </summary>
    public interface IWhenNotFirstInstanceHandler
    {
        /// <summary>
        /// The method which is called when the mutex cannot be locked 
        /// </summary>
        Task WhenNotFirstInstanceAsync();
    }
}
