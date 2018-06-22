using System;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Admin model class. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations and regular expressions have been used to restrict the properties values
    /// </summary>
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s]{1,255}$",
         ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

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
        [RegularExpression(@"^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-Ca-cEGHeghJ-PjpR-Tr-tW-Zw-z][A-Ca-cEGHeghJ-Nj-nPpR-Tr-tW-Zw-z])(?:\s*\d\s*){6}([A-Da-d]|\s)$",
            ErrorMessage = "Invalid National Insurance Number.\n Format: LL NN NN NN L")]
        public string NiN { get; set; }

        [Required]
        [RegularExpression(@"^(07[\d]{9}|7[\d]{9}|447[\d]{9}|00447[\d]{9})$",
            ErrorMessage = "Invalid UK mobile number. Try 447 or without 0")]
        public long MobileNumber { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}