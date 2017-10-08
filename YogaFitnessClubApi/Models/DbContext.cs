using System.Data.Entity;
using YogaFitnessClub.Models;

namespace YogaFitnessClubApi.Models
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Instructor> Instructors { get; set; }

        public DatabaseContext()
            : base("DefaultConnection")
        {
        }

        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }
    }
}