using Microsoft.Extensions.Logging;

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
            .WithBindMount(Path.Combine(AppContext.BaseDirectory, scriptsRelativePath),   "/liquibase/scripts");

    
    public LiquibaseBuilder WithUsername(string username)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: username))
        .WithEnvironment("LIQUIBASE_COMMAND_USERNAME", username);

    public LiquibaseBuilder WithPassword(string password)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: password))
        .WithEnvironment("LIQUIBASE_COMMAND_PASSWORD", password);

    public LiquibaseBuilder WithUrl(string url)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: url))
        .WithEnvironment("LIQUIBASE_COMMAND_URL", url);
    public LiquibaseBuilder WithChangelogFile(string changelogFile)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: changelogFile))
        .WithEnvironment("LIQUIBASE_COMMAND_CHANGELOG_FILE", changelogFile);
    public LiquibaseBuilder WithLoglevel(string loglevel)
        => Merge(DockerResourceConfiguration, new LiquibaseConfiguration(changelogRelativePath: loglevel))
        .WithEnvironment("LIQUIBASE_LOG_LEVEL", loglevel);

    public const string LiquibaseImage = "liquibase/liquibase";
    public const string DefaultChangelogRelativePath = "liquibase\\changelog";
    public const string DefaultScriptsRelativePath =   "liquibase\\scripts";
    public const string DefaultUsername = "root"; //Better in a derived class
    public const string DefaultPassword = "123456Ab"; //Better in a derived class
    public const string DefaultChangelogFile = "changelog.xml";
    public const string DefaultLoglevel = "INFO";
    public readonly string[] DefaultLiquibaseCommands = { "update" , @"--defaultsFile=/liquibase/changelog/liquibase.properties"};
    
    public const string DefaultUrl = "jdbc:mysql://percona:3306/sms_provider_api?createDatabaseIfNotExist=true"; //Better in a derived class

protected override LiquibaseConfiguration DockerResourceConfiguration { get; } //= default!;
    protected override LiquibaseBuilder Init()
    {
        //string[] liquibaseCommands = { "update", @"--defaultsFile=/liquibase/changelog/liquibase.properties" };
        string logMessage = "Liquibase command 'update' was executed successfully.";

        LiquibaseBuilder builder = base.Init()
            .WithImage(LiquibaseImage)
            .WithChangelogRelativePath(DefaultChangelogRelativePath)
            .WithScriptsRelativePath(DefaultScriptsRelativePath)
            .WithUsername(DefaultUsername)
            .WithPassword(DefaultPassword)
            .WithChangelogFile(DefaultChangelogFile)
            //.WithUrl(DefaultUrl)   
            .WithLoglevel(DefaultLoglevel) 
            .WithCommand(DefaultLiquibaseCommands)
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