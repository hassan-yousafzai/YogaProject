using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using YogaFitnessClub.Models;
using YogaFitnessClub.ViewModels;
using System.Collections.Generic;

namespace YogaFitnessClub.Repositories
{
    public interface ISessionRepository
    {
        void SaveSession(Session session);
        void DeleteSession(int id);
        Session EditSession(int id);
        IEnumerable<Activity> GetAllActivities();
        Customer GetCustomer(string id);
        IEnumerable<Session> GetSessions(int id);
    }

    public class SessionsRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionsRepository()
        {
            _context = new ApplicationDbContext();
        }

        //public SessionViewModel GetSessions(string id)
        //{
        //    var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == id);

        //    var sessionInDb = _context.Sessions
        //                              .Include(c => c.Customer)
        //                              .Where(c => c.Id == customerInDb.Id)
        //                              .ToList();


        //    var viewModel = new SessionViewModel()
        //    {
        //        Activities = _context.Activities.ToList(),
        //        Sessions = sessionInDb,
        //        Customer = customerInDb
        //    };

        //    return viewModel;
        //}


        public IEnumerable<Activity> GetAllActivities()
        {
            return _context.Activities.ToList();
        }

        public Customer GetCustomer(string id)
        {
            return _context.Customers.SingleOrDefault(c => c.UserId == id);

        }

        public IEnumerable<Session> GetSessions(int id)
        {
            return _context.Sessions
                    .Where(c => c.Customer.Id == id)
                    .ToList();
        }

        public void SaveSession(Session session)
        {
            _context.Sessions.AddOrUpdate(session);
            _context.SaveChanges();
        }

        public void DeleteSession(int id)
        {
            var sessionInDb = _context.Sessions.FirstOrDefault(c => c.Id == id);

            if (sessionInDb != null)
                _context.Sessions.Remove(sessionInDb);

            _context.SaveChanges();
        }

        public Session EditSession(int id)
        {
            var sessionInDb = _context.Sessions.SingleOrDefault(s => s.Id == id);

            return sessionInDb;
        }


    }



}
 