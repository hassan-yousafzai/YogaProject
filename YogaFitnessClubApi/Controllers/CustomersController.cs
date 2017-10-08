using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using YogaFitnessClub.Models;
using YogaFitnessClubApi.Models;
using YogaFitnessClubApi.Repositories;

namespace YogaFitnessClubApi.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController()
        {
            _customerRepository = new CustomerRepository();
        }

        // GET: api/Customers/5
        public IHttpActionResult GetCustomer(string id)
        {
            var customer = _customerRepository.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _customerRepository.SaveCustomer(customer);

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }
        
    }
}