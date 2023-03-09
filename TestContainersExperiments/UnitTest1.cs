using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Security;
using Testcontainers.MySql;

namespace TestContainersExperiments;

public class UnitTest1
{
    private MySqlContainer mySqlContainer = new MySqlBuilder()
            .WithPortBinding(3306, true)
        .WithEnvironment("MYSQL_ROOT_PASSWORD", "123456Ab")
        .WithEnvironment("MYSQL_USER", "admin")
        .WithEnvironment("MYSQL_PASSWORD", "123456Ab")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
        //.WithDockerEndpoint("127.0.0.1:2375 ")
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
        await mySqlContainer.StopAsync();

    }
}