using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuthTest.Models
{
    public class Repository
    {
        /// <summary>
        /// 客户端集合
        /// </summary>
        public static List<Client> clients
        {
            set
            {
                clients = value;
            }
            get
            {
                return new List<Client> {
                     new Client { clientId="client0", clientSecret="client0",RedirectUri="http://www.baidu.com" },
                     new Client { clientId="client1", clientSecret="client1" ,RedirectUri="http://www.baidu.com"},
                     new Client { clientId="client2", clientSecret="client2" ,RedirectUri="http://www.baidu.com"},
                     new Client { clientId="client3", clientSecret="client3",RedirectUri="http://www.baidu.com" },
                     new Client { clientId="client4", clientSecret="client4",RedirectUri="http://www.baidu.com" },
                     new Client { clientId="client5", clientSecret="client5",RedirectUri="http://www.baidu.com" },
                };
            }
        }

        /// <summary>
        /// 用户集合
        /// </summary>
        public static List<User> users
        {
            set
            {
                users = value;
            }
            get
            {
                return new List<User> {
                     new User { id=1, username="user0",password="user0" },
                     new User { id=2, username="user1",password="user1" },
                     new User { id=3, username="user2",password="user2" },
                     new User { id=4, username="user3",password="user3" },
                     new User { id=5, username="user4",password="user4" },
                     new User { id=6, username="user5",password="user5" },
                };
            }
        }

    }
}