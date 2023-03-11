

using Docker.DotNet.Models;

namespace Liquibase;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />

public sealed class LiquibaseBuilder : ContainerBuilder<LiquibaseBuilder, LiquibaseContainer, LiquibaseConfiguration>
{
    public const string LiquibaseImage = "liquibase/liquibase";
    public const string DefaultChangelogRelativePath= "liquibase\\changelog";
    public const string DefaultScriptsRelativePath = "liquibase\\scripts";

    protected override LiquibaseConfiguration DockerResourceConfiguration { get; } //= default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseBuilder" /> class.
    /// </summary>
    public LiquibaseBuilder()
        : this(new LiquibaseConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseBuilder" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    private LiquibaseBuilder(LiquibaseConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }


    public LiquibaseBuilder WithChangelogRelativePath(string changelogRelativePath)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: changelogRelativePath))
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, changelogRelativePath), "/liquibase/changelog");

    public LiquibaseBuilder WithScriptsRelativePath(string scriptsRelativePath)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(scriptsRelativePath: scriptsRelativePath))
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, scriptsRelativePath), "/liquibase/scripts");

    /// <inheritdoc />
    public override LiquibaseContainer Build()
    {
        Validate();
        return new LiquibaseContainer(DockerResourceConfiguration, TestcontainersSettings.Logger);
    }

    protected override LiquibaseBuilder Init()
    {
        string[] liquibaseCommands = { "update", @"--defaultsFile=/liquibase/changelog/liquibase.properties" };
        string logMessage = "Liquibase command 'update' was executed successfully.";


        LiquibaseBuilder builder = base.Init()
            .WithImage(LiquibaseImage)
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, DefaultChangelogRelativePath), "/liquibase/changelog")
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, DefaultScriptsRelativePath), "/liquibase/scripts")
            .WithCommand(liquibaseCommands)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(logMessage))
          ;

        //if (DockerResourceConfiguration.Network != null)
        //    builder = builder.WithNetwork(DockerResourceConfiguration.Network);
        return builder;
    }

    protected override void Validate()
    {
        base.Validate();
    }

    /// <inheritdoc />
    protected override LiquibaseBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LiquibaseConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override LiquibaseBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LiquibaseConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override LiquibaseBuilder Merge(LiquibaseConfiguration oldValue, LiquibaseConfiguration newValue)
    {
        return new LiquibaseBuilder(new LiquibaseConfiguration(oldValue, newValue));
    }
}