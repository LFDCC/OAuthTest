using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OAuthTest.Models;

namespace OAuthTest.OAuth2
{
    /// <summary>
    /// 授权业务类
    /// </summary>
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //DBcontext _dbcontext;
        //可以注入进来
        public AuthorizationServerProvider(/*DBcontext dbcontex*/)
        {
            //_dbcontext = dbcontex;
        }
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
            if (context.Parameters["grant_type"] == "client_credentials" || context.Parameters["grant_type"] == "authorization_code")
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
                    if (client != null)
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
            }
            else
            {
                context.Validated();
            }
            return Task.FromResult(0);
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

            return Task.FromResult(0);
        }

        #endregion 公共方法


        #region 客户端授权模式
        /// <summary>
        /// 客户端授权模式 grant_type=client_credentials
        /// 生成 access_token（client_credentials 授权方式)client_id=client0&client_secret=secret0 
        /// </summary>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });//GET,POST,PUT,DELETE,PATCH,OPTIONS....

            string clientSecret = context.OwinContext.Get<string>("clientSecret");
            var client = Repository.clients.SingleOrDefault(t => t.clientId == context.ClientId && t.clientSecret == clientSecret);
            if (client != null)
            {
                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, context.ClientId)
                }, OAuthDefaults.AuthenticationType);

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                    { "自定义参数0", "0" },
                    { "自定义参数1", context.ClientId }
                });//自定义输出参数

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
            {
                context.SetError("invalid_client_credentials", "客户端授权失败，clientSecret不正确");
            }

            return Task.FromResult(0);
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
                var identity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, context.UserName),
                    new Claim(ClaimTypes.UserData, user.id.ToString()),
                }, OAuthDefaults.AuthenticationType);

                var props = new AuthenticationProperties(new Dictionary<string, string> {
                    { "自定义参数0", "0" },
                    { "自定义参数1", context.UserName }
                });//自定义输出参数

                var ticket = new AuthenticationTicket(identity, props);

                context.Validated(ticket);
            }
            return Task.FromResult(0);

        }

        #endregion 密码授权模式

        #region 授权码模式


        /**
        * 
        *   第一次请求授权服务（获取 authorization_code），请求地址/authorize
        *   需要的参数：
        *   grant_type：必选，授权模式，值为 "authorization_code"。
        *   response_type：必选，授权类型，值固定为 "code"。
        *   client_id：必选，客户端 ID。
        *   redirect_uri：必选，重定向 URI，URL 中会包含 authorization_code。 （关联到client 配置到数据库中）
        *   scope：可选，申请的权限范围，比如微博授权服务值为 follow_app_official_microblog。
        *   state：可选，客户端的当前状态，可以指定任意值，授权服务器会原封不动地返回这个值，比如微博授权服务值为 weibo。
        
        *   第二次请求授权服务（获取 access_token），需要的参数：        
        *   grant_type：必选，授权模式，值为 "authorization_code"。
        *   code：必选，授权码，值为上面请求返回的 authorization_code。
        *   redirect_uri：必选，重定向 URI，必须和上面请求的 redirect_uri 值一样。
        *   client_id：必选，客户端 ID。
        *   
        *   第二次请求授权服务（获取 access_token），返回的参数：
        *   access_token：访问令牌.
        *   token_type：令牌类型，值一般为 "bearer"。
        *   expires_in：过期时间，单位为秒。
        *   refresh_token：更新令牌，用来获取下一次的访问令牌。
        *   scope：权限范围。
        * 
        **/

        /// <summary>
        /// 生成code
        /// 此处只处理登录授权
        /// </summary>
        public override async Task AuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            await base.AuthorizeEndpoint(context);
        }

        /// <summary>
        /// 验证 redirect_uri
        /// </summary>
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            var client = Repository.clients.SingleOrDefault(t => t.clientId == context.ClientId);
            if (client != null)
            {
                if (context.RedirectUri != client.RedirectUri)
                {
                    context.SetError("redirect_uri不一致");
                }
                else
                {
                    context.Validated(context.RedirectUri);
                }
            }
            else
            {
                context.SetError("客户端验证失败");
            }
            return Task.FromResult(0);
        }
        #endregion

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

            return Task.FromResult(0);
        }
        #endregion 刷新token
    }
}