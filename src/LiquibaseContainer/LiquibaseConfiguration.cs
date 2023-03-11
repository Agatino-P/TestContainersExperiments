namespace TestContainers.Liquibase;

/// <inheritdoc cref="ContainerConfiguration" />
public sealed class LiquibaseConfiguration : ContainerConfiguration
{
    //public string? ChangelogRelativePath { get; }
    //public string? ScriptsRelativePath { get; }
    //public string? Classpath { get; }
    //public string? Driver { get; }
    //public string? Username { get; }
    //public string? Password { get; }
    //public string? Url { get; }
    //public string? ChangelogFile { get; }
    //public string? SearchPath { get; }
    //public string? Loglevel { get; }


    public LiquibaseConfiguration(string? changelogRelativePath = null,
                                  string? scriptsRelativePath = null,
                                  string? classpath = null,
                                  string? driver = null,
                                  string? username = null,
                                  string? password = null,
                                  string? url = null,
                                  string? changelogFile = null,
                                  string? searchPath = null,
                                  string? loglevel = null)

    {
        //ChangelogRelativePath = changelogRelativePath;
        //ScriptsRelativePath = scriptsRelativePath;
        //Classpath = classpath;
        //Driver = driver;
        //Username = username;
        //Password = password;
        //Url = url;
        //ChangelogFile = changelogFile;
        //SearchPath = searchPath;
        //Loglevel = loglevel;
    }
    
    public LiquibaseConfiguration(LiquibaseConfiguration oldValue, LiquibaseConfiguration newValue)
        : base(oldValue, newValue)
    {
        //ChangelogRelativePath = BuildConfiguration.Combine(oldValue.ChangelogRelativePath, newValue.ChangelogRelativePath);
        //ScriptsRelativePath = BuildConfiguration.Combine(oldValue.ScriptsRelativePath, newValue.ScriptsRelativePath);
        //Classpath = BuildConfiguration.Combine(oldValue.Classpath, newValue.Classpath);
        //Driver = BuildConfiguration.Combine(oldValue.Driver, newValue.Driver);
        //Username = BuildConfiguration.Combine(oldValue.Username, newValue.Username);
        //Password = BuildConfiguration.Combine(oldValue.Password, newValue.Password);
        //Url = BuildConfiguration.Combine(oldValue.Url, newValue.Url);
        //ChangelogFile = BuildConfiguration.Combine(oldValue.ChangelogFile, newValue.ChangelogFile);
        //SearchPath = BuildConfiguration.Combine(oldValue.SearchPath, newValue.SearchPath);
        //SearchPath = BuildConfiguration.Combine(oldValue.SearchPath, newValue.SearchPath);
    }

    public LiquibaseConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public LiquibaseConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public LiquibaseConfiguration(LiquibaseConfiguration resourceConfiguration)
        : this(new LiquibaseConfiguration(), resourceConfiguration)
    {
    }

}