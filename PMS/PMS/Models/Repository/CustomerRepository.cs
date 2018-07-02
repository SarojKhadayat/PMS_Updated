using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Models;
using PMS.Models.PMSModel;

namespace PMS.Models.Repository
{
    public class CustomerRepository
    {
        public IList<Customer> GetAllCustomers()
        {
            ProductManagementDbEntities _db=new ProductManagementDbEntities();
            return _db.Customers.Select(x => x).ToList();
        }
    }
}