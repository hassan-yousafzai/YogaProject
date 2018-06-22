using System;
using System.Collections.Generic;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the skill interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface ISkillRepository
    {
        List<Skill> GetSkills();
        Skill GetSkill(int id);
        void AddSkill(Skill skill);
        void UpdateSkill(Skill skill);
        Skill GetSkillByName(string name);
        Boolean DeleteSkill(int id);
    }

    /// <summary>
    /// The skill Repository class that implements the skill interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository()
        {
            _context = new ApplicationDbContext();
        }

        //add a skill
        //convert the skill name to title case
        public void AddSkill(Skill skill)
        {
            skill.SkillName = Helper.Utility.ConvertToTitleCase(skill.SkillName);
            _context.Skills.Add(skill);
            _context.SaveChanges();
        }

        //delete a skill, only if it has not been used in any classes or sessions 
        public bool DeleteSkill(int id)
        {
            var status = false;
            var skillInDb = _context.Skills.Where(s => s.Id == id).SingleOrDefault();

            if (skillInDb != null)
            {
                var isUsedInClassSkill = _context.ClassSkills.Where(s => s.SkillId == skillInDb.Id).ToList();
                var isUsedInTutorsSkill = _context.TutorSkills.Where(s => s.SkillId == skillInDb.Id).ToList();

                if (isUsedInClassSkill.Count == 0 && isUsedInTutorsSkill.Count == 0)
                {
                    _context.Skills.Remove(skillInDb);
                    _context.SaveChanges();
                    return status = true;
                }
            }
            return status;
        }

        //get a skill by id
        public Skill GetSkill(int id)
        {
            return _context.Skills.SingleOrDefault(s => s.Id == id);
        }

        //get a skill by name 
        //the string sent here is converted to title case
        public Skill GetSkillByName(string name)
        {
            name = Helper.Utility.ConvertToTitleCase(name);
            return _context.Skills.Where(s => s.SkillName == name).FirstOrDefault();
        }

        //get list of skills
        public List<Skill> GetSkills()
        {
            return _context.Skills.ToList();
        }

        //update a skill
        //skill name is converted to title case
        public void UpdateSkill(Skill skill)
        {
            skill.SkillName = Helper.Utility.ConvertToTitleCase(skill.SkillName);

            var skillInDb = _context.Skills.Where(s => s.Id == skill.Id).SingleOrDefault();
            if (skillInDb != null)
            {
                skillInDb.SkillName = skill.SkillName;

                _context.SaveChanges();
            }
        }
    }
}

