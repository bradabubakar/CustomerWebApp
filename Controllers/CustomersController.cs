using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CustomerMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerMVCProject.Controllers
{
    public class CustomersController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();

        Customer _oCustomer = new Customer();
        List<Customer> _oCustomers = new List<Customer>();

        public CustomersController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<List<Customer>> GetAllCustomers()
        {
            _oCustomers = new List<Customer>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:44348/api/v1/Customers"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oCustomers = JsonConvert.DeserializeObject<List<Customer>>(apiResponse);
                }
            }
             return _oCustomers;
        }


        [HttpGet]
        public async Task<Customer> GetById(int customerId)
        {
            _oCustomer = new Customer();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.GetAsync("https://localhost:44348/api/v1/Customers/" + customerId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oCustomer = JsonConvert.DeserializeObject<Customer>(apiResponse);
                }
            }
            return _oCustomer;
        }


        [HttpPost]
        public async Task<Customer> AddUpdateCustomer(Customer customer)
        {
            _oCustomer = new Customer();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44348/api/v1/Customers/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _oCustomer = JsonConvert.DeserializeObject<Customer>(apiResponse);
                }
            }
            return _oCustomer;
        }


        [HttpDelete]
        public async Task<string> Delete(int customerId)
        {
            string message = "";

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:44348/api/v1/Customers/" + customerId))
                {
                    message = await response.Content.ReadAsStringAsync();
              
                }
            }
            return message;
        }


    }
}