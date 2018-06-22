using System.Text.RegularExpressions;
using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This controller handles everything regarding branches such as all the CRUD operations
    /// All actions of this controller aer authorized, admins have access to all the functionality but tutor has to some
    /// This contoller uses the branchReposiotry to complete its tasks
    /// </summary>
    [Authorize]
    public class BranchesController : Controller
    {
        private readonly IBranchRepository _branchRepository; //ref to the branchRepository

        //constructor
        public BranchesController()
        {
            _branchRepository = new BranchRepository();
        }

        //the index action method which renders the Index page in the Views -> Branches folder
        //the view requires listOfBranches and they are sent to it.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var listOfBranches = _branchRepository.GetBranches();
            return View(listOfBranches);
        }

        //A method that returns all the branches and sends it to an ajax request that hits this methods
        //get requests are denied
        [HttpPost]
        [Authorize(Roles = "Tutor, Admin")]
        public JsonResult FetchBranches()
        {
            var branches = _branchRepository.GetBranches();
            return new JsonResult { Data = branches, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //the SaveBranch action method saves a branch or updates an existing one
        //all the validations are performed to ensure data is valid and not duplicate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveBranch(Branch branch)
        {
            branch.Address = Regex.Replace(branch.Address, @"\s+", " ");//remove multiple whitespaces 

            if (branch.Id == 0)
            {
                var checkBranch = CheckIfBranchExist(branch);

                if (checkBranch != null)
                {
                    ViewData["Message"] = "Branch already exist";
                    return View("BranchForm");
                }
                else
                {
                    _branchRepository.AddBranch(branch);
                    return RedirectToAction("Index", "Branches");
                }
            }
            else
            {
                _branchRepository.UpdateBranch(branch);
                return RedirectToAction("Index", "Branches");
            }
        }

        //the EditBranch action method gets a branch id as a parameter
        //it checks if the branch exist then one view is showed or else redirected to the index action method
        //the view requires listOfBranches and they are sent to it.
        [Authorize(Roles = "Admin")]
        public ActionResult EditBranch(int id)
        {
            var branchInDb = _branchRepository.GetBranchById(id);
            if (branchInDb != null)
                return View("BranchForm", branchInDb);
            else
                return RedirectToAction("Index", "Branches");
        }

        //shows the bracnh form
        [Authorize(Roles = "Admin")]
        public ActionResult BranchForm()
        {
            return View();
        }

        //deletes a branch by id 
        //all checks are performed and then the appropriate view is displayed
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var status = _branchRepository.DeleteBranch(id);
            if (status == true)
                return RedirectToAction("Index", "Branches");
            else
            {
                ViewData["Message"] = "This branch cannot be deleted as it has existing rooms!";
                var listOfBranches = _branchRepository.GetBranches();
                return View("Index", listOfBranches);
            }
        }

        //a method to check if a branch exist or not by using its address as it is unique
        public Branch CheckIfBranchExist(Branch branch)
        {
            return _branchRepository.GetBranchByAddress(branch.Address);
        }

    }
}