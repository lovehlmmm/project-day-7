using Constants;
using Entities;
using Helpers;
using Newtonsoft.Json;
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
        private readonly IBaseService<CreditCard> _creditcardService;

        public UserInfoController(IUserService userService,IBaseService<Address> addressService,IBaseService<CreditCard> creditService)
        {
            _userService = userService;
            _addressService = addressService;
            _creditcardService = creditService;
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
        public async System.Threading.Tasks.Task<JsonResult> UpdateUser(User userUpdate,CreditCard creditCard)
        {
            HashingData hashingData = new HashingData();
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
                    if (creditCard.CreditNumber!=null|creditCard.CVC!=null|creditCard.Expire!=null)
                    {
                        creditCard.CustomerId = user.CustomerId.Value;
                        creditCard.CreatedAt = DateTime.Now;
                        creditCard.Status = Status.Active;
                        creditCard.CreditNumber = hashingData.Decode(creditCard.CreditNumber);
                        user.Customer.CreditCards.Add(creditCard);
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
        public JsonResult GetCreditCard(long id)
        {
            
            try
            {
                var credit = _creditcardService.Find(c => c.CreditCardId == id);
                
                if (credit!=null)
                {
                    credit.CreditNumber = credit.CreditNumber = AESEncrytDecry.DecryptStringAES(credit.CreditNumber).Substring(12, 4);
                    return Json(new { status = true,data = credit}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async System.Threading.Tasks.Task<JsonResult> DeleteCredit (int id)
        {
            try
            {
                var deleteCre = _creditcardService.Find(c => c.CreditCardId == id);
                if (deleteCre!=null)
                {
                    deleteCre.Status = Status.Deleted;
                    var deleted =await _creditcardService.UpdateAsync(deleteCre, id);
                    if (deleted!=null)
                    {
                        return Json(new { status = true }, JsonRequestBehavior.AllowGet);


                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);

        }
        public async System.Threading.Tasks.Task<JsonResult> AddCreditCard (string creditCard)
        {
            var message = "";
            HashingData hashing = new HashingData();
            var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userService.Find(u => u.Username == userSession.Username);
                if (user != null)
                {
                    if (creditCard!=null)
                    {
                        var card = JsonConvert.DeserializeObject<CreditCard>(creditCard);
                        card.CreditNumber =hashing.Decode(card.CreditNumber);
                            card.CreatedAt = DateTime.Now;
                            card.Status = Status.Active;
                            card.Expire = card.Expire.Remove(3, 2);
                            card.CustomerId = user.CustomerId.Value;
                            var added = await _creditcardService.AddAsync(card);
                            if (added != null)
                            {
                                return Json(new { status = true,card = new { added.CreditNumber,added.CreditCardId,added.Expire}}, JsonRequestBehavior.AllowGet);
                            }
                        
                    }
                    
                }

            }
            return Json(new { status = false,message}, JsonRequestBehavior.AllowGet);
        }
    }
}