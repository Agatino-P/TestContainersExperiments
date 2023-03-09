using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace Testcontainers.MySql;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />

public sealed class MySqlBuilder : ContainerBuilder<MySqlBuilder, MySqlContainer, MySqlConfiguration>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlBuilder" /> class.
    /// </summary>
    public MySqlBuilder()
        : this(new MySqlConfiguration())
    {
        // 1) To change the ContainerBuilder default configuration override the DockerResourceConfiguration property and the "MySqlBuilder Init()" method.
        //    Append the module configuration to base.Init() e.g. base.Init().WithImage("alpine:3.17") to set the modules' default image.

        // 2) To customize the ContainerBuilder validation override the "void Validate()" method.
        //    Use Testcontainers' Guard.Argument<TType>(TType, string) or your own guard implementation to validate the module configuration.

        // 3) Add custom builder methods to extend the ContainerBuilder capabilities such as "MySqlBuilder WithMySqlConfig(object)".
        //    Merge the current module configuration with a new instance of the immutable MySqlConfiguration type to update the module configuration.

         DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlBuilder" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    private MySqlBuilder(MySqlConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;

    }

    // /// <inheritdoc />
     protected override MySqlConfiguration DockerResourceConfiguration { get; }

    // /// <summary>
    // /// Sets the MySql config.
    // /// </summary>
    // /// <param name="config">The MySql config.</param>
    // /// <returns>A configured instance of <see cref="MySqlBuilder" />.</returns>
    // public MySqlBuilder WithMySqlConfig(object config)
    // {
    //     // Extends the ContainerBuilder capabilities and holds a custom configuration in MySqlConfiguration.
    //     // In case of a module requires other properties to represent itself, extend ContainerConfiguration.
    //     return Merge(DockerResourceConfiguration, new MySqlConfiguration(config: config));
    // }

    /// <inheritdoc />
    public override MySqlContainer Build()
    {
        Validate();
        return new MySqlContainer(DockerResourceConfiguration, TestcontainersSettings.Logger);
    }

    // /// <inheritdoc />
    protected override MySqlBuilder Init()
    {
        return base.Init().WithImage("percona:latest");
    }

    // /// <inheritdoc />
    // protected override void Validate()
    // {
    //     base.Validate();
    // }

    /// <inheritdoc />
    protected override MySqlBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new MySqlConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override MySqlBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new MySqlConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override MySqlBuilder Merge(MySqlConfiguration oldValue, MySqlConfiguration newValue)
    {
        return new MySqlBuilder(new MySqlConfiguration(oldValue, newValue));
    }
}