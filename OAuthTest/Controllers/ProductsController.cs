using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        [AllowAnonymous]
        public async Task<string> GetAuthorizationCode()
        {
            var clientId = "client0";
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.GetAsync($"http://localhost:4557/authorize?grant_type=authorization_code&response_type=code&client_id={clientId}&redirect_uri={HttpUtility.UrlEncode("http://localhost:4557/api/authorization_code")}");
            var authorizationCode = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine((await response.Content.ReadAsAsync<HttpError>()).ExceptionMessage);
                return null;
            }
            return authorizationCode;
        }

        [HttpGet]
        [Route("api/authorization_code")]
        [AllowAnonymous]
        public HttpResponseMessage Get(string code)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(code, Encoding.UTF8, "text/plain")
            };
        }
    }
}
