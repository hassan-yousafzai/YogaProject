using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    /// <summary>
    /// This is the PaymentLogViewModel. It has been used to get all the required data for the a view 
    /// </summary>
    public class PaymentLogViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; }
        public Customer Customer { get; set; }
    }
}