using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants;
using Helpers;
using WEB.Models;

namespace WEB.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Confirm()
        {
            try
            {
                var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer);
                if (userSession==null)
                {
                    return Json(new { status = false });
                }
                var cartSession = SessionHelper.GetSession(AppSettingConstant.CartSession);
                if (cartSession!=null)
                {
                    var user = userSession as UserSession;
                    String path = Server.MapPath(string.Format("~/Images/Upload/{0}/{1}",user.Username,DateTime.Now.ToString())); //Path

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    foreach (var item in cartSession as List<CartItem>)
                    {
                        string imgPath = Path.Combine(path, item.ImageTitle);
                        byte[] imageBytes = Convert.FromBase64String(item.Image);
                        System.IO.File.WriteAllBytes(imgPath, imageBytes);
                    }
                }
                
            }
            catch (Exception e)
            {
            }
            return Json(new { status = false });
        }
        public ActionResult ConfirmTest()
        {
            try
            {
                var userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer);
                if (userSession == null)
                {
                    return Redirect("/home");
                }
                var cartSession = SessionHelper.GetSession(AppSettingConstant.CartSession);
                if (cartSession != null)
                {
                    var user = userSession as UserSession;
                    string folderPath = string.Format("~/Images/Upload/{0}/{1}_{2}", user.Username, DateTime.Now.Second,DateTime.Now.Millisecond);
                    
                    String path = Server.MapPath(folderPath); //Path

                    //Check if directory exist
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                    }
                    foreach (var item in cartSession as List<CartItem>)
                    {
                        string imgPath = Path.Combine(path, item.ImageTitle);
                        byte[] imageBytes = Convert.FromBase64String(item.Image);
                        System.IO.File.WriteAllBytes(imgPath, imageBytes);
                    }
                    return Redirect("/upload");
                }

            }
            catch (Exception e)
            {
            }
            return Redirect("/home");
        }
    }
}