using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    public class SessionFormViewModel
    {
        public Customer Customer { get; set; }
        public Session Sessions { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
    }
}