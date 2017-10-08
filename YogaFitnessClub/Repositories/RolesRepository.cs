using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    public interface IRolesRepository
    {
        List<SelectListItem> GetRolesDropDownList();
        List<SelectListItem> GetUsersDropDownList(); //may change to only tutors
        List<string> GetExistingRoleNames();
        void CreateRole(FormCollection collection);
        void DeleteRole(string roleName);
        IdentityRole EditRole(string roleName);
        void EditRole(IdentityRole role);
        ApplicationUser GetSelectedUser(string userName);
        UserManager<ApplicationUser> UserManger();
    }


    public class RolesRepository : IRolesRepository
    {
        private  ApplicationDbContext _context;

        public RolesRepository()
        {
            _context = new ApplicationDbContext();
        }
        public List<SelectListItem> GetRolesDropDownList()
        {
            return _context.Roles
                           .OrderBy(r => r.Name)
                           .ToList().Select(r => new SelectListItem { Value = r.Name.ToString(), Text = r.Name })
                           .ToList();
        }

        public List<SelectListItem> GetUsersDropDownList()
        {
            return _context.Users
                           .OrderBy(u => u.UserName)
                           .ToList().Select(u => new SelectListItem { Value = u.UserName.ToString(), Text = u.UserName })
                           .ToList();
        }

        private List<IdentityRole> GetRolesList()
        {
            return _context.Roles.ToList();
        }

        public List<string> GetExistingRoleNames()
        {
            List<string> roleNameList = new List<string>();
            foreach (var item in GetRolesList())
            {
                roleNameList.Add(item.Name);
            }
            return roleNameList;
        }

        public void CreateRole(FormCollection collection)
        {

            _context.Roles.Add(new IdentityRole()
            {
                Name = collection["CreateRoleName"]
            });

            _context.SaveChanges();
        }

        public void DeleteRole(string roleName)
        {
            var thisRole = _context
                           .Roles
                           .Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();

            _context.Roles.Remove(thisRole);
            _context.SaveChanges();
        }

        public IdentityRole EditRole(string roleName)
        {
            var thisRole = _context
                           .Roles
                           .Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();
            return thisRole;
        }

        public void EditRole(IdentityRole role)
        {
            _context.Entry(role).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public ApplicationUser GetSelectedUser(string userName)
        {
            return _context.Users
                           .Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
                           .FirstOrDefault();
        }

        public UserManager<ApplicationUser> UserManger()
        {
            var userStore = new UserStore<ApplicationUser>(_context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            return userManager;
        }
    }  
}
