using System.Net.Http.Json;
using GenealogyApp.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

public class GenealogyEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public GenealogyEndpointsTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Signup_Login_Then_CreateMember_And_LinkParent_Succeeds()
    {
        var client = _factory.CreateClient();

        // 1) Signup
        var signup = await client.PostAsJsonAsync("/auth/signup", new
        {
            username = "john",
            email = "john@doe.com",
            phoneNumber = "000",
            password = "Strong!Passw0rd"
        });
        signup.EnsureSuccessStatusCode();

        // 2) Login
        var login = await client.PostAsJsonAsync("/auth/login", new
        {
            usernameOrEmail = "john",
            password = "Strong!Passw0rd"
        });
        login.EnsureSuccessStatusCode();
        var payload = await login.Content.ReadFromJsonAsync<dynamic>();
        string token = payload!.token;
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // 3) Créer 2 personnes
        var m1 = await client.PostAsJsonAsync("/api/people", new { firstName = "Dad" });
        var m2 = await client.PostAsJsonAsync("/api/people", new { firstName = "Kid" });
        m1.EnsureSuccessStatusCode(); m2.EnsureSuccessStatusCode();
        var p1 = await m1.Content.ReadFromJsonAsync<dynamic>();
        var p2 = await m2.Content.ReadFromJsonAsync<dynamic>();

        Guid parentId = Guid.Parse((string)p1!.memberId.ToString());
        Guid childId = Guid.Parse((string)p2!.memberId.ToString());

        // 4) Lier ParentOf
        var rel = await client.PostAsJsonAsync("/api/relationships/parent-of", new { parentId, childId });
        rel.EnsureSuccessStatusCode();

        // 5) Vérifier ancêtres du child
        var anc = await client.GetAsync($"/api/tree/{childId}/ancestors");
        anc.EnsureSuccessStatusCode();
    }
}
