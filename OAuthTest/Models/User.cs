using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthTest.Models
{
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }
}