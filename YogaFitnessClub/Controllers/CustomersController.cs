using System.Web.Mvc;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace YogaFitnessClub.Controllers
{
    /// <summary>
    /// This is a customer controller that handles everything about customers e.g all the CRUD operations 
    /// This whole controller is only restricted to customer and some action methods are allowed to Tutors
    /// This controller utilises the CustomerRepository to complete all its tasks
    /// </summary>
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController()
        {
            _customerRepository = new CustomerRepository();
        }

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        //returns the ReadOnlyCustomerForm view with the customer details in it 
        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            var customer = _customerRepository.GetCustomer();
            return View("ReadOnlyCustomerForm", customer);
        }

        //returns the CustomerForm view with the customer details in it 
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerForm()
        {
            var customer = _customerRepository.GetCustomer();
            return View(customer);
        }

        //saves or updates a customer 
        [Authorize(Roles = "Customer")]
        public ActionResult SaveCustomer(Customer customer)
        {
            _customerRepository.SaveCustomer(customer);
            return RedirectToAction("Index", "Customers");
        }

        //gets all the customers in json format 
        //an ajax request is sent here and list of all customers is sent back
        //this is used for tutor booking an event for customers 
        [HttpPost]
        [Authorize(Roles = "Customer, Tutor")]
        public JsonResult GetAllCustomers()
        {
            var customers = _customerRepository.GetAllCustomers();
            return new JsonResult { Data = customers, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}