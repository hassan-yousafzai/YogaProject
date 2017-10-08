using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    public class InstructorFormViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
    }
}