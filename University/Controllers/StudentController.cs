using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;

        public StudentController(IGroupRepository groupRepository, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
        }

        [Route("All")]
        public async Task<IActionResult> ShowAllPage()
        {
            var students = await _studentRepository.GetAll();

            return View(students);
        }

        [Route("Group")]
        public async Task<IActionResult> ShowByGroup(int id)
        {
            var students = await _studentRepository.GetAllByGroup(id);

            return View("ShowAllPage", students);
        }

        [Route("Add")]
        public IActionResult AddPage()
        {
            ViewBag.Title = "New student";

            return View();
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditPage(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            ViewBag.Title = "Edit student";
            var student = await _studentRepository.GetById(id.Value);

            return View("AddPage", student);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddStudent(Student student, int groupId)
        {
            if (student == null)
                return NotFound();

            if (ModelState.IsValid == false || groupId == default)
            {
                ViewBag.Title = "Data is not valid";
                return View("AddPage", student);
            }

            student.Group = await _groupRepository.GetById(groupId);
            await _studentRepository.AddOrUpdate(student);

            return Redirect("~/Student/All");
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveStudent(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            await _studentRepository.DeleteById(id.Value);

            return View("Partial/TableBodyPage", await _studentRepository.GetAll());
        }
    }
}
