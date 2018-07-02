using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Models.PMSModel;
using PMS.Models.Repository;
using Microsoft.Reporting.WebForms;
using PMS.Common;
using System.IO;
using PMS.Models;

namespace PMS.Controllers
{
    //SessionExpireFilter]
    
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            //if (Request.Cookies["userinfo"] != null)
            //{
            //    HttpCookie aCookie = Request.Cookies["userinfo"];
            //    var text = Server.HtmlEncode(aCookie.Value);
            //}
            //else
            //{
            //    return RedirectToAction("login", "Account");
            //}
            ProductRepository productRepository = new ProductRepository();
            var products = productRepository.GetAllProducts();
            //var products = productRepository.GetAllProductsAdo();           
            //TempData["products"] = products;
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagementDbEntities _db = new ProductManagementDbEntities();
            //ViewBag.message = "I am from viewbag";
            //ViewData["msg"] = "I am from  viewdata";
            //ViewData["AllProducts"] = TempData["products"];
            ViewBag.ownerList = (from owner in _db.Owners
                                 select new SelectListItem
                                 {
                                     Text = owner.OwnerName,
                                     Value = owner.Id.ToString()
                                 });
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                ProductRepository productRepository = new ProductRepository();
                //productRepository.AddProduct(product);  
                productRepository.InsertIntoProductUsingAdo(product);
                //productRepository.InsertProductUsingSp(product);
            }
            //_db.Entry(productToUpdate).CurrentValues.SetValues(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProductRepository productRepository = new ProductRepository();
            var product = productRepository.GetProductById(id);
            ProductManagementDbEntities _db = new ProductManagementDbEntities();
            ViewBag.ownerList = (from owner in _db.Owners
                                 select new SelectListItem
                                 {
                                     Text = owner.OwnerName,
                                     Value = owner.Id.ToString()
                                 });
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            ProductRepository productRepository = new ProductRepository();
            if (ModelState.IsValid)
            {
                productRepository.UpdateProduct(product);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(int id)
        {
            ProductRepository repository = new ProductRepository();
            repository.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        public ActionResult DownloadReport()
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Views/Reports"), "ProductDetails.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
             
            }
            ProductManagementDbEntities _db=new ProductManagementDbEntities();
            IList<Product> products = new List<Product>();
             products = _db.Products.Select(x => x).ToList();
            object prods = products.Select(pr => new ProductViewModel(pr)).ToList();
            ReportDataSource rd = new ReportDataSource("DataSet1",prods);
            lr.DataSources.Add(rd);
            IList<ReportParameter> reportParameters = new List<ReportParameter>()
            {
                new ReportParameter("title", "Test Report"),
                new ReportParameter("footer", "Generatd on" + DateTime.Now)
            };
            lr.SetParameters(reportParameters);
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
           
            lr.Refresh();
            renderedBytes = lr.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            Response.AddHeader("Content-Disposition",
                "attachment; filename=Test.pdf");

            return new FileContentResult(renderedBytes,mimeType);
        }
    }
}