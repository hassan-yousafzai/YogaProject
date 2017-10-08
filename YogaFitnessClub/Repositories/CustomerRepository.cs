using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http.ModelBinding;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(string id);
        Customer EditCustomer(string id);
        void SaveCustomer(Customer customer);

    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository()
        {
            _context = new ApplicationDbContext();
        }

        public Customer GetCustomer(string id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == id);

            return customerInDb;
        }

        public Customer EditCustomer(string id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == id);

            if (customerInDb == null)
                throw new Exception("Invalid Customer");

            return customerInDb;
        }

        public void SaveCustomer(Customer customer)
        {  
            _context.Customers.AddOrUpdate(customer);
            _context.SaveChanges();
        } 

    }
     
}
