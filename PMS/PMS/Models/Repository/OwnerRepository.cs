using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Models.PMSModel;

namespace PMS.Models.Repository
{
    public class OwnerRepository
    {
        private ProductManagementDbEntities _db=new ProductManagementDbEntities();
        public List<Owner> GetAllOwners()
        {
            return _db.Owners.Select(x => x).ToList();
        }

        public bool AddOwner(Owner owner)
        {
           _db.Owners.Add(owner);
            int id = _db.SaveChanges();
            return id > 0;
        }
    }
}