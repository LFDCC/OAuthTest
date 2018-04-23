using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OAuthTest.Models;

namespace OAuthTest.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        Product[] products = new Product[]
      {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
      };
        
        //[AllowAnonymous]
        [ActionName("all")]
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }
        [Route("api/products")]
        public Product GetProduct(int Id)
        {
            Product model = products.SingleOrDefault(t => t.Id == Id);
            return model;
        }

        [ActionName("filter")]
        public IEnumerable<Product> GetProdcuts(string name)
        {
            IEnumerable<Product> list = products.Where(t => t.Name.Contains(name));
            return list;
        }
    }
}
