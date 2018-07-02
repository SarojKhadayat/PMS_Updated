using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PMS.Models.PMSModel;

namespace PMS.Models.Repository
{
    public class ProductRepository
    {
        private ProductManagementDbEntities _db = new ProductManagementDbEntities();
        public IList<Product> GetAllProducts()
        {
            //var products = (from prods in db.Products select prods).ToList();
            //return products;         
            return _db.Products.Select(x => x).ToList();
        }

        public int AddProduct(Product product)
        {
            if (GetAllProductCodes().Contains(product.ProductCode))
            {
                return 0;
            }
            _db.Products.Add(product);
            return _db.SaveChanges();
        }
       
        public List<string> GetAllProductCodes()
        {
           return _db.Products.Select(x => x.ProductCode).ToList();
        }

        #region ADO.NET
        public void InsertIntoProductUsingAdo(Product product)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductManagementDbo"].ConnectionString;
            string test = System.Configuration.ConfigurationManager.AppSettings["Name"];
            string query = "Insert into Product (ProductName,ProductDesc,Quantity,ProductCode,CreatedDate,ValidTill,Price)" +
                           "values (@ProductName,@ProductDesc,@Quantity,@ProductCode,@CreatedDate,@ValidTill,@Price)";
            SqlConnection cn = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductDesc", product.ProductDesc);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
                cmd.Parameters.AddWithValue("@CreatedDate", product.CreatedDate);
                cmd.Parameters.AddWithValue("@ValidTill", product.ValidTill);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        public List<Product> GetAllProductsAdo()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductManagementDbo"].ConnectionString;
            string query = "select * from product";
            List<Product> products = new List<Product>();
            SqlConnection con = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();

                    product.ProductName = reader["ProductName"].ToString();
                    product.ProductCode = reader["ProductCode"].ToString();
                    product.ProductDesc = reader["ProductDesc"].ToString();
                    product.Price = Convert.ToInt32(reader["Price"]?.ToString());
                    product.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                    product.ValidTill = Convert.ToDateTime(reader["ValidTill"].ToString());

                    products.Add(product);
                }
            }

            return products;
        }
        public void InsertProductUsingSp(Product product)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductManagementDbo"].ConnectionString;
            string cmdText = "InsertIntoProduct";
            SqlConnection con=new SqlConnection(connectionString);
            using (SqlCommand cmd=new SqlCommand(cmdText,con))
            {
                cmd.Parameters.Add(new SqlParameter("@PCode", SqlDbType.VarChar));
                cmd.Parameters["@PCode"].Value = product.ProductCode;
               // cmd.Parameters.AddWithValue("@PCode", product.ProductCode);
                cmd.Parameters.Add(new SqlParameter("@PName", SqlDbType.VarChar));
                cmd.Parameters["@PName"].Value = product.ProductName;

                cmd.Parameters.Add(new SqlParameter("@PDesc", SqlDbType.VarChar));
                cmd.Parameters["@PDesc"].Value = product.ProductDesc;

                cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Int));
                cmd.Parameters["@Price"].Value = product.Price;

                cmd.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Int));
                cmd.Parameters["@Quantity"].Value = product.Quantity;
                cmd.Parameters.Add(new SqlParameter("@OwnerId", SqlDbType.Int));
                cmd.Parameters["@OwnerId"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@ValidTill", SqlDbType.DateTime));
                cmd.Parameters["@ValidTill"].Value = product.ValidTill;
                cmd.Parameters.Add(new SqlParameter("@PId", SqlDbType.Int));
                cmd.Parameters["@PId"].Value = product.Id;
             //   cmd.Parameters.Add("@PId", SqlDbType.Int).Direction = System.Data.ParameterDirection.ReturnValue;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                //int retval = (int)cmd.Parameters["@PId"].Value;
                con.Close();
            }
        }
        #endregion

        public Product GetProductById(int id)
        {
          // var test1= _db.Products.Where(x => x.Id == id).Select(x => x);
           //var test2= (from pd in _db.Products where pd.Id == id select pd);
            return _db.Products.Find(id);
        }

        public void UpdateProduct(Product product)
        {
            var oldProduct= _db.Products.Find(product.Id);
            product.CreatedDate = DateTime.Now;
            _db.Entry(oldProduct).CurrentValues.SetValues(product);
            _db.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _db.Products.Find(id);
            if (product != null)
            {
                _db.Products.Remove(product);
            }
            
            _db.SaveChanges();
        }
    }
}