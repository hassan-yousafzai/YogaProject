using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YogaFitnessClub.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime Birthdate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "National Insurance Number")]
        public string NiNumber { get; set; }

        //public string Phone { get; set; }

        [Required]
        public string Gender { get; set; }




    }
} 