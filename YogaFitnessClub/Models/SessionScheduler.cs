using System;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    public class SessionScheduler
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Tutor Name")]
        public string TutorName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string UserId { get; set; }

        [StringLength(255)]
        public string ThemeColour { get; set; }
    }
}

 