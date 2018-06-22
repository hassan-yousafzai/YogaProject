using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the roles interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface IRolesRepository
    {
        List<IdentityRole> GetRoles();
        List<Tutor> GetUsers();
        List<string> GetExistingRoleNames();
        void CreateRole(FormCollection collection);
        bool DeleteRole(string roleName);
        IdentityRole EditRole(string roleName);
        bool EditRole(IdentityRole role);
        ApplicationUser GetSelectedUser(string userName);
        UserManager<ApplicationUser> UserManger();
    }

    /// <summary>
    /// The roles Repository class that implements the roles interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class RolesRepository : IRolesRepository
    {
        private ApplicationDbContext _context;

        public RolesRepository()
        {
            _context = new ApplicationDbContext();
        }

        //get list of identity roles
        public List<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }

        //get list of tutors
        public List<Tutor> GetUsers()
        {
            return _context.Tutors.ToList();
        }

        //get list of roles
        private List<IdentityRole> GetRolesList()
        {
            return _context.Roles.ToList();
        }

        //a method that gets only the role name in a list
        public List<string> GetExistingRoleNames()
        {
            List<string> roleNameList = new List<string>();
            foreach (var item in GetRolesList())
            {
                roleNameList.Add(item.Name);
            }
            return roleNameList;
        }

        //create a role
        public void CreateRole(FormCollection collection)
        {

            _context.Roles.Add(new IdentityRole()
            {
                Name = collection["CreateRoleName"]
            });

            _context.SaveChanges();
        }

        //delete a role, only if it has no users
        public bool DeleteRole(string roleName)
        {
            var status = false;

            var thisRole = _context
                           .Roles
                           .Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();

            if (!(thisRole.Users.Count > 0))
            {
                _context.Roles.Remove(thisRole);
                _context.SaveChanges();
                status = true;
            }
            return status;
        }

        //edit a role, only if it has no users
        public IdentityRole EditRole(string roleName)
        {
            var thisRole = _context
                           .Roles
                           .Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();
            return thisRole;
        }

        //edit role
        public bool EditRole(IdentityRole role)
        {
            _context.Entry(role).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return false;
        }

        //get selected user by username(email)
        public ApplicationUser GetSelectedUser(string userName)
        {
            return _context.Users
                           .Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();
        }

        //a metod that allows direct access to the usermanager
        public UserManager<ApplicationUser> UserManger()
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            return userManager;
        }
    }
}
