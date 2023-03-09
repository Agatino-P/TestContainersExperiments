using Dapper;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using MySqlConnector;
using System.Security.Cryptography;

namespace TestContainersExperiments;

public class UnitTest1
{
        private IContainer mySqlContainer = new ContainerBuilder()
          .WithImage("percona")
          .WithPortBinding(3306, true)
          .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
          .WithEnvironment("MYSQL_USER", "admin")
          .WithEnvironment("MYSQL_PASSWORD", "123456Ab")
          .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
          .WithCleanUp(true)
          .Build();

    public UnitTest1()
    {

    }

    [Fact]
    public async Task Test1Async()
    {
        await mySqlContainer.StartAsync();
        int port = mySqlContainer.GetMappedPublicPort(3306);
        string connectionString = $"Server=localhost;Port={port};User=admin;Password=123456Ab;";

        MySqlConnection mySqlConnection= new MySqlConnection(connectionString) ;
            var a = mySqlConnection.Query("Show Variables").AsList();
       
            await mySqlContainer.StopAsync();


    }
}