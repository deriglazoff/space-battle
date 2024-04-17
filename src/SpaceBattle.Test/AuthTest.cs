using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;
using System.Text.Json;
using System.Net;

namespace SpaceBattle.Test;

public class AuthTest
{
    [Fact(DisplayName = "Создаем игру, получаем токен и проходим авторизацию")]
    public async Task CreateToken_Verify()
    {
        await using var application = new WebApplicationFactory<Auth.Program>();

        var user = "Vlad";
        using var client = application.CreateClient();

        var httpResponse = await client.PostAsync("Game", JsonContent.Create(new List<string>
        {
            user
        }));
        httpResponse.EnsureSuccessStatusCode();
        var gameId = JsonSerializer.Deserialize<string>(await httpResponse.Content.ReadAsStringAsync());

        httpResponse = await client.GetAsync($"Game?gameId={gameId}&user={user}");

        httpResponse.EnsureSuccessStatusCode();
        var response = await httpResponse.Content.ReadAsStringAsync();

        var token = JsonSerializer.Deserialize<Dictionary<string, string>>(response).First(x => x.Key == "access_token").Value;
        client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
        httpResponse = await client.GetAsync("/Game/Verify");
        httpResponse.EnsureSuccessStatusCode();

        Assert.Equal(user, await httpResponse.Content.ReadAsStringAsync());
    }

    [Fact(DisplayName = "Ошибка 404 если нет игры")]
    public async Task Game_NotFound()
    {
        await using var application = new WebApplicationFactory<Auth.Program>();

        var user = "Vlad";
        using var client = application.CreateClient();


        var httpResponse = await client.GetAsync($"Game?gameId={Guid.NewGuid()}&user={user}");

        Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
    }
    
    [Fact(DisplayName = "Без токена стучимся к защищенному методу")]
    public async Task Token_Verify_Unauthorized()
    {
        await using var application = new WebApplicationFactory<Auth.Program>();

        using var client = application.CreateClient();

        var httpResponse = await client.GetAsync("/Game/Verify");

        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
    }


    [Fact(DisplayName = "Создаем игру, пытаемся получить токен для другого юзера, ошибка авторизации")]
    public async Task User_Notfound()
    {
        await using var application = new WebApplicationFactory<Auth.Program>();

        var user = "Vlad";
        using var client = application.CreateClient();

        var httpResponse = await client.PostAsync("Game", JsonContent.Create(new List<string>
        {
            "Maxim",
            "Alexey"
        }));
        httpResponse.EnsureSuccessStatusCode();
        var gameId = JsonSerializer.Deserialize<string>(await httpResponse.Content.ReadAsStringAsync());

        httpResponse = await client.GetAsync($"Game?gameId={gameId}&user={user}");

        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
    }
}