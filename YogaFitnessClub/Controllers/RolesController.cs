using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a roles controller that handles everything about admin managing roles e.g all the CRUD operations 
    /// This whole controller is only restricted to admin
    /// This controller utilises the RolesRepository to complete all its tasks
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesController()
        {
            _rolesRepository = new RolesRepository();
        }

        //renderes the roles view and sends list of roles in a view bag
        public ActionResult Index()
        {
            ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
            return View();
        }

        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Roles/Create
        //Creates a new role and also checks if the role exist or not
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

        //deletes a role by its name
        //if the role is being used then it is not deleted
        public ActionResult Delete(string RoleName)
        {
            var check = _rolesRepository.DeleteRole(RoleName);
            if (check == true)
                return RedirectToAction("Index");
            else
            {
                ViewBag.RoleDeleteMessage = "This role cannot be deleted";
                ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
                return View("Index");
            }
        }

        //GET: /Roles/Edit/roleName
        //edits a role, if the role is being used then it cannot be edited
        public ActionResult Edit(string roleName)
        {
            var role = _rolesRepository.EditRole(roleName);
            if (role.Users.Count > 0)
            {
                ViewBag.RoleEditMessage = "This role cannot be edited.";
                ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
                return View("Index");
            }
            else
                return View(role);
        }
        
        //POST: /Roles/Edit/[id]
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

        //adds a role to a user
        //if the user has a role then it is not added 
        //validation has been put in place 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string AddUserNameToUser, string AddRoleNameToUser)
        {
            if (!string.IsNullOrEmpty(AddUserNameToUser) && !string.IsNullOrEmpty(AddRoleNameToUser))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(AddUserNameToUser);

                if (user == null)
                {
                    ViewBag.RoleAddToUserMessage = "Something went wrong!";
                    ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
                    return View("Index");
                }

                var checkUserExistingRoles = _rolesRepository.UserManger().GetRoles(user.Id);

                if (checkUserExistingRoles.Contains(AddRoleNameToUser) == true)
                {
                    ViewBag.RoleAddToUserMessage = "This role was already assigned to this user!";
                }
                else if (user.Roles.Count > 0)
                    ViewBag.RoleAddToUserMessage = "A user can only have one role!";
                else
                {
                    _rolesRepository.UserManger().AddToRole(user.Id, AddRoleNameToUser);
                    ViewBag.RoleAddToUserMessage = "Role added to the user successfully!";
                }
            }
            else
            {
                ViewBag.RoleAddToUserMessage = "Role was not added to the user!";
            }

            ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
            return View("Index");
        }

        //shows all the roles for a user 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string userEmail)
        {
            if (!string.IsNullOrEmpty(userEmail))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(userEmail);
                if (user != null)
                {
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
                    ViewBag.GetRolesForAUserMessage = "Something went wrong!";
            }
            else
            {
                ViewBag.Message = "You must select a user to retrieve his roles!";
            }

            ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
            return View("Index");
        }

        //delete a role from a user 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserToRemoveRoleFrom, string TheRoleToRemove)
        {
            if (!string.IsNullOrEmpty(UserToRemoveRoleFrom) && !string.IsNullOrEmpty(TheRoleToRemove))
            {
                ApplicationUser user = _rolesRepository.GetSelectedUser(UserToRemoveRoleFrom);

                if (user == null)
                {
                    ViewBag.RemoveRoleFromUserMessage = "Something went wrong!";
                    ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
                    return View("Index");
                }

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

            ViewBag.Roles = _rolesRepository.GetExistingRoleNames();
            return View("Index");
        }

        //an ajax request is sent here to get all users 
        //sent from roles view
        [HttpPost]
        public JsonResult GetUsersEmail()
        {
            var users = _rolesRepository.GetUsers();
            return new JsonResult { Data = users, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //an ajax request is sent here to get all the roles 
        [HttpGet]
        public JsonResult GetRoles()
        {
            var roles = _rolesRepository.GetRoles();
            return new JsonResult { Data = roles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}