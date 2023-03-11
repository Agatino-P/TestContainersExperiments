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
           .WithPortBinding(31788, 3306)
           //.WithPortBinding(3306, true)
           .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
           .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
           .WithNetwork(network)
           .WithNetworkAliases("percona")
           .WithCleanUp(true);
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


        //Given that the stase doesn't seem reliable a solution could be to wait for the log message with a timeot cancellation token
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(15));
        await myLiquibase.StartAsync(cts.Token);
    }

    [Fact]
    public async Task WithModuleAsync()
    {
        await network.CreateAsync();
        await mySqlContainer.StartAsync(default);
        LiquibaseContainer liquibase = new LiquibaseBuilder()
            //.WithChangelogRelativePath("liquibase\\changelog")
            //.WithScriptsRelativePath("liquibase\\scripts")
            .WithNetwork(network).Build();

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
