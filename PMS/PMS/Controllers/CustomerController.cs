using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Models.Repository;
namespace PMS.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        [Authorize]
        public ActionResult Index()
        {
            CustomerRepository customerRepository=new CustomerRepository();
            var allCustomers= customerRepository.GetAllCustomers();
            return View(allCustomers);
        }
    }
}