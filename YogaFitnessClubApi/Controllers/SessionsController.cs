using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using YogaFitnessClub.Models;
using YogaFitnessClubApi.Models;

namespace YogaFitnessClubApi.Controllers
{
    public class SessionsController : ApiController
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        // GET: api/Sessions
        public IQueryable<Session> GetSessions()
        {
            return _db.Sessions;
        }

        // GET: api/Sessions/5
        [ResponseType(typeof(Session))]
        public async Task<IHttpActionResult> GetSession(int id)
        {
            Session session = await _db.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        // PUT: api/Sessions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSession(int id, Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != session.Id)
            {
                return BadRequest();
            }

            _db.Entry(session).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sessions
        [ResponseType(typeof(Session))]
        public async Task<IHttpActionResult> PostSession(Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = session.Id }, session);
        }

        // DELETE: api/Sessions/5
        [ResponseType(typeof(Session))]
        public async Task<IHttpActionResult> DeleteSession(int id)
        {
            Session session = await _db.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _db.Sessions.Remove(session);
            await _db.SaveChangesAsync();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(int id)
        {
            return _db.Sessions.Count(e => e.Id == id) > 0;
        }
    }
}