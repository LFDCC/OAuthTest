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

namespace OAuthTest.OAuth2
{
    /// <summary>
    /// 刷新Token的实现类
    /// </summary>
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        private readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// 生成 refresh_token
        /// </summary>
        public override Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(60);

            context.SetToken(Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"));
            _refreshTokens[context.Token] = context.SerializeTicket();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 由 refresh_token 解析成 access_token
        /// 使用完成就移除掉refresh_token
        /// </summary>
        public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_refreshTokens.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
            return Task.FromResult(0);
        }
    }
}