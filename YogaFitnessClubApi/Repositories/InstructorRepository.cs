using System;
using System.Linq;
using YogaFitnessClub.Models;
using YogaFitnessClubApi.Models;

namespace YogaFitnessClubApi.Repositories
{
    public interface IInstructorRepository
    {
        Instructor GetInstructor(int id);
        Instructor EditInstructor(int id);
        void SaveInstructor(Instructor instructor);
    }

    public class InstructorRepository : IInstructorRepository
    {
        private readonly DatabaseContext _context;

        public InstructorRepository()
        {
            _context = new DatabaseContext();
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
    }
}