using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Models.PMSModel;
using PMS.Models.Repository;

namespace PMS.Controllers
{
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(string name, string address, string number, bool isActive=false)
        {
            Owner owner = new Owner()
            {
                OwnerName = name,
                OwnerAddress = address,
                PhoneNumber = number,
                IsActive = isActive
            };
            OwnerRepository repository=new OwnerRepository();
            bool status = repository.AddOwner(owner);
           return Json(status);
        }

        public ActionResult _Index()
        {
            OwnerRepository repository = new OwnerRepository();
            var allOwners = repository.GetAllOwners();
            return  PartialView("_Index",allOwners);
        }
    }
}