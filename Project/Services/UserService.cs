using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Constants;
using Entities;
using Helpers;
using Repositories;

namespace Services
{
    public class UserService:BaseService<User>,IUserService
    {
        private readonly IBaseRepository<User> _repositoryUser;
        private readonly HashingData _hashingData;
        public UserService(IBaseRepository<User> repositoryUser) : base(repositoryUser)
        {
            _repositoryUser = repositoryUser;
            _hashingData = new HashingData();
        }
        public User CheckLogin(string username, string password,string role)
        {
            User user = _repositoryUser.Find(u =>
                u.Username == username & u.Status!=Status.Deleted & u.Role==role);
            if(user!=null)
            {
                if (password.Equals(_hashingData.DecryptString(user.Password, AppSettingConstant.PasswordHash)))
                return user;
            }

            return null;
        }

        public bool Register(User user)
        {
            user.Password = _hashingData.EncryptString(user.Password, AppSettingConstant.PasswordHash);
            var resultUser =
                TransactionRepository.GenericSafeTransaction<POPContext>(context =>
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                });
                if (resultUser)
                {
                    return true;
                }
            return false;
        }
    }
}
