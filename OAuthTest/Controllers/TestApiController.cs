using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Text;
using System.Web;

namespace OAuthTest.Controllers
{
    [AllowAnonymous]
    public class TestApiController : ApiController
    {
        string api = "http://localhost:4557";
        string clientId = "client0";
        string clientSecret = "client0";
        HttpClient _httpClient = new HttpClient();

        public async Task<string> GetAuthorizationCode()
        {
            var response = await _httpClient.GetAsync($"{api}/authorize?grant_type=authorization_code&response_type=code&client_id={clientId}&redirect_uri={HttpUtility.UrlEncode(api + "/api/authorization_code")}");
            var authorizationCode = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            return authorizationCode;
        }

        [HttpGet]
        [Route("api/authorization_code")]
        public HttpResponseMessage Get(string code)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(code, Encoding.UTF8, "text/plain")
            };
        }
    }
}
