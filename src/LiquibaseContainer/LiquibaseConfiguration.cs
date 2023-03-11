using DotNet.Testcontainers.Networks;

namespace Liquibase;

/// <inheritdoc cref="ContainerConfiguration" />
public sealed class LiquibaseConfiguration : ContainerConfiguration
{
    public string? ChangelogRelativePath { get;}
    public string? ScriptsRelativePath { get; } 
    


    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseConfiguration" /> class.
    /// </summary>
    /// <param name="config">The Liquibase config.</param>
    public LiquibaseConfiguration(string? changelogRelativePath=null, string? scriptsRelativePath = null)
    {
        ChangelogRelativePath = changelogRelativePath;
        ScriptsRelativePath = scriptsRelativePath;
      
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LiquibaseConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LiquibaseConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LiquibaseConfiguration(LiquibaseConfiguration resourceConfiguration)
        : this(new LiquibaseConfiguration(), resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseConfiguration" /> class.
    /// </summary>
    /// <param name="oldValue">The old Docker resource configuration.</param>
    /// <param name="newValue">The new Docker resource configuration.</param>
    public LiquibaseConfiguration(LiquibaseConfiguration oldValue, LiquibaseConfiguration newValue)
        : base(oldValue, newValue)
    {
        ChangelogRelativePath = BuildConfiguration.Combine(oldValue.ChangelogRelativePath, newValue.ChangelogRelativePath);
        ScriptsRelativePath = BuildConfiguration.Combine(oldValue.ScriptsRelativePath, newValue.ScriptsRelativePath);
//        Network = BuildConfiguration.Combine(oldValue.Network, newValue.Network);

    // // Create an updated immutable copy of the module configuration.
    // Config = BuildConfiguration.Combine(oldValue.Config, newValue.Config);
}

    // /// <summary>
    // /// Gets the Liquibase config.
    // /// </summary>
    // public object Config { get; }
}