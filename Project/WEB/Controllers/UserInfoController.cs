using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class UserInfoController : Controller
    {

        private readonly IUserService _userService;
        private readonly IBaseService<Address> _addressService;

        public UserInfoController(IUserService userService,IBaseService<Address> addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        // GET: UserInfo
        public ActionResult Index()
        {
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username);
                if (user != null)
                {
                    user.Password = "";
                    ViewBag.ShowUser = user;
                    return View();
                }

            }
            return RedirectToAction("Index", "LoginUser", new { url = Request.Url.ToString() });
        }
        public async System.Threading.Tasks.Task<JsonResult> Delete(long id)
        {
            var check = _addressService.Find(a => a.AddressId == id);
            if (check!=null)
            {
                check.Status = Status.Deleted;
                var deleted = await _addressService.UpdateAsync(check,check.AddressId);
                if (deleted!=null)
                {
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task<JsonResult> UpdateUser(User userUpdate)
        {
            HashingData hashingData = new HashingData(AppSettingConstant.SaltLength);
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username);
                if (user != null)
                {
                    if(userUpdate.Customer.DateOfBirth != null)
                    {
                        user.Customer.DateOfBirth = userUpdate.Customer.DateOfBirth;

                    }
                    if (userUpdate.Password!=null)
                    {
                        user.Password = hashingData.EncryptString(userUpdate.Password,AppSettingConstant.PasswordHash);
                    }
                    user.Customer.Gender = userUpdate.Customer.Gender;
                    user.Customer.PhoneNumber = userUpdate.Customer.PhoneNumber;
                    user.Customer.CustomerName = userUpdate.Customer.CustomerName;
                    user.ModifiedAt = DateTime.Now;
                    foreach (var item in userUpdate.Customer.Addresses)
                    {
                        var checkAddr = _addressService.Find(a => a.AddressId == item.AddressId);
                        if (checkAddr != null)
                        {
                            checkAddr.AddressDetails = item.AddressDetails;
                            checkAddr.ModifiedAt = DateTime.Now;
                            await _addressService.UpdateAsync(checkAddr, checkAddr.AddressId);
                        }
                        else
                        {
                            if (item.AddressDetails!=null)
                            {
                                checkAddr = new Address();
                                checkAddr.AddressDetails = item.AddressDetails;
                                checkAddr.CreatedAt = DateTime.Now;
                                checkAddr.CustomerId = user.CustomerId;
                                checkAddr.Status = Status.Active;
                                await _addressService.AddAsync(checkAddr);
                            }
                        }

                    }
                    var result = await _userService.UpdateAsync(user, user.Username);
                    if (result!=null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                    }
                    
                }

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}