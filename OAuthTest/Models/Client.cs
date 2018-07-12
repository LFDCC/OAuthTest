using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthTest.Models
{
    public class Client
    {
        /// <summary>
        /// 客户端ID ，必须
        /// </summary>
        public string clientId { get; set; }
        /// <summary>
        /// 客户端秘钥，客户端授权模式必须要有
        /// </summary>
        public string clientSecret { get; set; }

        public string RedirectUri { get; set; }


    }
}