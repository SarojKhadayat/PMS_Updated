using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Models.PMSModel;

namespace PMS.Models
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            ProductCode = product.ProductCode;
            ProductName = product.ProductName;
            CreatedDate = product.CreatedDate;
            OwnerId = product.OwnerId;
            Price = product.Price;
            Quantity = product.Quantity;
            ProductDesc = product.ProductDesc;
            ValidTill = product.ValidTill;
        }

        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ValidTill { get; set; }
        public Nullable<int> Price { get; set; }
        public int Quantity { get; set; }
    }
}