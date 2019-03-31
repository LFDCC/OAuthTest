using System;
using Microsoft.Owin.Security.Infrastructure;

namespace OAuthTest.Provider
{
    /// <summary>
    /// 创建、验证Token的实现类
    /// </summary>
    public class AccessTokenProvider : AuthenticationTokenProvider
    {
        public AccessTokenProvider(string name)
        {
            Console.WriteLine(name);
        }
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