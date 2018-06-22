using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a class controller that handles everything about events scheduled by a tutor 
    /// These events can be classes or sessions 
    /// This whole controller is only restricted to a Tutor 
    /// This controller utilises the ClassReposiotry class to complete all its tasks
    /// </summary>
    [Authorize(Roles = "Tutor")]
    public class ClassController : Controller
    {
        private readonly IClassRepository _classRepository; //ref to the classRepository

        //Constructor
        public ClassController()
        {
            _classRepository = new ClassRepository();
        }

        //the index action method which renders the main canlendar view
        public ActionResult Index()
        {
            return View();
        }

        //A method that gets all the classes or sessions for a logged in user 
        //an ajax request is sent here from the view in the Views -> index file    
        [HttpPost]
        public JsonResult GetEvents()
        {
            var events = _classRepository.GetAllEventsForLoggedInUser();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //A method that saves classes or sessions  
        //an ajax request is sent here from the view in the Views -> index file    
        [HttpPost]
        public JsonResult SaveEvent(Class model)
        {
            var status = _classRepository.Save(model);
            return new JsonResult { Data = new { status = status } };
        }

        //an ajax request is sent here from the view in the Views -> index file
        //when a tutor schedule an event the room availibility is first checked to ensure it is free
        [HttpPost]
        public JsonResult CheckRoomAvailibility(Class model)
        {
            var status = _classRepository.CheckRoomAvailibility(model);
            return new JsonResult { Data = new { status = status } };
        }

        //A method that deletes class or session
        //an ajax request is sent here from the view in the Views -> index file    
        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var status = _classRepository.Delete(id);
            return new JsonResult { Data = new { status = status } };
        }
    }
}