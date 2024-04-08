using MassTransit.Testing;
using MassTransit;
using Xunit;
using SpaceBattle.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace SpaceBattle.Test;

public class RabbitMqTest
{
    [Fact(DisplayName = "POST запрос,в котором должна быть публикация сообщения, проверяем его у подписчика")]
    public async Task WebHost_Post_ConsumedTrue()
    {
        await using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services => services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumer<MyConsumer>();
            })));

        var testHarness = application.Services.GetTestHarness();

        using var client = application.CreateClient();

        var httpResponse = await client.PostAsync("RabbitMq", JsonContent.Create(new MyMessage
        {
            GameId = Guid.NewGuid(),
            OperationId = Guid.NewGuid(),
            ObjectId = Guid.NewGuid(),
            Argumets = new { }
        }));

        Assert.True(httpResponse.IsSuccessStatusCode);
        Assert.True(await testHarness.Consumed.Any<MyMessage>());
    }
}