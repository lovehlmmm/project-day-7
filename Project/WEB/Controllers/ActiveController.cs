using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Helpers;
using Services;

namespace WEB.Controllers
{
    public class ActiveController : Controller
    {
        private IUserService _userService;

        public ActiveController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: Active
        public async Task<ActionResult> Index(string key)
        {
            HashingData hashingData = new HashingData();
            try
            {
                key = hashingData.Decode(key);
                key = hashingData.DecryptString(key, AppSettingConstant.PasswordHash);
                var user = _userService.Find(u => u.Username.Equals(key) & u.Status.Equals(Status.Inactive));
                if (user != null)
                {
                    if (user.ActiveMail!=null)
                    {

                        if (hashingData.DecryptString(user.ActiveMail, AppSettingConstant.PasswordHash)
                            .Equals(key))
                        {
                            user.ActiveMail = null;
                            user.Status = Status.Active;
                            await _userService.UpdateAsync(user, user.Username);
                            TempData["status"] = true;
                            return Redirect("/login");
                        }
                        
                    }
                }
            }
                catch (Exception e) 
            {
                Console.WriteLine(e);
            }

            return Redirect("/Home");
        }
    }
}