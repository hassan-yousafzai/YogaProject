using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Skill model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// </summary>
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Skill Name")]
        [RegularExpression(@"^[a-zA-Z\s]{1,100}$",
         ErrorMessage = "Skill name is not valid.")]
        public string SkillName { get; set; }

    }
}