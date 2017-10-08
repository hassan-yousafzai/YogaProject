using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YogaFitnessClub.Models;
using YogaFitnessClub.Repositories;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var customerRepo = new CustomerRepository2();

            var result = customerRepo.GetCustomer("e3911f67-0fd5-4872-b829-024710d013d0");

        }

        [TestMethod]
        public void TestMethod2()
        {
            var customerRepo = new CustomerRepository2();

            var t = new Customer()
            {
                Address = "BPP",
                Birthdate = DateTime.Now,
                Email = "boo@gmail.com",
                Name = "boo",
                Postcode = "BB4 5LR"
            };

            customerRepo.SaveCustomer(t);



        }
    }
}
