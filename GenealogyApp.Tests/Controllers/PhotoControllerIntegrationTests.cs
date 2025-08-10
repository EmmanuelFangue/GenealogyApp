using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenealogyApp.Tests.Controllers
{
    public class PhotoControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PhotoControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPhotos_ShouldReturnOk()
        {
            var memberId = Guid.NewGuid(); // Id d’un membre existant en base
            var response = await _client.GetAsync($"/api/photo/{memberId}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
