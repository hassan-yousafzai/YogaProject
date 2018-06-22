using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Class model. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations have been used to restrict the properties values. 
    /// The NotMapped annotation is used to enfore that that property should not be added to the table of this model
    /// This model also has relationships to other models 
    /// </summary>
    public class Class
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ThemeColour { get; set; }

        [Required]
        public string UserId { get; set; }     

        [Required]
        public int TutorId { get; set; }    
        public Tutor Tutor { get; set; }

        [Required]
        public int ClassTypeId { get; set; }
        public ClassType ClassType { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        public int AvailableSpace { get; set; }

        [NotMapped]
        public string SelectedSkills { get; set; }

        [NotMapped]
        public int Repeat { get; set; }

        [NotMapped]
        public string RecurSelectedOption { get; set; }
    }
}

