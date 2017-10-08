using System.Web.Mvc;
using YogaFitnessClub.Models;
using Microsoft.AspNet.Identity;
using YogaFitnessClub.Repositories;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace YogaFitnessClub.Controllers
{
    [Authorize(Roles ="User")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository2 _customerRepository;

        public CustomersController()
        {
            _customerRepository = new CustomerRepository2();
        }

        public CustomersController(ICustomerRepository2 customerRepository)
        {
            _customerRepository = customerRepository ;
        }

        public ActionResult Index()
        {
            var customer = _customerRepository.GetCustomer(GetLoggedInUserId());
            
            if (customer == null)
                return View();

            return View(customer);
        }

        public ActionResult Details()
        { 
            var customer = _customerRepository.GetCustomer(GetLoggedInUserId());

            if (customer == null)
                return View();

            return View(customer);
        }

       //public ActionResult Edit()
       //{
       //     var customer = _customerRepository.EditCustomer(GetLoggedInUserId());

       //    return View("CustomerForm", customer);
       //}

        public ActionResult SaveCustomer(Customer customer)
        {
            _customerRepository.SaveCustomer(customer);

            return RedirectToAction("Index", "Customers");
        }

        public string GetLoggedInUserId()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                                  .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            return user.Id;
        }
    }
}