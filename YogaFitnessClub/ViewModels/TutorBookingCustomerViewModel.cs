using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    /// <summary>
    /// This is the TutorBookingCustomerViewModel. It has been used to get all the required data for the a view 
    /// </summary>
    public class TutorBookingCustomerViewModel
    {
        public IEnumerable<ClassSessionViewModel> ClassSessions { get; set; }
        public Customer Customer { get; set; }
    }
}