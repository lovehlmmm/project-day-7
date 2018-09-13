using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class UserEmailConfirm
    {
        public UserEmailConfirm(string email,string key)
        {
            Email = email;
            Key = key;
        }
        public string Email { get; set; }
        public string Key { get; set; }

    }
}