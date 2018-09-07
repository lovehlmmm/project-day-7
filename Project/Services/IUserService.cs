using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Services
{
    public interface IUserService:IBaseService<User>
    {
        User CheckLogin(string username, string password);
    }
}
