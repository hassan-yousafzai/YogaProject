using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the ClassType model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// </summary>
    public class ClassType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Difficulty Level")]
        [RegularExpression(@"^[a-zA-Z\s]{1,40}$",
         ErrorMessage = "Class Type Name is not valid.")]
        public string ClassTypeName { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)",
            ErrorMessage = "Invalid Price.")]
        [Range(10, 1000)]
        public double Price { get; set; }
    }
}