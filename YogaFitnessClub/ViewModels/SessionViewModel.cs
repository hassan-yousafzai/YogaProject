using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    public class SessionViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
    }
}