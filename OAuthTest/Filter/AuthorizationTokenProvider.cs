using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;

namespace OAuthTest.Filter
{
    /// <summary>
    /// 创建、验证Token的实现类（默认继承父级）
    /// </summary>
    public class AuthorizationTokenProvider : AuthenticationTokenProvider
    {
        private readonly ConcurrentDictionary<string, string> _authenticationCodes = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);
        
        //<summary>
        //创建Token
        //</summary>
        //<param name="context">上下文</param>
        //<returns></returns>
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            /*
            if (string.IsNullOrEmpty(context.Ticket.Identity.Name)) return;
            string IpAddress = context.Request.RemoteIpAddress + ":" + context.Request.RemotePort;
            var token = new Token()
            {
                ClientId = context.Ticket.Identity.Name,
                ClientType = "client_credentials",
                Scope = context.Ticket.Properties.Dictionary["scope"],
                UserName = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.Parse(context.Ticket.Properties.IssuedUtc.ToString()),
                ExpiresUtc = DateTime.Parse(context.Ticket.Properties.IssuedUtc.ToString()),
                IpAddress = IpAddress
            };
            token.AccessToken = context.SerializeTicket();
            token.RefreshToken = string.Empty;//await _clientAuthorizationService.GenerateOAuthClientSecretAsync();
            //Token没有过期的情况强行刷新，删除老的Token保存新的Token
            if (await _clientAuthorizationService.SaveTokenAsync(token))
            {
                context.SetToken(token.AccessToken);
            }*/
            await base.CreateAsync(context);
        }

        //<summary>
        //验证Token
        //</summary>
        //<param name="context">上下文</param>
        //<returns></returns>
        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //ClaimsIdentity claim = context.Ticket.Identity;
            /*
            var request = new OAuthRequestTokenContext(context.OwinContext, context.Token);

            var ticket = new AuthenticationTicket(new ClaimsIdentity(), new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow.AddYears(-1),
                ExpiresUtc = DateTime.UtcNow.AddYears(-1)
            });
            if (request == null || !string.IsNullOrEmpty(request.Token))
            {
                context.SetTicket(ticket);
            }
            /*
            //验证Token是否过期
            var vaild = await _clientAuthorizationService.VaildOAuthClientSecretAsync();
            if (vaild)
            {
                context.SetTicket(ticket);
            }
            */
            await base.ReceiveAsync(context);
        }
    }
}