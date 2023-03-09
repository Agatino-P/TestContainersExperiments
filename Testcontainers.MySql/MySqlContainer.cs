namespace Testcontainers.MySql;

/// <inheritdoc cref="DockerContainer" />
[PublicAPI]
public sealed class MySqlContainer : DockerContainer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    /// <param name="logger">The logger.</param>
    public MySqlContainer(MySqlConfiguration configuration, ILogger logger)
        : base(configuration, logger)
    {
    }
}