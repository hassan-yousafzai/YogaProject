using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YogaFitnessClub.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserId { get; set; }

    }
}