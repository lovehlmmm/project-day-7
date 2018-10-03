using Constants;
using Entities;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace WEB.ScheduledTasks
{

    public class JobSchedule
    {

        public static async System.Threading.Tasks.Task ChangeOrderStatusJob()
        {
            IBaseRepository<Order> _orderService = new BaseRepositoryEF<Order>();
            System.Diagnostics.Debug.WriteLine("Late job!");
            DateTime oldestDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0));
            var orders = await _orderService.FindAllAsync(o => o.Status == OrderStatus.Pending & o.CreatedAt < oldestDate);
            if (orders.Count() > 0)
            {
                foreach (var item in orders)
                {
                    item.IsCancel = false;
                    await _orderService.UpdateAsync(item, item.OrderId);
                }
                //var result = await _orderService.UpdateAllAsync(orders, "OrderId");
            }
        }

    }
}