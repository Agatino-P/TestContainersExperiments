using Dapper;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MySqlConnector;
using Org.BouncyCastle.Crypto.Generators;
using Testcontainers.MySql;

namespace TestContainersExperiments;

public class UnitTest1
{
    //private IContainer mySqlContainer = new ContainerBuilder()
    //  .WithImage("percona")
    //  .WithPortBinding(3306, true)
    //  .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
    //  .WithEnvironment("MYSQL_USER", "admin")
    //  .WithEnvironment("MYSQL_PASSWORD", "123456Ab")
    //  .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
    //  .WithCleanUp(true)
    //  .Build();

    public UnitTest1()
    {

    }

    //[Fact]
    //public async Task Test1Async()
    //{
    //    await mySqlContainer.StartAsync();
    //    int port = mySqlContainer.GetMappedPublicPort(3306);
    //    string connectionString = $"Server=localhost;Port={port};User=admin;Password=123456Ab;";

    //    MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
    //    var a = mySqlConnection.Query("Show Variables").AsList();

    //    await mySqlContainer.StopAsync();


    //}

    [Fact]
    public async void MySqlContainer()
    {
        MySqlBuilder mySqlBuilder = new MySqlBuilder()
            .WithImage("percona:8.0.27-18")
            .WithPortBinding(31788, 3306)
            .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")

            //.WithUsername("admin")
            //.WithPassword("123456Ab")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
            .WithCleanUp(true);
        MySqlContainer mySqlContainer = mySqlBuilder.Build();
        await mySqlContainer.StartAsync(default);
        string connectionString = mySqlContainer.GetConnectionString();
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);
        var a = mySqlConnection.Query("Show Variables").AsList();

    }

    [Fact]
    public async void WithVolumes()
    {

        INetwork network = new NetworkBuilder().WithName(Guid.NewGuid().ToString("D")).Build();
        await network.CreateAsync().ConfigureAwait(false);

        MySqlBuilder mySqlBuilder = new MySqlBuilder()
           .WithImage("percona:8.0.27-18")
           .WithPortBinding(31788, 3306)
           //.WithPortBinding(3306, true)
           .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
           .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
           .WithNetwork(network)
           .WithNetworkAliases("percona")
           .WithCleanUp(true);
        MySqlContainer mySqlContainer = mySqlBuilder.Build();
        await mySqlContainer.StartAsync(default);
        //string connectionString = mySqlContainer.GetConnectionString();

        string pwd = AppContext.BaseDirectory;
        string localChangelogPath = $"{pwd}liquibase\\changelog";
        string localScriptsPath = $"{pwd}liquibase\\scripts";
        string[] liquibaseCommands = { "update", @"--defaultsFile=/liquibase/changelog/liquibase.properties" };
        string logMessage = "Liquibase command 'update' was executed successfully.";


        IContainer myLiquibase = new ContainerBuilder()
          .WithImage("liquibase/liquibase")
          //.WithPortBinding(28080, 80)
          .WithBindMount(localChangelogPath, "/liquibase/changelog")
          .WithBindMount(localScriptsPath, "/liquibase/scripts")
          .WithCommand(liquibaseCommands)
          .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged(logMessage))
          .WithNetwork(network)
          .Build();


        //Given that the stase doesn't seem reliable a solution could be to wait for the log message with a timeot cancellation token
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(15));
        try
        {
            await myLiquibase.StartAsync(cts.Token);
        }
        catch (OperationCanceledException ex)
        {
            throw;
        }
        int a = 1;
    }
}
