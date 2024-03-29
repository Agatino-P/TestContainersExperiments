using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using TestContainers.Liquibase;
using Testcontainers.MySql;

namespace TestContainersExperiments;

public class UnitTest1
{
    private INetwork network = new NetworkBuilder().WithName(Guid.NewGuid().ToString("D")).Build();

    private MySqlContainer mySqlContainer;

    public UnitTest1()
    {
        MySqlBuilder mySqlBuilder = new MySqlBuilder()
           .WithImage("percona:8.0.27-18")           
           .WithName("percona")
           .WithPortBinding(3306, 3306)
           //.WithPortBinding(3306, true)
           .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
           .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
           .WithNetwork(network)
           .WithNetworkAliases("percona")
           .WithCleanUp(true)
           ;
        mySqlContainer = mySqlBuilder.Build();
        //string connectionString = mySqlContainer.GetConnectionString();

    }

    [Fact]
    public async void WithVolumes()
    {
        await network.CreateAsync();
        await mySqlContainer.StartAsync(default);

        string pwd = AppContext.BaseDirectory;
        string localChangelogPath = $"{pwd}liquibase\\changelog";
        string localScriptsPath = $"{pwd}liquibase\\scripts";
        string[] liquibaseCommands = { "update", @"--defaultsFile=/liquibase/changelog/liquibase.properties" };
        string logMessage = "Liquibase command 'update' was executed successfully.";


        IContainer myLiquibase = new ContainerBuilder()
          .WithImage("liquibase/liquibase")
          .WithBindMount(localChangelogPath, "/liquibase/changelog")
          .WithBindMount(localScriptsPath, "/liquibase/scripts")
          .WithCommand(liquibaseCommands)
          .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(logMessage))
          .WithNetwork(network)
          .Build();

          //.WithName("LiquibaseContainer")
          //.WithNetworkAliases("liquibase")

        //Given that the stase doesn't seem reliable a solution could be to wait for the log message with a timeot cancellation token
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(150));
        try
        {
            await myLiquibase.StartAsync(cts.Token);
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    [Fact]
    public async Task WithModuleAsync()
    {

        await network.CreateAsync();
        await mySqlContainer.StartAsync(default);

        string[] liquibaseCommands = { "update", @"--defaultsFile=/liquibase/changelog/liquibase.properties" };
        string logMessage = "Liquibase command 'update' was executed successfully.";


        LiquibaseContainer liquibase = new LiquibaseBuilder()
          //.WithCommand(liquibaseCommands)
          //.WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(logMessage))
          .WithNetwork(network)
          .Build();
        /* 
         * Devono restare
        classpath: /liquibase/changelog/mysql-connector-j-8.0.31.jar
        driver: com.mysql.cj.jdbc.Driver
        searchPath:/liquibase/changelog,/liquibase/scripts
        
        Vanno testati
        username: root
        password: 123456Ab
        url: jdbc:mysql://percona:3306/sms_provider_api_liquibase?createDatabaseIfNotExist=true
        changelogFile: changelog.xml
        loglevel: INFO
         * 
         */

        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(15));
        try
        {
            await liquibase.StartAsync(cts.Token);

        }
        catch (Exception ex)
        {

            throw;
        }

    }

}
