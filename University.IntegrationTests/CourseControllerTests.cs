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
    public class CourseControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly ICourseRepository _courseRepository;

        public CourseControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _courseRepository = factory.Resolve<ICourseRepository>();
        }

        [Fact]
        public async Task EditPageTest()
        {
            _courseRepository.AddCourses(1);
            var courses = await _courseRepository.GetAll();
            var course = courses.First();

            var dictionary = new Dictionary<string, string>
            {
                { "id", $"{course.Id}" }
            };

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Course/Edit", dictionary);

            Assert.Contains(course.Name, responseString);
            Assert.Contains(course.Description, responseString);
        }

        [Fact]
        public async Task ShowAllPageTest()
        {
            _courseRepository.AddCourses(5);

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Get, "/Course/All");
            var courses = await _courseRepository.GetAll();

            foreach (var course in courses)
            {
                Assert.Contains(course.Name, responseString);
                Assert.Contains(course.Description, responseString);
            }
        }

        [Fact]
        public void AddCourseTest()
        {
            var faker = new Faker<Course>()
                .RuleFor(s => s.Name, f => f.Random.String2(25))
                .RuleFor(s => s.Description, f => f.Random.String2(50));

            faker.Generate(5).ForEach(async (item) =>
            {
                var dictionary = new Dictionary<string, string>()
                {
                    { "Name", item.Name },
                    { "Description", item.Description }
                };

                var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Course/Add", dictionary);

                Assert.All(dictionary.Values, (value) => responseString?.Contains(value));
            });
        }

        [Fact]
        public async Task RemoveCourseTest()
        {
            _courseRepository.AddCourses(3);
            var courses = await _courseRepository.GetAll();
            var lastCourse = courses.Last();

            var dictionary = new Dictionary<string, string>()
            {
                {"id", $"{lastCourse.Id}"}
            };

            _ = await _client.GetResponseFromRequest(HttpMethod.Post, "/Course/Remove", dictionary);
            courses = await _courseRepository.GetAll();

            Assert.DoesNotContain(lastCourse.Id, courses.Select(s => s.Id));
        }
    }
}
