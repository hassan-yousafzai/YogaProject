using System;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Customer model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]{1,255}$",
         ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]        
        public DateTime? Birthdate { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[0-9]{1,9}[a-zA-Z\-\.\,\s]{1,40}$",
            ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) ?[0-9][A-Za-z]{2})$",
         ErrorMessage = "Invalid UK Postcode.")]
        public string Postcode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}