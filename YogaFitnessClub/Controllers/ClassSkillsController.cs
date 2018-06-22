using System.Web.Mvc;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a ClassSkills controller that handles everything about events classskills 
    /// This whole controller is only restricted to a Tutor 
    /// This controller utilises the ClassSkillsRepository to complete all its tasks
    /// </summary>
    [Authorize]
    public class ClassSkillsController : Controller
    {
        private readonly IClassSkillsRepository _classSkillsRepository;//ref to the repository

        //Constructor
        public ClassSkillsController()
        {
            _classSkillsRepository = new ClassSkillsRepository();
        }

        //A method that gets all the selectedSkills by the class id
        //an ajax request is sent here from the view in the Views -> index file 
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult GetClassSelectedSkills(int id)
        {
            var data = _classSkillsRepository.GetClassSelectedSkills(id);
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }
    }
}