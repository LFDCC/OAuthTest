using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthTest.Models
{
    public class CurUser
    {
        public static User UserInfo
        {
            get
            {
                return HttpContext.Current.Session["UserInfo"] as User;
            }
        }
    }
}