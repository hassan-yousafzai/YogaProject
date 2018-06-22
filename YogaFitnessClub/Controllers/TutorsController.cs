using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a tutor controller that handles everything about tutors e.g all the CRUD operations 
    /// This whole controller is only restricted to tutors 
    /// This controller utilises the TutorRepository to complete all its tasks
    /// </summary>
    [Authorize(Roles = "Tutor")]
    public class TutorsController : Controller
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly ICustomerRepository _customerRepository;

        public TutorsController()
        {
            _tutorRepository = new TutorRepository();
            _customerRepository = new CustomerRepository();
        }

        //display a ReadOnlyTutorForm with the tutor details in it
        public ActionResult Index()
        {
            var tutor = _tutorRepository.GetTutor();
            return View("ReadOnlyTutorForm", tutor);
        }

        //display a tutor form
        public ActionResult TutorForm()
        {
            var tutor = _tutorRepository.GetTutor();
            return View(tutor);
        }

        //save or update a tutor details
        public ActionResult Save(Tutor tutor)
        {
            _tutorRepository.SaveTutor(tutor);
            return RedirectToAction("Index", "Tutors");
        }

        //show all tutor all the scheduled classes or sessions that shows how many 
        //customers have book and how many spaces are available
        public ActionResult ShowAllTutorBooking()
        {
            var bookings = _tutorRepository.GetAllBookings();
            return View(bookings);
        }

        //a view for tutor to book an event for a customer
        public ActionResult BookACustomer()
        {
            return View();
        }

        //a view for logging payments 
        public ActionResult LogPayment()
        {
            return View();
        }

        //this method processes the logging payment for a customer 
        //and shows the page to a tutor that can process the logging payment
        public ActionResult ProcessLogPayment(Customer customer)
        {
            if (customer.UserId == null)
                return RedirectToAction("LogPayment");

            var bookings = _tutorRepository.GetUserBookingsByUserId(customer.UserId);
            var cust = _customerRepository.GetCustomerByUserId(customer.UserId);
            var vModel = new PaymentLogViewModel
            {
                Bookings = bookings,
                Customer = cust
            };

            return View(vModel);
        }

        //the tutor is shown a list and then a tutor can process logging payments for a class or session
        //validation has been considered
        //
        public ActionResult ProcessingLogPayment(Customer customer, int id)
        {
            if (customer.UserId == null || id == 0)
                return RedirectToAction("LogPayment");

            var check = _tutorRepository.TutorLogCustomerPayment(customer.UserId, id);

            var bookings = _tutorRepository.GetUserBookingsByUserId(customer.UserId);
            var cust = _customerRepository.GetCustomerByUserId(customer.UserId);
            var vModel = new PaymentLogViewModel
            {
                Bookings = bookings,
                Customer = cust
            };

            if (check == false)
            {
                ViewData["Message"] = "Something went wrong with processing payment.";
                return View("ProcessLogPayment", vModel);
            }

            ViewData["Message"] = "Payment succesfully received!";
            return View("ProcessLogPayment", vModel);
        }
    }
}

