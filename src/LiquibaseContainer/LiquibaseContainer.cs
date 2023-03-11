namespace TestContainers.Liquibase;

/// <inheritdoc cref="DockerContainer" />

public sealed class LiquibaseContainer : DockerContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LiquibaseContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    /// <param name="logger">The logger.</param>
    public LiquibaseContainer(LiquibaseConfiguration configuration, ILogger logger)
        : base(configuration, logger)
    {
    }
}