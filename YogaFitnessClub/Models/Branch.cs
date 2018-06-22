using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Branch model class. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// </summary>
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Location Name")]
        [RegularExpression(@"^[a-zA-Z\s]{1,40}$",
         ErrorMessage = "Location name is not valid.")]
        public string LocationName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[0-9]{1,9}[a-zA-Z\-\.\,\s]{1,40}$",
            ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [StringLength(8)]
        [Required]
        [RegularExpression("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) ?[0-9][A-Za-z]{2})$",
         ErrorMessage = "Invalid UK Postcode.")]
        public string Postcode { get; set; }

        [Required]
        [RegularExpression(@"^(07[\d]{9}|7[\d]{9}|447[\d]{9}|00447[\d]{9})$",
            ErrorMessage = "Invalid UK mobile number. Try 447 or without 0")]
        public long MobileNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}