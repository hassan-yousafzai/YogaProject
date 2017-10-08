using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Activity> Activity { get; set; }
        public IEnumerable<Session> Session { get; set; }
    }
}