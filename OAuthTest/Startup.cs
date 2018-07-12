using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using OAuthTest.Filter;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(OAuthTest.Startup))]

namespace OAuthTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,//允许客户端以http协议请求；
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/token"),//密码授权 客户端授权 需要的access_token      
                AuthorizeEndpointPath = new PathString("/authorize"), //获取 authorization_code 认证服务请求地址           
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),//access_token 过期时间

                Provider = new AuthorizationServerProvider(),//授权服务
                //AccessTokenProvider = new AccessTokenProvider(),//自定义创建accesstoken 验证accesstoken规则
                RefreshTokenProvider = new RefreshTokenProvider(),//自定义创建refreshtoken 验证refreshtoken规则
                AuthorizationCodeProvider = new AuthorizationCodeProvider(), //authorization_code 授权服务
            };
            app.UseOAuthBearerTokens(OAuthServerOptions);//表示 token_type 使用 bearer 方式
            app.UseCors(CorsOptions.AllowAll);//允许跨域
            app.UseWebApi(config);
        }
    }
}
