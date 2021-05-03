using System.Collections.Generic;
using System.Threading.Tasks;
using University.Models;

namespace University.Repositories.Interfaces
{
    public interface IStudentRepository: IRepository<Student>
    {
        public Task<IEnumerable<Student>> GetAllByGroup(int groupId);
    }
}
