using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesController()
        {
            _rolesRepository = new RolesRepository();
        }


        public ActionResult Index()
        {
            ViewBag.Roles = _rolesRepository.GetRolesDropDownList();
            ViewBag.Users = _rolesRepository.GetUsersDropDownList();
            return View();
        }

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var requestedRoleName = collection["CreateRoleName"].ToString();

            if (!String.IsNullOrWhiteSpace(requestedRoleName))
            {

                var CheckIfRoleExists = _rolesRepository.GetExistingRoleNames().Contains(requestedRoleName);

                if (CheckIfRoleExists == false)
                {
                    _rolesRepository.CreateRole(collection);
                    ViewBag.Message = "Role created successfully!";
                }
                else
                {
                    ViewBag.Message = "Role already exists!";
                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult Delete(string RoleName)
        {
            _rolesRepository.DeleteRole(RoleName);
            return RedirectToAction("Index");
        }
       
        // GET: /Roles/Edit/roleName
        public ActionResult Edit(string roleName)
        {
            var role = _rolesRepository.EditRole(roleName);
            return View(role);
        }

        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                _rolesRepository.EditRole(role);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string AddUserNameToUser, string AddRoleNameToUser)
        {
            if (!string.IsNullOrEmpty(AddUserNameToUser) && !string.IsNullOrEmpty(AddRoleNameToUser))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(AddUserNameToUser);

                var checkUserExistingRoles = _rolesRepository.UserManger().GetRoles(user.Id);

                if (checkUserExistingRoles.Contains(AddRoleNameToUser) == true)
                {
                    ViewBag.RoleAddToUserMessage = "This role was already assigned to this user!";
                }
                else
                {
                    _rolesRepository.UserManger().AddToRole(user.Id, AddRoleNameToUser);
                    ViewBag.RoleAddToUserMessage = "Role added to the user successfully!";
                }
            }
            else
            {
                ViewBag.RoleAddToUserMessage = "Something went wrong! Role was not added to the user!";
            }

            // Repopulate Dropdown Lists
            ViewBag.Roles = _rolesRepository.GetRolesDropDownList();
            ViewBag.Users = _rolesRepository.GetUsersDropDownList();

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string ListOfUserNames)
        {
            if (!string.IsNullOrEmpty(ListOfUserNames))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(ListOfUserNames);

                var numberOfRoles = _rolesRepository.UserManger().GetRoles(user.Id);
                if (numberOfRoles.Count < 1)
                {
                    ViewBag.NumberOfRolesForThisUser = 0;
                }
                else
                {
                    ViewBag.RolesForThisUser = _rolesRepository.UserManger().GetRoles(user.Id);
                    ViewBag.GetRolesForAUserMessage = "Roles retrieved successfully!";
                }

            }
            else
            {
                ViewBag.Message = "You must select a user to retrieve his roles!";               
            }

            // Repopulate Dropdown Lists
            ViewBag.Roles = _rolesRepository.GetRolesDropDownList();
            ViewBag.Users = _rolesRepository.GetUsersDropDownList();

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserToRemoveRoleFrom, string TheRoleToRemove)
        {
            if (!string.IsNullOrEmpty(UserToRemoveRoleFrom) && !string.IsNullOrEmpty(TheRoleToRemove))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(UserToRemoveRoleFrom);

                if (_rolesRepository.UserManger().IsInRole(user.Id, TheRoleToRemove))
                {
                    _rolesRepository.UserManger().RemoveFromRole(user.Id, TheRoleToRemove);
                    ViewBag.RemoveRoleFromUserMessage = "The '" + TheRoleToRemove + "' Role has been removed from this user!";
                }
                else
                {
                    ViewBag.RemoveRoleFromUserMessage = "This user doesn't belong to '" + TheRoleToRemove + "' role.";
                }
            }
            else
            {
                ViewBag.RemoveRoleFromUserMessage = "You must select a user and a role!";
            }

            // Repopulate Dropdown Lists
            ViewBag.Roles = _rolesRepository.GetRolesDropDownList();
            ViewBag.Users = _rolesRepository.GetUsersDropDownList();

            return View("Index");
        }

    }
}