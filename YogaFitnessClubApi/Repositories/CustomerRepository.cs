using System;
using System.Data.Entity.Migrations;
using System.Linq;
using YogaFitnessClub.Models;
using YogaFitnessClubApi.Models;

namespace YogaFitnessClubApi.Repositories
{

    public interface ICustomerRepository
    {
        Customer GetCustomer(string id);
        Customer EditCustomer(string id);
        void SaveCustomer(Customer customer);

    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository()
        {
            _context = new DatabaseContext();
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