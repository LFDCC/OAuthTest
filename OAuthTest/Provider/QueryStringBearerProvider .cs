using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OAuthTest.Provider
{
    /// <summary>
    /// 扩展使用url方式传递access_token
    /// </summary>
    public class QueryStringBearerProvider : OAuthBearerAuthenticationProvider
    {
        private string _name { get; set; }
        
        public QueryStringBearerProvider(string name)
        {
            _name = name;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get(_name);

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult(0);
        }
    }
}