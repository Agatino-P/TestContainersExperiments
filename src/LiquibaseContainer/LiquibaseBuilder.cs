namespace TestContainers.Liquibase;

public sealed class LiquibaseBuilder : ContainerBuilder<LiquibaseBuilder, LiquibaseContainer, LiquibaseConfiguration>
{

    public override LiquibaseContainer Build()
    {
        Validate();
        return new LiquibaseContainer(DockerResourceConfiguration, TestcontainersSettings.Logger);
    }

    public LiquibaseBuilder WithChangelogRelativePath(string changelogRelativePath)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: changelogRelativePath))
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, changelogRelativePath), "/liquibase/changelog");

    public LiquibaseBuilder WithScriptsRelativePath(string scriptsRelativePath)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(scriptsRelativePath: scriptsRelativePath))
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, scriptsRelativePath), "/liquibase/scripts");

    public const string LiquibaseImage = "liquibase/liquibase";
    public const string DefaultChangelogRelativePath = "liquibase\\changelog";
    public const string DefaultScriptsRelativePath = "liquibase\\scripts";

    protected override LiquibaseConfiguration DockerResourceConfiguration { get; } //= default!;
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

        return builder;
    }
    protected override void Validate() => base.Validate();


    public LiquibaseBuilder() : this(new LiquibaseConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }

    private LiquibaseBuilder(LiquibaseConfiguration resourceConfiguration) : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    protected override LiquibaseBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration) 
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(resourceConfiguration));

    protected override LiquibaseBuilder Clone(IContainerConfiguration resourceConfiguration) 
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(resourceConfiguration));

    protected override LiquibaseBuilder Merge(LiquibaseConfiguration oldValue, LiquibaseConfiguration newValue) 
        => new LiquibaseBuilder(new LiquibaseConfiguration(oldValue, newValue));
}