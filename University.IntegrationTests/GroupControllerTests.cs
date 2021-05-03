using Bogus;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using University.IntegrationTests.Extensions;
using University.Models;
using University.Repositories.Interfaces;
using Xunit;

namespace University.IntegrationTests
{
    public class GroupControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;

        public GroupControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _courseRepository = factory.Resolve<ICourseRepository>();
            _groupRepository = factory.Resolve<IGroupRepository>();
        }

        [Fact]
        public async Task EditPageTest()
        {
            await _groupRepository.AddGroups(_courseRepository, 1);
            var groups = await _groupRepository.GetAll();
            var group = groups.First();

            var dictionary = new Dictionary<string, string>
            {
                { "id", $"{group.Id}" }
            };

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Group/Edit", dictionary);

            Assert.Contains(group.Name, responseString);
            Assert.Contains(group.Description, responseString);
        }

        [Fact]
        public async Task ShowAllPageTest()
        {
            await _groupRepository.AddGroups(_courseRepository, 5);

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Get, "/Group/All");
            var groups = await _groupRepository.GetAll();

            foreach (var group in groups)
            {
                Assert.Contains(group.Name, responseString);
                Assert.Contains(group.Description, responseString);
                Assert.Contains(group.Course.Name, responseString);
            }
        }

        [Fact]
        public async Task AddGroupTest()
        {
            _courseRepository.AddCourses(5);
            var courses = await _courseRepository.GetAll() as List<Course>;

            var faker = new Faker<Group>()
                .RuleFor(s => s.Name, f => f.Random.String2(25))
                .RuleFor(s => s.Description, f => f.Random.String2(50))
                .RuleFor(s => s.Course, f => f.Random.ListItem(courses));

            faker.Generate(5).ForEach(async (item) =>
            {
                var dictionary = new Dictionary<string, string>()
                {
                    { "Name", item.Name },
                    { "Description", item.Description },
                    { "Id", $"{item.Course.Id}" }
                };

                var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Group/Add", dictionary);

                Assert.All(dictionary.Values, (value) => responseString?.Contains(value));
            });
        }

        [Fact]
        public async Task RemoveGroupTest()
        {
            await _groupRepository.AddGroups(_courseRepository, 3);
            var groups = await _groupRepository.GetAll();
            var lastGroup = groups.Last();

            var dictionary = new Dictionary<string, string>()
            {
                {"id", $"{lastGroup.Id}"}
            };

            _ = await _client.GetResponseFromRequest(HttpMethod.Post, "/Group/Remove", dictionary);
            groups = await _groupRepository.GetAll();

            Assert.DoesNotContain(lastGroup.Id, groups.Select(s => s.Id));
        }
    }
}
