using System.Text.RegularExpressions;
using System.Web.Mvc;
using YogaFitnessClub.Helper;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a skill controller that handles everything about skills e.g all the CRUD operations 
    /// This whole controller is only restricted to admin 
    /// This controller utilises the SkillRepository to complete all its tasks
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class SkillsController : Controller
    {
        private readonly ISkillRepository _skillRepository;

        public SkillsController()
        {
            _skillRepository = new SkillRepository();
        }

        //sends list of skills in json formate to an ajax request that hits this method
        public JsonResult FetchSkills()
        {
            var data = _skillRepository.GetSkills();
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //shows the main view of skills with a list of skills sent to it
        public ActionResult Index()
        {
            var skills = _skillRepository.GetSkills();
            return View(skills);
        }

        //skills form
        public ActionResult SkillForm()
        {
            return View();
        }

        //saves or updates a skills
        //all validaiton has been considered
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSkill(Skill skill)
        {
            skill.SkillName = Regex.Replace(skill.SkillName, @"\s+", " ");//remove multiple whitespaces 
            //skill checking for only logged in users 
            if (skill.Id == 0)
            {
                var checkSkill = CheckIfSkillExist(skill);

                if (checkSkill != null)
                {
                    ViewData["Message"] = "This skill already exist.";
                    return View("SkillForm");
                }
                else
                {
                    _skillRepository.AddSkill(skill);
                    return RedirectToAction("Index", "Skills");
                }
            }
            else
            {
                var skillInDb = _skillRepository.GetSkill(skill.Id);
                if (skillInDb.SkillName == skill.SkillName)
                {
                    ViewData["Message"] = "No fields were updated.";
                    return View("SkillForm");
                }
                else
                {
                    _skillRepository.UpdateSkill(skill);
                    return RedirectToAction("Index", "Skills");
                }
            }
        }

        //check if skills exists amd return a skill or else a null will be returned
        public Skill CheckIfSkillExist(Skill skill)
        {
            skill.SkillName = Utility.ConvertToTitleCase(skill.SkillName);
            return _skillRepository.GetSkillByName(skill.SkillName);
        }

        //shows the skills form to edit a skill
        public ActionResult EditSkill(int id)
        {
            var skillInDb = _skillRepository.GetSkill(id);
            if (skillInDb != null)
                return View("SkillForm", skillInDb);
            else
                return RedirectToAction("Index", "SKills");
        }

        //deletes a skill but validation has been put in place
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var status = _skillRepository.DeleteSkill(id);
            if (status == true)
                return RedirectToAction("Index", "Skills");
            else
            {
                ViewData["Message"] = "This skill cannot be deleted as it is being used.";
                var listOfSkills = _skillRepository.GetSkills();
                return View("Index", listOfSkills);
            }
        }
    }
}