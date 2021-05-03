using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using University.Models;
using University.Repositories.Interfaces;

namespace University.Controllers
{
    [Route("[controller]")]
    public class CourseController: Controller
    {
        private readonly ICourseRepository _courseRepository;
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [Route("All")]
        public async Task<IActionResult> ShowAllPage()
        {
            var courses = await _courseRepository.GetAll();

            return View(courses);
        }

        [Route("Add")]
        public IActionResult AddPage()
        {
            ViewBag.Title = "New course";
            return View();
        }

        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> EditPage(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            ViewBag.Title = "Edit course";
            var course = await _courseRepository.GetById(id.Value);
            return View("AddPage", course);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddCourse(Course course)
        {
            if (course == null)
                return NotFound();

            if (ModelState.IsValid == false)
            {
                ViewBag.Title = "Data is not valid";
                return View("AddPage", course);
            }

            await _courseRepository.AddOrUpdate(course);

            return Redirect("~/Course/All");
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveCourse(int? id)
        {
            if (id.HasValue == false)
                return NotFound();

            try
            {
                await _courseRepository.DeleteById(id.Value);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Error", "you cannot delete a course while there are groups in that course.");
            }

            return View("Partial/TableBodyPage", await _courseRepository.GetAll());
        }
    }
}
