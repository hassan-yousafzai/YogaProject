using System;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
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