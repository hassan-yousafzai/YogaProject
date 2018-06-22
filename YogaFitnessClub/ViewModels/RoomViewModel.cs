using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    /// <summary>
    /// This is the RoomViewModel. It has been used to get all the required data for the a view 
    /// </summary>
    public class RoomViewModel
    {
        public IEnumerable<Branch> Branches { get; set; }
        public Room Room { get; set; }
        public bool NotEmptyModel { get; set; }
    }
}