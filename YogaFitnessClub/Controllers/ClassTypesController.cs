using System.Text.RegularExpressions;
using System.Web.Mvc;
using YogaFitnessClub.Helper;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a ClassTypes controller that handles everything about events classtypes e.g all the CRUD operations 
    /// This whole controller is only restricted and some action methods are allowed to Tutors/Admins
    /// This controller utilises the ClassSkillsRepository to complete all its tasks
    /// </summary>
    [Authorize]
    public class ClassTypesController : Controller
    {
        private readonly IClassTypeRepository _classTypeRepository;

        public ClassTypesController()
        {
            _classTypeRepository = new ClassTypeRepository();
        }

        //returns the Index view in the Views -> ClassTypes folder 
        //sends list of classtypes to the view
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var listOfClassTypes = _classTypeRepository.GetClassTypes();
            return View(listOfClassTypes);
        }

        //return the form for the clss types
        [Authorize(Roles = "Admin")]
        public ActionResult ClassTypeForm()
        {
            return View();
        }

        //saves a new classtype or updates an existing one
        //all validaiton has been performed before adding or updating a class type
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveClassType(ClassType classType)
        {
            classType.ClassTypeName = Regex.Replace(classType.ClassTypeName, @"\s+", " ");//remove multiple whitespaces 

            if (classType.Id == 0)
            {
                var checkClassType = CheckIfClassTypeExist(classType);

                if (checkClassType != null)
                {
                    ViewData["Message"] = "This difficulty already exist";
                    return View("ClassTypeForm");
                }
                else
                {
                    _classTypeRepository.AddClassType(classType);
                    return RedirectToAction("Index", "ClassTypes");
                }
            }
            else
            {
                var classTypeInDb = _classTypeRepository.GetClassType(classType.Id);
                if (classTypeInDb.ClassTypeName == classType.ClassTypeName
                    && classTypeInDb.Price == classType.Price)
                {
                    ViewData["Message"] = "You did not update any fields!";
                    return View("ClassTypeForm");
                }

                var classTypesInDb = _classTypeRepository.GetClassTypeByName(classType.ClassTypeName);
                if (classTypesInDb != null)
                {
                    ViewData["Message"] = "Cannot update as this difficulty level exist!";
                    return View("ClassTypeForm");
                }
                else
                {
                    _classTypeRepository.UpdateClassType(classType);
                    return RedirectToAction("Index", "ClassTypes");
                }
            }
        }

        //a method that checks if a classTyoe exist or not and returns the classType if yes else reutrns null
        public ClassType CheckIfClassTypeExist(ClassType classType)
        {
            classType.ClassTypeName = Utility.ConvertToTitleCase(classType.ClassTypeName);
            return _classTypeRepository.GetClassTypeByName(classType.ClassTypeName);
        }

        //an id of a class type is sent here to edit a classtype and this methods renders the form to edit 
        [Authorize(Roles = "Admin")]
        public ActionResult EditClassType(int id)
        {
            var classTypeInDb = _classTypeRepository.GetClassTypeById(id);
            if (classTypeInDb != null)
                return View("ClassTypeForm", classTypeInDb);
            else
                return RedirectToAction("Index", "ClassTypes");
        }

        //delete a classtype 
        //checks for validation
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var status = _classTypeRepository.DeleteClassType(id);
            if (status == true)
                return RedirectToAction("Index", "ClassTypes");
            else
            {
                ViewData["Message"] = "This difficulty cannot be deleted as it is being used!";
                var listOfClassTypes = _classTypeRepository.GetClassTypes();
                return View("Index", listOfClassTypes);
            }
        }

        //a method that returns all classtypes as in json format 
        //an ajax request is sent here to fetchClassTypes
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult FetchClassTypes()
        {
            var classTypes = _classTypeRepository.GetClassTypes();
            return new JsonResult { Data = classTypes, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        //returns a single classtype by its id]
        //sends it back as json 
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public JsonResult GetClassType(int id)
        {
            var classType = _classTypeRepository.GetClassType(id);
            return new JsonResult { Data = classType, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }
    }
}