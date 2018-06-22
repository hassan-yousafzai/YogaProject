using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Room model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// This model also has relationships to other models 
    /// </summary>
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        [RegularExpression("^[a-zA-Z]{1,4}[-]?[0-9]{2,5}$",
            ErrorMessage = "Invalid room number.")]
        public string RoomNumber { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)",
            ErrorMessage = "Invalid capacity.")]
        [Range(1, 50)]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

    }
}