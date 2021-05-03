using System.Net.Http;
using System.Threading.Tasks;
using University.IntegrationTests.Extensions;
using Xunit;

namespace University.IntegrationTests
{
    public class GeneralTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GeneralTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/Course/Add")]
        [InlineData("/Group/Add")]
        [InlineData("/Student/Add")]
        public async Task AddPageTest(string uri)
        {
            var responseString = await _client.GetResponseFromRequest(HttpMethod.Get, uri);

            Assert.NotEmpty(responseString);
        }
    }
}
