﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Inceptum.AppServer.Configuration;
using Inceptum.AppServer.Hosting;
using Inceptum.AppServer.Management.Handlers;
using Inceptum.AppServer.Management.Resources;
using Inceptum.AppServer.Model;
using OpenRasta.Codecs;
//using OpenRasta.Codecs.Razor;
using OpenRasta.Configuration;
using OpenRasta.IO;

namespace Inceptum.AppServer.Management.OpenRasta
{
    public class Configurator : IConfigurationSource
    {
        #region IConfigurationSource Members

        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<ServerError>().WithoutUri.TranscodedBy<NewtonsoftJsonCodec>();
                ResourceSpace.Has.ResourcesOfType<InstanceCommand>().WithoutUri.TranscodedBy<NewtonsoftJsonCodec>();
                ResourceSpace.Has.ResourcesOfType<string>().WithoutUri.TranscodedBy<UtfTextPlainCodec>();
                ResourceSpace.Uses.UriDecorator<SplitParamsUriDecorator>();


                ResourceSpace.Has
                    .ResourcesOfType<Application[]>()
                    .AtUri("api/applications")
                    .And.AtUri("api/applications/rediscover").Named("rediscover")
                    .HandledBy<ApplicationsHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();       
                
                ResourceSpace.Has
                    .ResourcesOfType<Application>()
                    .AtUri("api/applications/{vendor}/{application}")
                    .HandledBy<ApplicationsHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<ApplicationInstanceInfo[]>()
                    .AtUri("api/applications/{vendor}/{application}/instances")
                    .And.AtUri("api/instances")
                    .HandledBy<InstancesHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();
                


                ResourceSpace.
                    Has.ResourcesOfType<ApplicationInstanceInfo>()
                    .AtUri("api/instance") //For post (as it processes new instance there is no id yet)
                    .And.AtUri("api/instance/{instance}")
                    .And.AtUri("api/instance/{instance}/start").Named("start")
                    .And.AtUri("api/instance/{instance}/stop").Named("stop")
                    .And.AtUri("api/instance/{instance}/restart").Named("restart")
                    .HandledBy<InstancesHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();
                
                ResourceSpace.Has
                    .ResourcesOfType<CommandResult>()
                    .AtUri("api/instance/{instance}/command").Named("command")
                    .HandledBy<InstancesHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<HostInfo>()
                    .AtUri("api/host")
                    .HandledBy<HostHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<ConfigurationInfo[]>()
                    .AtUri("api/configurations")
                    .HandledBy<ConfigurationsHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<ConfigurationInfo>()
                    .AtUri("api/configurations/{configuration}")
                    .HandledBy<ConfigurationsHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<BundleInfo>()
                    .AtUri("api/configurations/{configuration}/{bundle}")
                    .HandledBy<ConfigurationsHandler>()
                    .TranscodedBy<NewtonsoftJsonCodec>();

                ResourceSpace.Has
                    .ResourcesOfType<IDownloadableFile>()
                    .AtUri("api/configurations/{configuration}/export").Named("export")
                    .And.AtUri("api/configurations/{configuration}/import").Named("import")
                    .HandledBy<ConfigurationsHandler>()
                    .TranscodedBy<ApplicationOctetStreamCodec>();

                ResourceSpace.Has.ResourcesOfType<object>()
                    .AtUri("/configuration/{configuration}/{bundle}").Named("configBundle")
                    .And.AtUri("/configuration/{configuration}/{bundle}/{overrides}").Named("configBundleWithOverrides")
                    .HandledBy<ConfigurationsHandler>();
            }
        }

        #endregion
    }
}