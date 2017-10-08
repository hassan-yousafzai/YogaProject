using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using YogaFitnessClub.Models;
using YogaFitnessClubApi.Models;

namespace YogaFitnessClubApi.Repositories
{
    public interface ISessionRepository
    {
        List<Session> GetSession(int id);
        List<Activity> NewSession();
        Session SaveSession(Session session);
        void DeleteSession(int id);
        Session EditSession(int id);
        List<Session> GetSessionsByInstructor();
    }

    public class SessionsRepository : ISessionRepository
    {
        private readonly DatabaseContext _context;

        public SessionsRepository()
        {
            _context = new DatabaseContext();
        }

        public List<Session> GetSession(int id)
        {
            var sessionInDb = _context.Sessions
                .Include(c => c.Customer)
                .Where(c => c.CustomerId == id).
                ToList();

            var activityInDb = _context.Activities.ToList();

            if (sessionInDb == null)
                throw new Exception("Not found");


            return sessionInDb;
        }

        public List<Activity> NewSession()
        {
            return _context.Activities.ToList();
        }

        public Session SaveSession(Session session)
        {

            session.CustomerId = 1;
            _context.Sessions.AddOrUpdate(session);
            _context.SaveChanges();

            return session;
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

            if (sessionInDb == null)
                throw new Exception("Session Not found");

            return sessionInDb;
        }

        public List<Session> GetSessionsByInstructor()
        {
            return 
                _context.Sessions
                    .Include(c => c.Customer)
                    .ToList()
                    .OrderBy(c => c.CustomerId)
                    .ThenBy(s => s.Date)
                    .ToList();
        
        }
    }
}