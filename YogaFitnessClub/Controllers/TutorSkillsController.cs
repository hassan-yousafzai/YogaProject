using System.Web.Mvc;
using YogaFitnessClub.Helper;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a tutorskills controller that handles everything about tutorsskills e.g some CRUD operations 
    /// This whole controller is only restricted to sdmins but some methods are allowed to be accessed by a tutor 
    /// This controller utilises the TutorSkillRepository to complete all its tasks
    /// </summary>
    [Authorize]
    public class TutorSkillsController : Controller
    {
        private readonly ITutorSkillRepository _tutorSkillRepository;

        public TutorSkillsController()
        {
            _tutorSkillRepository = new TutorSkillRepository();
        }

        //an ajax request is sent here to get all skills for a logged in tutor that has been assigned to the tutor
        //data is sent back in json form
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult FetchTutorSkills()
        {
            var loggedInUserId = Utility.GetLoggedInUserId();
            var data = _tutorSkillRepository.GetTutorSkills(loggedInUserId);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //displays the tutor skill page 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        //displays the tutorskills form
        [Authorize(Roles = "Admin")]
        public ActionResult TutorSkillForm()
        {
            return View();
        }

        //allows the admin to add a skill to a tutor, 
        //checks for validation to ensure a skill is not added to a tutor twice 
        //an ajax request is sent here with its data to add a skill to tutor 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult AddSkill(TutorSkill model)
        {
            var status = _tutorSkillRepository.AddTutorSkill(model);
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //allows the admin to add a get all the skills 
        //an ajax request is sent here to get all the skills and then the data is sent back in json format
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult GetAllSkills()
        {
            var skills = _tutorSkillRepository.GetAllSkills();
            return new JsonResult { Data = skills, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //an ajax request is sent here to get all the tutors in the system 
        //data is then sent back in json format
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult GetAllTutors()
        {
            var tutors = _tutorSkillRepository.GetAllTutors();
            return new JsonResult { Data = tutors, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

    }
}