using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This booking controller is used to handle everything about bookings or showing bookings to the users
    /// This controller calls the bookingReposiotry and customerReposiotry to process all its intended tasks
    /// </summary>
    [Authorize] // the controller and its views are restricted to logged in users
    public class BookingsController : Controller
    {
        //ref to repositories
        private readonly IBookingsRepository _bookingsRepository;
        private readonly ICustomerRepository _customerRepository;

        //constructor initialisation 
        public BookingsController()
        {
            _bookingsRepository = new BookingsRepository();
            _customerRepository = new CustomerRepository();
        }

        //this action method is only for a user with Customer role
        //it calls the booking reppository's methods to complete the booking process 
        //it is responsible for processing customer's bookings 
        [Authorize(Roles = "Customer")]
        public ActionResult ProcessBooking(int id)
        {
            var check = _bookingsRepository.AddBooking(id);
            if (check == false)
            {
                ViewData["Message"] = "You have already booked this class/session.";
                var classSessions = GetListOfClassSession();
                return View("AvailableBookings", classSessions);
            }
            var userBookings = _bookingsRepository.UserBookings();
            return RedirectToAction("YourBookings", userBookings);
        }

        //This action method is restricted to customer
        //This method shows all the available bookings 
        [Authorize(Roles = "Customer")]
        public ActionResult AvailableBookings()
        {
            var classSessions = GetListOfClassSession();
            return View(classSessions);
        }

        //This action method is restricted to customer
        //This method shows a customer all his bookings 
        [Authorize(Roles = "Customer")]
        public ActionResult YourBookings()
        {
            var userBookings = UserBookings();
            return View(userBookings);
        }

        //This action method is restricted to customer
        //This method cancels a customer's booking if the booking 
        [Authorize(Roles = "Customer")]
        public ActionResult CancelBooking(int id)
        {
            var check = _bookingsRepository.CancelBooking(id);
            if (check == false)
            {
                ViewData["Message"] = "Something went wrong.";
                var userBookings = UserBookings();
                return View("YourBookings", userBookings);
            }
            return RedirectToAction("YourBookings");
        }

        //This action method is restricted to Tutor
        //This method shows a customer all his bookings 
        [Authorize(Roles = "Tutor")]
        public ActionResult ProcessBookACustomer(Customer customer)
        {
            if (customer.UserId == null)
                return RedirectToAction("BookACustomer", "Tutors");

            var classSessions = GetListOfClassSession();
            var cust = _customerRepository.GetCustomerByUserId(customer.UserId);

            var vModel = new TutorBookingCustomerViewModel
            {
                ClassSessions = classSessions,
                Customer = cust
            };
            return View(vModel);
        }

        //This action method is restricted to Tutor
        //This method is used when a tutor books an event for a customer 
        [Authorize(Roles = "Tutor")]
        public ActionResult ProcessTutorCustomerBooking(TutorBookingCustomerViewModel vModel, int id)
        {
            if (vModel.Customer.UserId == null || id == 0)
                return RedirectToAction("BookACustomer", "Tutors");

            var check = _bookingsRepository.TutorAddCustomerBooking(vModel.Customer.UserId, id);

            var classSessions = GetListOfClassSession();
            var cust = _customerRepository.GetCustomerByUserId(vModel.Customer.UserId);
            var vvModel = new TutorBookingCustomerViewModel
            {
                ClassSessions = classSessions,
                Customer = cust
            };

            if (check == false)
            {
                ViewData["Message"] = "The customer already booked this class/session.";

                return View("ProcessBookACustomer", vvModel);
            }

            ViewData["Message"] = "Event booked successfully.";
            return View("ProcessBookACustomer", vvModel);
        }

        //This method gets a list of ClassSessionViewModel 
        private List<ClassSessionViewModel> GetListOfClassSession()
        {
            var events = _customerRepository.GetAllEvents();
            ClassSessionViewModel vModel = null;
            List<ClassSessionViewModel> classSessions = new List<ClassSessionViewModel>();
            List<ClassSkill> skills = null;
            foreach (var classSession in events)
            {
                skills = _customerRepository.GetSelectedSkills(classSession.Id);
                vModel = new ClassSessionViewModel
                {
                    Id = classSession.Id,
                    StartDate = classSession.StartDate,
                    EndDate = classSession.EndDate,
                    Title = classSession.Title,
                    UserId = classSession.UserId,
                    TutorId = classSession.TutorId,
                    Tutor = classSession.Tutor,
                    ClassTypeId = classSession.ClassTypeId,
                    ClassType = classSession.ClassType,
                    RoomId = classSession.RoomId,
                    Room = classSession.Room,
                    ClassSkills = skills,
                    SpaceLeft = classSession.AvailableSpace
                };
                classSessions.Add(vModel);
            }
            return classSessions;
        }

        //This method gets a list of customer's personal booking
        //This method also sets the CanCancel property of booking model to true or false
        //depending on if the event has more than 3 or not
        private List<Booking> UserBookings()
        {
            var userBookings = _bookingsRepository.UserBookings();
            DateTime start;
            DateTime currentDate = DateTime.Now.Date;
            foreach (var booking in userBookings)
            {
                start = booking.Class.StartDate;
                var numberOfDaysLeftToCancel = (start - currentDate).Days;
                if (numberOfDaysLeftToCancel > 3)
                    booking.CanCancel = true;
                else
                    booking.CanCancel = false;
            }
            return userBookings;
        }
    }
}