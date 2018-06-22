using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using YogaFitnessClub.Models;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the tutor interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface ITutorRepository
    {
        Tutor GetTutor();
        void SaveTutor(Tutor tutor);
        List<TutorBookingsViewModel> GetAllBookings();
        List<Booking> GetUserBookingsByUserId(string id);
        bool TutorLogCustomerPayment(string customerId, int classId);
    }

    /// <summary>
    /// The tutor Repository class that implements the tutor interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class TutorRepository : ITutorRepository
    {
        private readonly ApplicationDbContext _context;

        public TutorRepository()
        {
            _context = new ApplicationDbContext();
        }

        //get list of TutorBookingsViewModel 
        public List<TutorBookingsViewModel> GetAllBookings()
        {
            List<TutorBookingsViewModel> tutorBookings = new List<TutorBookingsViewModel>();

            var loggedInUserId = Helper.Utility.GetLoggedInUserId();
            var classes = _context.Classes
                                  .Where(c => c.UserId == loggedInUserId)
                                  .Include(c => c.ClassType)
                                  .Include(r => r.Room)
                                  .Include(b => b.Room.Branch)
                                  .Include(t => t.Tutor)
                                  .ToList();

            TutorBookingsViewModel vModel;
            foreach (var cls in classes)
            {
                var bookings = _context.Bookings.Where(b => b.ClassId == cls.Id).ToList();
                vModel = new TutorBookingsViewModel
                {
                    ClassId = cls.Id,
                    Class = cls,
                    NumberOfBookings = bookings.Count
                };
                tutorBookings.Add(vModel);
            }
            return tutorBookings;
        }

        //get a tutor by logged in id
        public Tutor GetTutor()
        {
            var loggedInUserId = Helper.Utility.GetLoggedInUserId();
            return _context.Tutors.Where(t => t.UserId == loggedInUserId).SingleOrDefault();
        }

        //get user bookings by user id
        public List<Booking> GetUserBookingsByUserId(string id)
        {
            var customer = _context.Customers.Where(c => c.UserId == id).SingleOrDefault();
            return _context.Bookings
                           .Where(b => b.CustomerId == customer.Id)
                           .Include(c => c.Class)
                           .Include(c => c.Class.Tutor)
                           .Include(c => c.Class.Room)
                           .Include(c => c.Class.Room.Branch)
                           .Include(c => c.Class.ClassType)
                           .ToList();
        }

        //save a tutor or update
        public void SaveTutor(Tutor tutor)
        {
            _context.Tutors.AddOrUpdate(tutor);
            _context.SaveChanges();
        }

        //log customer payments and send them email 
        public bool TutorLogCustomerPayment(string customerId, int classId)
        {
            var customer = _context.Customers.Where(c => c.UserId == customerId).SingleOrDefault();
            var classInDb = _context.Classes
                                    .Where(c => c.Id == classId)
                                    .Include(c => c.ClassType)
                                    .Include(r => r.Room)
                                    .Include(b => b.Room.Branch)
                                    .Include(t => t.Tutor)
                                    .SingleOrDefault();
            if (classInDb != null)
            {
                var bookingCheck = _context.Bookings
                                    .Where(b => b.CustomerId == customer.Id && b.ClassId == classId)
                                    .FirstOrDefault();
                if (bookingCheck != null)
                {
                    bookingCheck.PaidOrNot = true;
                    _context.SaveChanges();
                    Helper.Utility.SendBookingMail(customer, classInDb, "You have paid for this booking!");
                    return true;
                }
            }
            return false;
        }

    }
}