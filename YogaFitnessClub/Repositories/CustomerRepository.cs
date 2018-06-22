using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    /// <summary>
    /// the Customer interface with some methods signatures
    /// any class that implements this interface will have to implement the methods inside the interface
    /// </summary>
    public interface ICustomerRepository
    {
        Customer GetCustomer();
        Customer GetCustomerByUserId(string UserId);
        void SaveCustomer(Customer customer);
        List<Customer> GetAllCustomers();
        List<Class> GetAllEvents();
        List<ClassSkill> GetSelectedSkills(int id);
    }

    /// <summary>
    /// The Customer Repository class that implements the Customer interface
    /// This repository has a direct reference to the ApplicaitonDbContext where
    /// all the database tables can be manipulated
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository()
        {
            _context = new ApplicationDbContext();
        }

        //get logged in customer 
        public Customer GetCustomer()
        {
            var loggedInUserId = Helper.Utility.GetLoggedInUserId();
            var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == loggedInUserId);
            return customerInDb;
        }

        //Save or update a customer 
        public void SaveCustomer(Customer customer)
        {
            if (customer.Id > 0)
            {
                var cust = _context.Customers.Where(c => c.Id == customer.Id).SingleOrDefault();
                if (cust != null)
                {
                    cust.Name = customer.Name;
                    cust.Address = customer.Address;
                    cust.Postcode = customer.Postcode;
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }
        }

        //get all events (classes and sessions) from todays date
        //from the classes table 
        public List<Class> GetAllEvents()
        {
            DateTime currentDate = DateTime.Now.Date;

            return _context.Classes
                .Where(c => c.StartDate > currentDate)
                .Include(c => c.ClassType)
                .Include(r => r.Room)
                .Include(b => b.Room.Branch)
                .Include(t => t.Tutor)
                .OrderBy(o => o.StartDate)
                .ToList();
        }

        //get list of selected skills by id
        public List<ClassSkill> GetSelectedSkills(int id)
        {
            var classInDb = _context.Classes.Where(c => c.Id == id).SingleOrDefault();

            if (classInDb != null)
            {
                return _context.ClassSkills
                .Where(c => c.ClassId == classInDb.Id)
                .Include(s => s.Skill)
               .ToList();
            }
            return null;
        }

        //get all customers from the customers table
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        //get customer by user id
        public Customer GetCustomerByUserId(string UserId)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == UserId);
            return customerInDb;
        }
    }

}
