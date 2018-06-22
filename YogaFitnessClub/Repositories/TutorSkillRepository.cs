using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;


namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the TutorSkill interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface ITutorSkillRepository
    {
        List<TutorSkill> GetTutorSkills(string id);
        List<Skill> GetAllSkills();
        List<Tutor> GetAllTutors();
        Tutor GetTutor(int id);
        Skill GetSkill(int id);
        bool AddTutorSkill(TutorSkill model);
    }

    /// <summary>
    /// The TutorSkill Repository class that implements the TutorSkill interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class TutorSkillRepository : ITutorSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public TutorSkillRepository()
        {
            _context = new ApplicationDbContext();

        }

        //get a tutor by id
        public Tutor GetTutor(int id)
        {
            return _context.Tutors.Where(t => t.Id == id).SingleOrDefault();
        }

        //get a skill by id
        public Skill GetSkill(int id)
        {
            return _context.Skills.Where(s => s.Id == id).SingleOrDefault();
        }

        //get all the skills as a list
        public List<Skill> GetAllSkills()
        {
            return _context.Skills.ToList();
        }

        //get all tutors as a list
        public List<Tutor> GetAllTutors()
        {
            return _context.Tutors.ToList();
        }

        //get TutorSkill by uesr id
        //also load data from the tutor table and skill
        public List<TutorSkill> GetTutorSkills(string userId)
        {
            return _context.TutorSkills
                           .Include(i => i.Tutor)
                           .Include(i => i.Skill)
                           .Where(i => i.Tutor.UserId == userId).ToList();
        }

        //add tutorskill 
        public bool AddTutorSkill(TutorSkill model)
        {
            var status = false;
            var tutorSkill = _context.TutorSkills.Where(t => t.SkillId == model.SkillId && t.TutorId == model.TutorId).FirstOrDefault();
            if (tutorSkill == null)
            {
                _context.TutorSkills.Add(model);
                _context.SaveChanges();
                status = true;
            }
            return status;
        }
    }
}