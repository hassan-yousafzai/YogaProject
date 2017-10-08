using System;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;
using YogaFitnessClub.ViewModels;

namespace YogaFitnessClub.Repositories
{
    public interface IInstructorRepository
    {
        Instructor GetInstructor(int id);
        Instructor EditInstructor(int id);
        void SaveInstructor(Instructor instructor);
        InstructorFormViewModel GetSessions();
    }

    public class InstructorRepository : IInstructorRepository
    {
        private readonly ApplicationDbContext _context;

        public InstructorRepository()
        {
            _context = new ApplicationDbContext();
        }

        public Instructor GetInstructor(int id)
        {
            var instructorInDb = _context.Instructors.SingleOrDefault(c => c.Id == id);

            if (instructorInDb == null)
                throw new Exception();

            return instructorInDb;
        }

        public Instructor EditInstructor(int id)
        {
            var instructorInDb = _context.Instructors.SingleOrDefault(s => s.Id == id);

            if (instructorInDb == null)
                throw new Exception("Session Not found");

            return instructorInDb;
        }

        public void SaveInstructor(Instructor instructor)
        {
            var instructorInDb = _context.Instructors.Single(c => c.Id == instructor.Id);

            instructorInDb.Name = instructor.Name;
            instructorInDb.Gender = instructor.Gender;
            instructorInDb.Birthdate = instructor.Birthdate;
            instructorInDb.NiNumber = instructor.NiNumber;
            instructorInDb.Address = instructor.Address;
            instructorInDb.Postcode = instructor.Postcode;
            instructorInDb.Email = instructor.Email;

            _context.SaveChanges();

        }

        public InstructorFormViewModel GetSessions()
        {
            var sessionsInDb =
                _context.Sessions
                    .Include(c => c.Customer)
                    .ToList()
                    .OrderBy(c => c.CustomerId)
                    .ThenBy(s => s.Date);

            var viewModel = new InstructorFormViewModel
            {
                Activities = _context.Activities.ToList(),
                Sessions = sessionsInDb,
            };


            if (sessionsInDb == null)
                throw new Exception();

            return viewModel;
        }
    }

}
