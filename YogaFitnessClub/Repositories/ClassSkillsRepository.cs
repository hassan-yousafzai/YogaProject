using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the classSkills interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    interface IClassSkillsRepository
    {
        List<ClassSkill> GetClassSelectedSkills(int classId);
    }

    /// <summary>
    /// The class Repository class that implements the class interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class ClassSkillsRepository : IClassSkillsRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassSkillsRepository()
        {
            _context = new ApplicationDbContext();
        }

        //get list of classSkills from the classSkills table
        public List<ClassSkill> GetClassSelectedSkills(int classId)
        {
            var classSkills = _context.ClassSkills
                                      .Where(c => c.ClassId == classId)
                                      .Include(c => c.Class)
                                      .Include(c => c.Skill)
                                      .ToList();
            return classSkills;
        }
    }
}