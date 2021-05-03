using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("[controller]")]
    public class GroupController: Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;

        public GroupController(ICourseRepository courseRepository, IGroupRepository groupRepository, IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _studentRepository = studentRepository;
        }
        
        [Route("All")]
        public async Task<IActionResult> ShowAllPage()
        {
            var groups = await _groupRepository.GetAll();

            return View(groups);
        }

        [Route("Course")]
        public async Task<IActionResult> ShowByCourse(int id)
        {
            var groups = await _groupRepository.GetAllByCourse(id);

            return View("ShowAllPage", groups);
        }

        [Route("Add")]
        public IActionResult AddPage()
        {
            ViewBag.Title = "New group";

            return View();
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditPage(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            ViewBag.Title = "Edit group";
            var group = await _groupRepository.GetById(id.Value);
            return View("AddPage", group);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddGroup(Group group, int courseId)
        {
            if (group == null)
                return NotFound();

            if (ModelState.IsValid == false || courseId == default)
            {
                ViewBag.Title = "Data in not valid";
                return View("AddPage", group);
            }
            
            group.Course = await _courseRepository.GetById(courseId);
            await _groupRepository.AddOrUpdate(group);

            return Redirect("~/Group/All");
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveGroup(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            try
            {
                await _groupRepository.DeleteById(id.Value);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Error", "You cannot delete a group while students are in the group");
            }

            return View("Partial/TableBodyPage", await _groupRepository.GetAll());
        }
    }
}
