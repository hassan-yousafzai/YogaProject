using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Controllers
{
    [Authorize(Roles = "Tutor")]
    public class SessionSchedulerController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        // GET: SessionScheduler
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var currentLoggedInUser = GetLoggedInUserId();
                var events = GetAllEventsForLoggedInUser();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(SessionScheduler e)
        {
            var status = false;
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (e.Id > 0)
                {
                    var loggedInUserId = GetLoggedInUserId();
                    //Update the event
                    var v = _context.SessionScheduler.Where(a => a.Id == a.Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Title = e.Title;
                        v.StartDate = e.StartDate;
                        v.EndDate = e.EndDate;
                        v.Description = e.Description;
                        v.TutorName = e.TutorName;
                        v.ThemeColour = e.ThemeColour;
                    }
                }
                else
                {
                    e.UserId = GetLoggedInUserId();
                    _context.SessionScheduler.Add(e);
                }

                _context.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var v = dc.SessionScheduler.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    dc.SessionScheduler.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult Test()
        {
            return View();
        }

        public List<SessionScheduler> GetAllEventsForLoggedInUser()
        {
            var currentLoggedInUser = GetLoggedInUserId();
            var events = _context.SessionScheduler
                .Where(s => s.UserId == currentLoggedInUser)
                .ToList();
            return events;
        }

        public string GetLoggedInUserId()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                  .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return user.Id;
        }
    }
}



         