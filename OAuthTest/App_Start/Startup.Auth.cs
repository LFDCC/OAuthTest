using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using OAuthTest.Provider;
using Owin;
using System;


namespace OAuthTest
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                CookieName = "OAuthTest.Cookie",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,//允许客户端以http协议请求；
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/oauth2/token"),//密码授权 客户端授权 需要的access_token post
                AuthorizeEndpointPath = new PathString("/oauth2/authorize"), //获取 authorization_code 认证服务请求地址 get
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),//access_token 过期时间

                Provider = new AuthorizationServerProvider(),//授权服务
                AccessTokenProvider = new AccessTokenProvider("test0"),//自定义创建accesstoken 验证accesstoken规则
                RefreshTokenProvider = new RefreshTokenProvider(),//自定义创建refreshtoken 验证refreshtoken规则
                AuthorizationCodeProvider = new AuthorizationCodeProvider(), //authorization_code 授权服务
            };
            app.UseOAuthBearerTokens(OAuthServerOptions);//表示 token_type 使用 bearer 方式

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                Provider = new QueryStringBearerProvider("access_token")
            });
            app.UseCors(CorsOptions.AllowAll);//允许跨域
        }
    }
}
