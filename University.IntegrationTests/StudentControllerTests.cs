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
    public class StudentControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _courseRepository = factory.Resolve<ICourseRepository>();
            _groupRepository = factory.Resolve<IGroupRepository>();
            _studentRepository = factory.Resolve<IStudentRepository>();
        }

        [Fact]
        public async Task EditPageTest()
        {
            await _studentRepository.AddStudents(_groupRepository, _courseRepository, 1);
            var students = await _studentRepository.GetAll();
            var student = students.First();

            var dictionary = new Dictionary<string, string>
            {
                { "id", $"{student.Id}" }
            };

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Student/Edit", dictionary);

            Assert.Contains(student.FirstName, responseString);
            Assert.Contains(student.LastName, responseString);
        }

        [Fact]
        public async Task ShowAllPageTest()
        {
            await _studentRepository.AddStudents(_groupRepository, _courseRepository, 5);

            var responseString = await _client.GetResponseFromRequest(HttpMethod.Get, "/Student/All");
            var students = await _studentRepository.GetAll();

            foreach (var student in students)
            {
                Assert.Contains(student.FirstName, responseString);
                Assert.Contains(student.LastName, responseString);
                Assert.Contains(student.Group.Name, responseString);
                Assert.Contains(student.Group.Course.Name, responseString);
            }
        }

        [Fact]
        public async Task AddGroupTest()
        {
            await _studentRepository.AddStudents(_groupRepository, _courseRepository, 5);
            var groups = await _groupRepository.GetAll() as List<Group>;

            var faker = new Faker<Student>()
                .RuleFor(s => s.FirstName, f => f.Random.String2(25))
                .RuleFor(s => s.LastName, f => f.Random.String2(50))
                .RuleFor(s => s.Group, f => f.Random.ListItem(groups));

            faker.Generate(5).ForEach(async (item) =>
            {
                var dictionary = new Dictionary<string, string>()
                {
                    { "FirstName", item.FirstName },
                    { "LastName", item.LastName },
                    { "Id", $"{item.Group.Id}" }
                };

                var responseString = await _client.GetResponseFromRequest(HttpMethod.Post, "/Student/Add", dictionary);

                Assert.All(dictionary.Values, (value) => responseString?.Contains(value));
            });
        }

        [Fact]
        public async Task RemoveStudentTest()
        {
            await _studentRepository.AddStudents(_groupRepository, _courseRepository, 3);
            var students = await _studentRepository.GetAll();
            var lastStudent = students.Last();

            var dictionary = new Dictionary<string, string>()
            {
                { "id", $"{lastStudent.Id}" }
            };

            _ = await _client.GetResponseFromRequest(HttpMethod.Post, "/Student/Remove", dictionary);
            students = await _studentRepository.GetAll();

            Assert.DoesNotContain(lastStudent.Id, students.Select(s => s.Id));
        }
    }
}
