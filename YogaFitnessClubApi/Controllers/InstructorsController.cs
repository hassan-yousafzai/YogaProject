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
    public class InstructorsController : ApiController
    {
        private readonly DatabaseContext _db = new DatabaseContext();

        // GET: api/Instructors
        public IQueryable<Instructor> GetInstructors()
        {
            return _db.Instructors;
        }

        // GET: api/Instructors/5
        [ResponseType(typeof(Instructor))]
        public async Task<IHttpActionResult> GetInstructor(int id)
        {
            Instructor instructor = await _db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return Ok(instructor);
        }

        // PUT: api/Instructors/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInstructor(int id, Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != instructor.Id)
            {
                return BadRequest();
            }

            _db.Entry(instructor).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructorExists(id))
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

        // POST: api/Instructors
        [ResponseType(typeof(Instructor))]
        public async Task<IHttpActionResult> PostInstructor(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Instructors.Add(instructor);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = instructor.Id }, instructor);
        }

        // DELETE: api/Instructors/5
        [ResponseType(typeof(Instructor))]
        public async Task<IHttpActionResult> DeleteInstructor(int id)
        {
            Instructor instructor = await _db.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            _db.Instructors.Remove(instructor);
            await _db.SaveChangesAsync();

            return Ok(instructor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InstructorExists(int id)
        {
            return _db.Instructors.Count(e => e.Id == id) > 0;
        }
    }
}