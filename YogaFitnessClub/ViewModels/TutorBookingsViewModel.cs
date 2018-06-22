using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    /// <summary>
    /// This is the TutorBookingsViewModel. It has been used to get all the required data for the a view 
    /// </summary>
    public class TutorBookingsViewModel
    {
        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int NumberOfBookings { get; set; }

    }
}