using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Constants;
using Entities;
using Services;

namespace WEB.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private IBaseService<Order> _orderService;

        public OrderController(IBaseService<Order> orderService)
        {
            _orderService = orderService;
        }
        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetList()
        {
            try
            {
                var pageNumber = int.Parse(Request.QueryString["pageNumber"]);
                var pageSize = int.Parse(Request.QueryString["pageSize"]);
                var list = await _orderService.GetAllAsync(pageNumber, pageSize, o => o.CreatedAt,
                    o => o.Status != Status.Deleted);
                var total = await _orderService.CountAsync(size => size.Status != Status.Deleted);
                var totalPage = (int)Math.Ceiling((double)(total / pageSize)) + 1;
                var list2 = list.Select(o => new
                {
                    o.OrderId,
                    o.Customer.CustomerName,
                    Total = (o.OrderDetails?.Sum(od =>  od?.Quantity * od?.Product.ProductPrice)),
                    o.Status,
                    o.PhoneNumber,
                    o.Address?.AddressDetails,
                    o.CreatedAt,
                    o.ModifiedAt
                }).ToList();
                return Json(new { status = true, data = list2, totalPage = totalPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> Get(long id)
        {
            try
            {
                Order order = await _orderService.FindAsync(o => o.OrderId == id && o.Status != Status.Deleted);
                if (order!=null)
                {
                    var result = new
                    {
                        order.OrderId,
                        order.Address?.AddressDetails,
                        order.Status,
                        order.CreatedAt,
                        OrderDetails = order.OrderDetails.Select(od=> new {od.Quantity,od.Product.ProductPrice,od.Image,Total= od.Quantity*od.Product.ProductPrice}),
                        Total = order.OrderDetails.Sum(o => o.Quantity * o.Product.ProductPrice),
                        order.Note
                    };
                    return Json(new { status = true,data=result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }
    }
}