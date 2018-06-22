using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Helper;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the class interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IClassRepository
    {
        List<Class> GetAllEventsForLoggedInUser();
        bool Save(Class model);
        bool CheckRoomAvailibility(Class model);
        bool Delete(int id);
    }

    /// <summary>
    /// The class Repository class that implements the class interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository()
        {
            _context = new ApplicationDbContext();
        }

        //gets all the events (classes and sessions) from the classes table
        //for logged in tutor, and ignores all events in the past 
        public List<Class> GetAllEventsForLoggedInUser()
        {
            DateTime currentDateTime = DateTime.Today;
            var currentLoggedInUser = Utility.GetLoggedInUserId();
            return _context.Classes
                           .Where(s => s.UserId == currentLoggedInUser
                                  && s.StartDate >= currentDateTime)
                           .ToList();
        }

        //save the class or session
        public bool Save(Class model)
        {
            var status = false;
            if (model.Id > 0)
            {
                //Update the event
                var classInDb = _context.Classes.Where(a => a.Id == model.Id).FirstOrDefault();
                if (classInDb != null)
                {
                    classInDb.Title = model.Title;
                    classInDb.RoomId = model.RoomId;
                    classInDb.ThemeColour = model.ThemeColour;
                    _context.SaveChanges();
                    RemoveAndReaddClassSkills(model);
                    status = true;
                }
            }
            else
            {
                //add all the required details to the model
                model.UserId = Utility.GetLoggedInUserId();
                var tutorId = _context.Tutors.Where(t => t.UserId == model.UserId).FirstOrDefault();
                model.TutorId = tutorId.Id;
                var room = _context.Rooms.Where(r => r.Id == model.RoomId).SingleOrDefault();//get room user selected and then configure avaliablespace
                model.AvailableSpace = room.Capacity;

                //No Repeat
                //add one event 
                if (Convert.ToInt32(Recur.None) == Convert.ToInt32(model.RecurSelectedOption))
                {
                    _context.Classes.Add(model);
                    _context.SaveChanges();
                    AddClassSkills(model);
                }

                //if the tutor selected recurring option 
                if (model.Repeat > 1)
                {
                    //Weekly - call the recurWeekly method
                    if (Convert.ToInt32(Recur.Weekly) == Convert.ToInt32(model.RecurSelectedOption))
                        RecurWeekly(model);

                    //Monthly - call the recurMonthly method
                    if (Convert.ToInt32(Recur.Monthly) == Convert.ToInt32(model.RecurSelectedOption))
                        RecurMonthly(model);

                    //Monthly by day- call the recurMonthlyByDay method
                    if (Convert.ToInt32(Recur.MonthlyByDay) == Convert.ToInt32(model.RecurSelectedOption))
                        RecurMonthlyByDay(model);

                }
                _context.SaveChanges(); //save all changes
                status = true;
            }
            return status;
        }

        //recur events weekly 
        private void RecurWeekly(Class model)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            _context.Classes.Add(model);
            _context.SaveChanges();
            AddClassSkills(model);

            for (var i = 0; i < model.Repeat; i++)
            {
                startDateTime = model.StartDate;
                endDateTime = model.EndDate;
                model.StartDate = startDateTime.AddDays(7);
                model.EndDate = endDateTime.AddDays(7);
                var checkIfClassExist = _context.Classes
                    .Where(c => c.StartDate == model.StartDate &&
                    c.EndDate == model.EndDate &&
                    c.UserId == model.UserId).FirstOrDefault();
                var roomCheck = CheckRecurRoomAvailibility(model);

                if (checkIfClassExist == null && roomCheck == false)
                {
                    _context.Entry(model).State = EntityState.Detached;
                    _context.Classes.Add(model);
                    _context.SaveChanges();
                    AddClassSkills(model);
                    _context.SaveChanges();
                }
            }
        }

        //recur monthly
        private void RecurMonthly(Class model)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            _context.Classes.Add(model);
            _context.SaveChanges();
            AddClassSkills(model);

            for (var i = 0; i < model.Repeat; i++)
            {
                startDateTime = model.StartDate;
                endDateTime = model.EndDate;
                model.StartDate = startDateTime.AddMonths(1);
                model.EndDate = endDateTime.AddMonths(1);
                var checkIfClassExist = _context.Classes
                    .Where(c => c.StartDate == model.StartDate &&
                    c.EndDate == model.EndDate &&
                    c.UserId == model.UserId).FirstOrDefault();

                var roomCheck = CheckRecurRoomAvailibility(model);

                if (checkIfClassExist == null && roomCheck == false)
                {
                    _context.Entry(model).State = EntityState.Detached;
                    _context.Classes.Add(model);
                    _context.SaveChanges();
                    AddClassSkills(model);
                }
            }
        }

        //recur monthly by day
        private void RecurMonthlyByDay(Class model)
        {
            DateTime startDateTime;
            DateTime endDateTime;
            _context.Classes.Add(model);
            _context.SaveChanges();
            AddClassSkills(model);
            for (var i = 0; i < model.Repeat; i++)
            {
                startDateTime = model.StartDate;
                endDateTime = model.EndDate;
                model.StartDate = GetNextWeekDayCycle(startDateTime);
                model.EndDate = GetNextWeekDayCycle(endDateTime);
                var checkIfClassExist = _context.Classes
                    .Where(c => c.StartDate == model.StartDate &&
                    c.EndDate == model.EndDate &&
                    c.UserId == model.UserId).FirstOrDefault();

                var roomCheck = CheckRecurRoomAvailibility(model);

                if (checkIfClassExist == null && roomCheck == false)
                {
                    _context.Entry(model).State = EntityState.Detached;
                    _context.Classes.Add(model);
                    _context.SaveChanges();
                    AddClassSkills(model);
                }
            }
        }

        //remove and readd classksills to the table
        private void RemoveAndReaddClassSkills(Class model)
        {
            //First remove the skilss for this class from ClassSkills table
            var classInDb = _context.Classes.Where(a => a.Id == model.Id).FirstOrDefault();
            if (classInDb != null)
            {
                var classSkills = _context.ClassSkills.Where(cs => cs.ClassId == classInDb.Id).ToList();

                foreach (var cs in classSkills)
                {
                    _context.ClassSkills.Remove(cs);
                }
                _context.SaveChanges();
            }

            //add the skills
            string[] skillsArray = model.SelectedSkills.Split(' ');
            foreach (var item in skillsArray)
            {
                if (item != "")
                {
                    var classSkill = new ClassSkill
                    {
                        ClassId = model.Id,
                        SkillId = Convert.ToInt32(item)
                    };
                    _context.ClassSkills.Add(classSkill);
                    _context.SaveChanges();
                }
            }
        }

        //add classSkills to the classksills table for a class
        private void AddClassSkills(Class model)
        {
            string[] skillsArray = model.SelectedSkills.Split(' ');
            foreach (var item in skillsArray)
            {
                if (item != "")
                {
                    var classSkill = new ClassSkill
                    {
                        ClassId = model.Id,
                        SkillId = Convert.ToInt32(item)
                    };
                    _context.ClassSkills.Add(classSkill);
                    _context.SaveChanges();
                }
            }
        }

        //get next week day cycle for monthly by day
        private static DateTime GetNextWeekDayCycle(DateTime selectedDateTime)
        {
            DateTime nextWeekDayCycle = selectedDateTime.AddDays(28);
            // the following evaluates to 1 for 1-7, 2 for 8-14, etc
            int n1 = (selectedDateTime.Day - 1) / 7 + 1;
            int n2 = (nextWeekDayCycle.Day - 1) / 7 + 1;
            if (n2 != n1)
            {
                nextWeekDayCycle = nextWeekDayCycle.AddDays(7);
            }
            return nextWeekDayCycle;
        }

        //check room availibility 
        public bool CheckRoomAvailibility(Class model)
        {
            var status = false;
            if (model.Id == 0)
            {
                var classInDb = _context.Classes
                                        .Where(c => c.StartDate == model.StartDate &&
                                               c.EndDate == model.EndDate &&
                                               c.RoomId == model.RoomId)
                                        .SingleOrDefault();
                //return true if room is booked
                if (classInDb != null)
                    status = true;
            }
            return status;
        }

        //check room availibility when recurring events
        private bool CheckRecurRoomAvailibility(Class model)
        {
            var status = false;
            var classInDb = _context.Classes
                                        .Where(c => c.StartDate == model.StartDate &&
                                               c.EndDate == model.EndDate &&
                                               c.RoomId == model.RoomId)
                                         .SingleOrDefault();

            //return true if room is booked
            if (classInDb != null)
                status = true;

            return status;
        }

        //delete class or session (done by the tutor)
        //and send cancellation email to the customers who booked the class
        public bool Delete(int id)
        {
            var status = false;
            var classInDb = _context.Classes.Where(a => a.Id == id).FirstOrDefault();
            if (classInDb != null)
            {
                var bookings = _context.Bookings.Where(c => c.ClassId == classInDb.Id).ToList();
                if (bookings.Count > 0)
                {
                    Customer customer;
                    Booking booking;
                    foreach (var b in bookings)
                    {
                        customer = _context.Customers.Where(c => c.Id == b.CustomerId).SingleOrDefault();
                        booking = _context.Bookings.Where(bb => bb.Id == b.Id).SingleOrDefault();
                        if (booking != null)
                        {
                            _context.Bookings.Remove(booking);
                            _context.SaveChanges();
                            Utility.SendCancellationMail(customer, "Your booked event has been cancelled by your tutor!");
                        }
                    }
                }

                _context.Classes.Remove(classInDb);
                _context.SaveChanges();
                status = true;
            }
            return status;
        }
    }//end 

    //enum for recur
    enum Recur
    {
        None = 1,
        Weekly = 2,
        Monthly = 3,
        MonthlyByDay = 4
    };

}