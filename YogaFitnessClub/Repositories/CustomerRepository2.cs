using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using YogaFitnessClub.Models;

namespace YogaFitnessClub.Repositories
{
    public interface ICustomerRepository2
    {
        Customer GetCustomer(string id);
        //Customer EditCustomer(string id);
        void SaveCustomer(Customer customer);

    }

    public class CustomerRepository2 : ICustomerRepository2
    {
        private HttpClient _client;

        public CustomerRepository2()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:50909") };
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public Customer GetCustomer(string id)
        {
            
            var res = _client.GetAsync($"api/Customers/{id}").Result;

            if (res.IsSuccessStatusCode)
            {
                var EmpResponse = res.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<Customer>(EmpResponse);
            }

            return null;

        }

        //public Customer EditCustomer(string id)
        //{
        //    var customerInDb = _context.Customers.SingleOrDefault(c => c.UserId == id);

        //    if (customerInDb == null)
        //        throw new Exception("Invalid Customer");

        //    return customerInDb;
        //}

        public void SaveCustomer(Customer customer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50909");

            var myContent = JsonConvert.SerializeObject(customer);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            client.PostAsync("api/Customers", byteContent);
        }

    }

}
