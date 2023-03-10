//using Docker.DotNet.Models;
//using DotNet.Testcontainers.Builders;
//using DotNet.Testcontainers.Configurations;
//using DotNet.Testcontainers.Containers;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TestContainersExperiments;
//internal class Modules_experiments
//{
//}



//public class MySqlContainer : DockerContainer
//{
//    public MySqlContainer(MySqlConfiguration configuration, ILogger logger) : base(configuration, logger)
//    {
//    }
//}


//public sealed class MySqlConfiguration : ContainerConfiguration
//{
//    public MySqlConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
//        : base(resourceConfiguration)
//    {
//        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
//    }

//    public MySqlConfiguration(IContainerConfiguration resourceConfiguration)
//       : base(resourceConfiguration)
//    {
//        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
//    }
//    public MySqlConfiguration(MySqlConfiguration oldValue, MySqlConfiguration newValue)
//        : base(oldValue, newValue)
//    {
//        // // Create an updated immutable copy of the module configuration.
//        // Config = BuildConfiguration.Combine(oldValue.Config, newValue.Config);
//    }
//}

//public sealed class MySqlBuilder : ContainerBuilder<MySqlBuilder, MySqlContainer, MySqlConfiguration>
//{
//    protected override MySqlConfiguration DockerResourceConfiguration { get; }


//    private MySqlBuilder(MySqlConfiguration resourceConfiguration)
//       : base(resourceConfiguration)
//    {
//        DockerResourceConfiguration = resourceConfiguration;
//    }

//    public override MySqlContainer Build()
//    {
//        Validate();
//        return new MySqlContainer(DockerResourceConfiguration, TestcontainersSettings.Logger);
//    }

//    protected override MySqlBuilder Clone(IContainerConfiguration resourceConfiguration)
//    {
//        return Merge(DockerResourceConfiguration, new MySqlConfiguration(resourceConfiguration));
//    }

//    protected override MySqlBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
//    {
//        return Merge(DockerResourceConfiguration, new MySqlConfiguration(resourceConfiguration));

//    }

//    protected override MySqlBuilder Merge(MySqlConfiguration oldValue, MySqlConfiguration newValue)
//    {
//        return new MySqlBuilder(new MySqlConfiguration(oldValue, newValue));

//    }
//}