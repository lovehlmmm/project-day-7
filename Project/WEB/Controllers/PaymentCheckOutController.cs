using Constants;
using Entities;
using Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEB.Helpers;
using WEB.Models;

namespace WEB.Controllers
{
    public class PaymentCheckOutController : Controller
    {
        private readonly IBaseService<Product> _productRepository;
        private readonly IBaseService<Address> _addressRepository;
        private readonly IBaseService<User> _userRepository;
        private readonly IBankService _bankService;
        private readonly IBaseService<OrderDetail> _orderDetailsService;
        private readonly IBaseService<Order> _orderService;
        private readonly IOrderService _orderServiceTrans;

        public PaymentCheckOutController(IOrderService orderServiceTrans, IBaseService<Product> productRepository, IBaseService<Address> addressRepository, IBaseService<User> userRepository, IBankService bankService, IBaseService<Order> orderService, IBaseService<OrderDetail> orderDetailsService)
        {
            _productRepository = productRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _bankService = bankService;
            _orderDetailsService = orderDetailsService;
            _orderService = orderService;
            _orderServiceTrans = orderServiceTrans;
        }
        // GET: PaymentCheckOut
        public ActionResult Index()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var checkOut = TempData["checkout"] as CheckOut;
                    if (checkOut != null)
                    {
                        var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                        ViewBag.Cart = cart;
                        ViewBag.User = user;
                        ViewBag.CheckOut = checkOut;
                        return View();
                    }

                }

            }
            return Redirect("/home");
        }
        public ActionResult GetModalCredit()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;

            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    ViewBag.User = user;
                }
            }
            return PartialView("~/Views/PaymentCheckOut/ModalCredit.cshtml");
        }

        public JsonResult GetCredit(long id)
        {
            try
            {
                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
                if (userSession != null)
                {
                    var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                    if (user != null)
                    {
                        var credit = user.Customer.CreditCards.SingleOrDefault(a => a.CustomerId == user.CustomerId & a.CreditCardId == id & a.Status.Equals(Status.Active));
                        if (credit != null)
                        {
                            return Json(new { status = true, credit = new { CreditNumber = credit.CreditNumber.Substring(12,4), credit.CreditCardId, credit.Expire } }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAddress(long id)
        {
            try
            {
                UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
                if (userSession != null)
                {
                    var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                    if (user != null)
                    {
                        var address = user.Customer.Addresses.SingleOrDefault(a => a.CustomerId == user.CustomerId & a.AddressId == id);
                        if (address != null)
                        {
                            return Json(new { status = true, address = new { address.AddressDetails, address.AddressId } }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCard()
        {
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var cards = user.Customer.CreditCards.Where(c => c.Status == Status.Active).ToList();
                    ViewBag.Credits = cards;
                }

            }
            return PartialView("~/Views/PaymentCheckOut/CardPartial.cshtml");
        }

        public JsonResult Payment(long id)
        {
            var message = "";
            UserSession userSession = SessionHelper.GetSession(AppSettingConstant.LoginSessionCustomer) as UserSession;
            if (userSession != null)
            {
                var user = _userRepository.Find(u => u.Status.Equals(Status.Active) && u.Username.Equals(userSession.Username));
                if (user != null)
                {
                    var orderSession = SessionHelper.GetSession(AppSettingConstant.CheckOutSession) as Order;
                    if (orderSession != null)
                    {
                        var card = user.Customer.CreditCards.FirstOrDefault(c => c.CreditCardId == id & c.Status == Status.Active);
                        if (card != null)
                        {
                            var bankCredit = _bankService.CheckCard(card.CreditNumber, card.Expire, card.CVC);
                            if (bankCredit != null)
                            {
                                var cart = SessionHelper.GetSession(AppSettingConstant.CartSession) as List<CartItem>;
                                var amount = cart.Sum(c => c.Quantity * (c.Product.ProductPrice + c.Material.Price));
                                string rootPath = "~/Images/Upload/";
                                var extenPath = string.Format("{0}/{1}_{2}", user.Username, DateTime.Now.Second, DateTime.Now.Millisecond);
                                string path = Server.MapPath(rootPath+extenPath); //Path

                                //Check if directory exist
                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                                }
                                foreach (var item in cart as List<CartItem>)
                                {
                                    string imgPath = Path.Combine(path, item.ImageTitle);
                                    byte[] imageBytes = Convert.FromBase64String(item.Image);
                                    System.IO.File.WriteAllBytes(imgPath, imageBytes);
                                }
                                if (bankCredit.Balance >= amount)
                                {
                                    bankCredit.Balance -= amount.Value;
                                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                                    foreach (var item in cart)
                                    {
                                        OrderDetail orderDetail = new OrderDetail()
                                        {
                                            MaterialId = item.Material.Id,
                                            ProductId = item.Product.ProductId,
                                            Quantity = item.Quantity,
                                            Option = item.Option,
                                            Image = item.ImageTitle
                                        };
                                        orderDetails.Add(orderDetail);
                                    }
                                    orderSession.CreditCardId = card.CreditCardId;
                                    orderSession.CreatedAt = DateTime.Now;
                                    orderSession.Status = OrderStatus.Pending;
                                    orderSession.CustomerId = user.CustomerId;
                                    orderSession.FolderImage = extenPath;
                                    var transac = _orderServiceTrans.TransactionPayment(orderSession, orderDetails, bankCredit);
                                    if (transac!=null)
                                    {
                                        var addressDetails = _addressRepository.Find(a => a.AddressId == orderSession.AddressId);
                                        MailOrder model = new MailOrder(cart, transac.OrderId, user.Email, addressDetails.AddressDetails, user.Customer.CustomerName, card.CreditNumber,transac.FolderImage, amount.Value,transac.PhoneNumber);
                                        var body = ViewToString.RenderRazorViewToString(this, "MailOrder", model);
                                        var bodyAdmin = ViewToString.RenderRazorViewToString(this, "MailBackAdmin", model);
                                        string mailAdmin = ConfigurationManager.AppSettings["mailadmin"];
                                        Task.Factory.StartNew((() =>
                                        {
                                            SendEmail.Send(mailAdmin, bodyAdmin, "New order notification!");
                                            SendEmail.Send(user.Email, body, "Your order information!");
                                        }));
                                        SessionHelper.Delete(AppSettingConstant.CartSession);
                                        TempData["Success"] = "Order Success";
                                        return Json(new { status = true, message,transac.OrderId}, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists(path))
                                        {
                                            System.IO.File.Delete(path);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
        }

    }
}