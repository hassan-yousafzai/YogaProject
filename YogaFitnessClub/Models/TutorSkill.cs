using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the TutorSkill model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// This model also has relationships to other models 
    /// </summary>
    public class TutorSkill
    {
        public int Id { get; set; }

        [Required]
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        [Required]
        public int TutorId { get; set; }
        public Tutor Tutor { get; set; }
    }
}