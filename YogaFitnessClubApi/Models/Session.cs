using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        [Display(Name = "Instructor Name")]
        public string InstructorName { get; set; }

        public Activity Activity { get; set; }

        [Display(Name = "Activity")]
        [Required]
        public int ActivityId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int CustomerId { get; set; }
        

    }
}