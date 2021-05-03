using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Db;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Repositories
{
    public class GroupRepository: Repository<Group>, IGroupRepository
    {
        public GroupRepository(UniversityContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Group>> GetAllByCourse(int courseId)
        {
            var result = await DbSet
                .Include(p => p.Course)
                .Where(c => c.Course.Id == courseId)
                .ToListAsync();

            return result;
        }
    }
}
