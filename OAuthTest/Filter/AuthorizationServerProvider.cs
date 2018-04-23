using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using OAuthTest.Models;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 授权业务类
    /// </summary>
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        #region 公共方法 
        /// <summary>  
        /// 该方法表示 谁能发起授权
        /// 第三方平台发起授权请求，客户端模式在此验证，其他模式可以忽略context.Validated();
        /// 验证客户端 [Authorization Basic Base64(clientId:clientSecret)|Authorization: Basic 5zsd8ewF0MqapsWmDwFmQmeF0Mf2gJkW]
        /// 对third party application(也有可能是自己的内部应用) 认证  
        /// 为third party application颁发appKey和appSecret存储到数据库，验证key、secret的有效性。在此省略了颁发验证 appKey和appSecrect的环节        
        /// </summary>  
        /// <param name="context"></param>  
        /// <returns></returns>  
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            //Basic 、from 两种请求方式，Basic从header[Authorization](解密)取 ，from 从Parameters[clientId] Parameters[clientSecret] 取 
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) || context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                if (!string.IsNullOrEmpty(clientSecret))
                {
                    //如果包含clientSecret，如果是客户端授权模式，后面会用到
                    context.OwinContext.Set("clientSecret", clientSecret);
                }
                //模拟请求数据库验证
                var client = Repository.clients.SingleOrDefault(t => t.clientId == clientId);
                if (clientId != null)
                {
                    context.Validated(clientId);
                }
                else
                {
                    context.SetError("invalid_client", "客户端验证失败");
                }
            }
            else
            {
                context.SetError("invalid_client", "客户端验证失败");
            }
            return Task.FromResult<object>(null);
        }
        /// <summary>
        /// 把Context中的属性加入到token中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return base.TokenEndpoint(context);
        }

        #endregion 公共方法


        #region 客户端授权模式
        /// <summary>
        /// 客户端授权模式 grant_type=client_credentials
        /// 生成 access_token（client_credentials 授权方式）        
        /// </summary>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });//GET,POST,PUT,DELETE,PATCH,OPTIONS....

            string clientSecret = context.OwinContext.Get<string>("clientSecret");
            var client = Repository.clients.SingleOrDefault(t => t.clientId == context.ClientId && t.clientSecret == clientSecret);
            if (client != null)
            {
                var identity = new ClaimsIdentity(new GenericIdentity(
                                context.ClientId, OAuthDefaults.AuthenticationType),
                                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                    { "code", "0" },
                    { "clientid", context.ClientId },
                });//自定义输出参数

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("invalid_client_credentials", "客户端授权失败，clientSecret不正确");
            }

            return base.GrantClientCredentials(context);
        }

        #endregion 客户端授权模式

        #region 密码授权模式
        /// <summary>
        /// 密码授权模式 grant_type=password
        /// Resource Owner Password Credentials Grant 的授权方式；
        /// 验证用户名与密码 [Resource Owner Password Credentials Grant[username与password]|grant_type=password&username=irving&password=654321]
        /// 重载 OAuthAuthorizationServerProvider.GrantResourceOwnerCredentials() 方法即可
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //模拟数据库验证
            var user = Repository.users.SingleOrDefault(t => t.username == context.UserName && t.password == context.Password);
            if (user == null)
            {
                context.SetError("invalid_identity", "用户名或密码错误");
            }
            else
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.UserData, user.id.ToString()));

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                    { "code", "0" },
                    { "username", context.UserName },
                    { "pwd", context.Password }
                });//自定义输出参数

                var ticket = new AuthenticationTicket(identity, props);

                context.Validated(ticket);
            }
            return base.GrantResourceOwnerCredentials(context);

        }

        #endregion 密码授权模式

        #region 刷新token
        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        #endregion 刷新token
    }
}