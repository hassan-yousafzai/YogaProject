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
    public class ActivitiesController : ApiController
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        // GET: api/Activities
        public IQueryable<Activity> GetActivities()
        {
            return _db.Activities;
        }

        // GET: api/Activities/5
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> GetActivity(int id)
        {
            Activity activity = await _db.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        // PUT: api/Activities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutActivity(int id, Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != activity.Id)
            {
                return BadRequest();
            }

            _db.Entry(activity).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
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

        // POST: api/Activities
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> PostActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Activities.Add(activity);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = activity.Id }, activity);
        }

        // DELETE: api/Activities/5
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> DeleteActivity(int id)
        {
            Activity activity = await _db.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _db.Activities.Remove(activity);
            await _db.SaveChangesAsync();

            return Ok(activity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ActivityExists(int id)
        {
            return _db.Activities.Count(e => e.Id == id) > 0;
        }
    }
}