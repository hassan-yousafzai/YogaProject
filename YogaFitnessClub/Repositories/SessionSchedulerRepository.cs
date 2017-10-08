using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    public interface ISessionSchedulerRepository
    {
        JsonResult GetEvents();


    }

    public class SessionSchedulerRepository : ISessionSchedulerRepository

    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public JsonResult GetEvents()
        {
            throw new NotImplementedException();
        }
    }
}