using System.Collections.Generic;
using System.Threading.Tasks;
using University.Models;

namespace University.Repositories.Interfaces
{
    public interface IGroupRepository: IRepository<Group>
    {
        public Task<IEnumerable<Group>> GetAllByCourse(int courseId);
    }
}
