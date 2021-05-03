using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Db;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Repositories
{
    public class StudentRepository: Repository<Student>, IStudentRepository
    {
        public StudentRepository(UniversityContext db) : base(db)
        {
            
        }

        public async Task<IEnumerable<Student>> GetAllByGroup(int groupId)
        {
            var result = await DbSet
                .Include(p => p.Group)
                .ThenInclude(t => t.Course)
                .Where(c => c.Group.Id == groupId)
                .ToListAsync();

            return result;
        }
    }
}
