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
    /// 创建、验证Token的实现类
    /// </summary>
    public class AccessTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            base.Create(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            base.Receive(context);
        }
    }
}