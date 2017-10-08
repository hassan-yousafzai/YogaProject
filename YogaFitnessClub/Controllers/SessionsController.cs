using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionsController()
        {
            _sessionRepository = new SessionsRepository();
        }

        public SessionsController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository ;
        }

        // GET: Sessions
        public ActionResult Index()
        {
            return View();
        }

        //Shows the session details by using customer id
        public ActionResult SessionDetails()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var customer = _sessionRepository.GetCustomer(loggedInUserId);

            var viewModel = new SessionViewModel
            {
                Customer =  customer,
                Sessions = _sessionRepository.GetSessions(customer.Id),
                Activities = _sessionRepository.GetAllActivities()
            };

            return View(viewModel);
        }

        public ActionResult SessionForm()
        {
            var viewModel = new SessionFormViewModel
            {
                Activities = _sessionRepository.GetAllActivities(),
            };
            return View(viewModel);
        }

        public ActionResult DeleteSession(int id)
        {
            _sessionRepository.DeleteSession(id);
            return View("SessionDetails");
        }

        public ActionResult EditSession(int id)
        {
            var session = _sessionRepository.EditSession(id);
            var loggedInUserId = User.Identity.GetUserId();

            var viewModel = new SessionFormViewModel
            {
                Sessions = session,
                Customer = _sessionRepository.GetCustomer(loggedInUserId),
                Activities = _sessionRepository.GetAllActivities()
            };


            return View("SessionForm", viewModel);
        }


        public ActionResult Save(SessionFormViewModel viewModel )
        {
           var loggedInUserId = User.Identity.GetUserId();
            var customer = _sessionRepository.GetCustomer(loggedInUserId);
      
           var session = new Session
           {
               Customer = customer,
               CustomerId = customer.Id,
               ActivityId = viewModel.Sessions.ActivityId,
               Date = viewModel.Sessions.Date,
               Time = viewModel.Sessions.Time,
               InstructorName = viewModel.Sessions.InstructorName,
               Id = viewModel.Sessions.Id
           };


             _sessionRepository.SaveSession(session);

            return RedirectToAction("Index", "Customers");
        }
    }
}