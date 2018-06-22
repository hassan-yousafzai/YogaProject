using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the bookings interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IBookingsRepository
    {
        Class GetClass(int id);
        bool AddBooking(int id);
        bool TutorAddCustomerBooking(string customerId, int classId);
        List<Booking> UserBookings();
        bool CancelBooking(int id);
    }

    /// <summary>
    /// The bookings Repository class that implements the bookings interface
    /// This repository has a direct reference to the ApplicaitonDbContext where 
    /// all the database tables can be manipulated
    /// </summary>
    public class BookingsRepository : IBookingsRepository
    {
        private readonly ApplicationDbContext _context;
        const int ZERO = 0;
        public BookingsRepository()
        {
            _context = new ApplicationDbContext();
        }

        //Add a booking to the booking table 
        //validation has been considered 
        //the avilableSpaces is decremented by 1 when a booking is recorded
        //if the booking is successfully recorded then an email is sent to the customer with details
        public bool AddBooking(int id)
        {
            var loggedInUser = Helper.Utility.GetLoggedInUserId();
            var customer = _context.Customers.Where(c => c.UserId == loggedInUser).SingleOrDefault();
            var classInDb = _context.Classes
                                    .Where(c => c.Id == id)
                                    .Include(c => c.ClassType)
                                    .Include(r => r.Room)
                                    .Include(b => b.Room.Branch)
                                    .Include(t => t.Tutor)
                                    .SingleOrDefault();
            if (classInDb != null)
            {
                var bookingCheck = _context.Bookings
                                    .Where(b => b.CustomerId == customer.Id && b.ClassId == id)
                                    .FirstOrDefault();
                if (bookingCheck == null && classInDb.AvailableSpace > ZERO)
                {
                    classInDb.AvailableSpace--;
                    Booking booking = new Booking
                    {
                        DateTime = DateTime.Now,
                        CustomerId = customer.Id,
                        ClassId = id,
                        PaidOrNot = false
                    };
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    Helper.Utility.SendBookingMail(customer, classInDb, "Your recent booking!");
                    return true;
                }
            }
            return false;
        }

        //cancel booking and remove it from the bookings table
        //validation has been put in place
        //if the booking is cancelled then send an email to the customer about it
        public bool CancelBooking(int id)
        {
            var booking = _context.Bookings.Where(b => b.Id == id).SingleOrDefault();

            if (booking != null)
            {
                var customer = _context.Customers.Where(c => c.Id == booking.CustomerId).SingleOrDefault();
                var classInDb = _context.Classes.Where(c => c.Id == booking.ClassId).SingleOrDefault();
                classInDb.AvailableSpace++;

                _context.Bookings.Remove(booking);
                _context.SaveChanges();
                Helper.Utility.SendCancellationMail(customer, "Your booking cancellation confirmation");
                return true;
            }
            return false;
        }

        //get all classes from the calsses table
        public Class GetClass(int id)
        {
            return _context.Classes
                           .Where(c => c.Id == id)
                           .Include(c => c.ClassType)
                           .Include(r => r.Room)
                           .Include(b => b.Room.Branch)
                           .Include(t => t.Tutor)
                           .SingleOrDefault();
        }

        //when a tutor book an event for a customer this method is used
        //the class's available space is decremented
        //and then an email is sent to the customer about the booking 
        public bool TutorAddCustomerBooking(string customerId, int classId)
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
                if (bookingCheck == null && classInDb.AvailableSpace > 0)
                {
                    classInDb.AvailableSpace--;
                    Booking booking = new Booking
                    {
                        DateTime = DateTime.Now,
                        CustomerId = customer.Id,
                        ClassId = classId,
                        PaidOrNot = false
                    };
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    Helper.Utility.SendBookingMail(customer, classInDb, "Your recent booking!");
                    return true;
                }
            }
            return false;
        }

        //get list of all the bookings for a logged in user
        //ensure to get only those bookings which is not in the past 
        public List<Booking> UserBookings()
        {
            DateTime currentDate = DateTime.Now.Date;
            var loggedInUser = Helper.Utility.GetLoggedInUserId();
            var customer = _context.Customers.Where(c => c.UserId == loggedInUser).SingleOrDefault();
            return _context.Bookings
                           .Where(b => b.CustomerId == customer.Id && b.Class.StartDate >= currentDate)
                           .Include(c => c.Class)
                           .Include(c => c.Class.ClassType)
                           .Include(r => r.Class.Room)
                           .Include(b => b.Class.Room.Branch)
                           .Include(t => t.Class.Tutor)
                           .Include(b => b.Customer)
                           .ToList();
        }
    }
}
