using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorsController()
        {
            _instructorRepository = new InstructorRepository();
        }

        public InstructorsController(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        // GET: Instructors
        public ActionResult Index()
        {
            return View();
        }


        // GET: Instrctors/1
        public ActionResult Details(int id)
        {
            var instructor = _instructorRepository.GetInstructor(id);

            if (instructor == null)
                return HttpNotFound();

            return View(instructor);
        }

        //Edit Instructors
        public ActionResult Edit(int id)
        {
            var instructor = _instructorRepository.EditInstructor(id);
            return View("InstructorForm", instructor);
        }

        [HttpPost]
        public ActionResult SaveInstructor(Instructor instructor)
        {
            _instructorRepository.SaveInstructor(instructor);

            return RedirectToAction("Index");
        }

        public ActionResult GetSessionList()
        {
            var sessionList = _instructorRepository.GetSessions();

            return View(sessionList);
        }

    }
}