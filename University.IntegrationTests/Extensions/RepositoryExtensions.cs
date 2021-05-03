using Bogus;
using System.Collections.Generic;
using System.Threading.Tasks;
using University.Models;
using University.Repositories.Interfaces;

namespace University.IntegrationTests.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddCourses(this ICourseRepository repository, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var faker = new Faker<Course>()
                    .RuleFor(c => c.Name, f => f.Random.String2(25))
                    .RuleFor(c => c.Description, f => f.Random.String2(50));

                faker.Generate(count).ForEach(async (c) => await repository.AddOrUpdate(c));
            }
        }

        public static async Task AddGroups(this IGroupRepository groupRepository, ICourseRepository courseRepository, int count)
        {
            courseRepository.AddCourses(count);
            var courses = await courseRepository.GetAll() as List<Course>;

            for (var i = 0; i < count; i++)
            {
                var faker = new Faker<Group>()
                    .RuleFor(g => g.Name, f => f.Random.String2(25))
                    .RuleFor(g => g.Description, f => f.Random.String2(50))
                    .RuleFor(g => g.Course, f => f.Random.ListItem(courses));

                faker.Generate(count).ForEach(async (g) => await groupRepository.AddOrUpdate(g));
            }
        }

        public static async Task AddStudents(this IStudentRepository studentRepository, IGroupRepository groupRepository,
            ICourseRepository courseRepository, int count)
        {
            await groupRepository.AddGroups(courseRepository, count);
            var groups = await groupRepository.GetAll() as List<Group>;

            for (var i = 0; i < count; i++)
            {
                var faker = new Faker<Student>()
                    .RuleFor(s => s.FirstName, f => f.Random.String2(30))
                    .RuleFor(s => s.LastName, f => f.Random.String2(30))
                    .RuleFor(s => s.Group, f => f.Random.ListItem(groups));

                faker.Generate(count).ForEach(async (s) => await studentRepository.AddOrUpdate(s));
            }
        }
    }
}
