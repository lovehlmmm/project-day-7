using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class UserEmailConfirm
    {
        public UserEmailConfirm(string email,string key,string username)
        {
            Email = email;
            Key = key;
            Username = username;
        }
        public string Email { get; set; }
        public string Key { get; set; }
        public string Username { get; set; }

    }
}   