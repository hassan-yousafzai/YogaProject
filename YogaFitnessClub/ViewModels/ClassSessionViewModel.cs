using System;
using System.Collections.Generic;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.ViewModels
{
    /// <summary>
    /// This is the ClassSessionViewModel. It has been used to get all the required data for the a view 
    /// </summary>
    public class ClassSessionViewModel
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }

        public int TutorId { get; set; }
        public Tutor Tutor { get; set; }

        public int ClassTypeId { get; set; }
        public ClassType ClassType { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public List<ClassSkill> ClassSkills { get; set; }

        public int SpaceLeft { get; set; }

    }
}