using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the admin interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    interface IAdminRepository
    {
        void SaveAdmin(Admin admin);
    }

    /// <summary>
    /// The admin Repository class that implements the admin interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository()
        {
            _context = new ApplicationDbContext();
        }

        //save admin to the admins table
        public void SaveAdmin(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
    }
}