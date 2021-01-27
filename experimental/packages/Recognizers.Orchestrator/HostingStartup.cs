// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Orchestrator;

[assembly: HostingStartup(typeof(Microsoft.Bot.Components.Recognizers.Orchestrator.HostingStartup))]

namespace Microsoft.Bot.Components.Recognizers.Orchestrator
{
    public class HostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            ComponentRegistration.Add(new OrchestratorComponentRegistration());
        }
    }
}
