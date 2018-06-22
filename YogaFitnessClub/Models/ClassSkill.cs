using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the ClassSkill model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations have been used to restrict the properties values
    /// This model also has relationships to other models 
    /// </summary>
    public class ClassSkill
    {
        public int Id { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}