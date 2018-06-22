using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace YogaFitnessClub.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //This the the application db context that has been used to add all the below model to the database as tables
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<TutorSkill> TutorSkills { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<ClassType> ClassTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ClassSkill> ClassSkills { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           //ensure that the tables are named the following 
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Branch>().ToTable("Branches");
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Tutor>().ToTable("Tutors");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<TutorSkill>().ToTable("TutorSkills");
            modelBuilder.Entity<Skill>().ToTable("Skills");
            modelBuilder.Entity<ClassType>().ToTable("ClassType");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<ClassSkill>().ToTable("ClassSkills");

        }
    }
}