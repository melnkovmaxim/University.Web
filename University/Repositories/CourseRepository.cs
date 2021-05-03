using University.Db;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Repositories
{
    public class CourseRepository: Repository<Course>, ICourseRepository
    {
        public CourseRepository(UniversityContext db) : base(db)
        {
        }
    }
}
