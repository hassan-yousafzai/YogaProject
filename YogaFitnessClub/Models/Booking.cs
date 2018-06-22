using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YogaFitnessClub.Models
{
    /// <summary>
    /// This is the Booking model class. It has been used to create a database table in the database using EF (IdentityModels) 
    /// Data annotations have been used to restrict the properties values. 
    /// The NotMapped annotation is used to enfore that that property should not be added to the table of this model
    /// This model also has relationships to other models 
    /// </summary>
    public class Booking
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public bool PaidOrNot { get; set; }

        [NotMapped]
        public bool CanCancel { get; set; }
    }
}