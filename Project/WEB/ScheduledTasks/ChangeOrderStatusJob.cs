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

    public class ChangeOrderStatusJob 
    {
        private readonly IBaseRepository<Order> _orderService;

        public ChangeOrderStatusJob()
        {
            _orderService = new BaseRepositoryEF<Order>();
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
        }

        public async System.Threading.Tasks.Task Execute()
        {
                    System.Diagnostics.Debug.WriteLine("Late job!");
                    DateTime oldestDate = DateTime.Now.Subtract(new TimeSpan(24, 0, 0, 0, 0));
                    var orders = await _orderService.FindAllAsync(o => o.Status == OrderStatus.Pending & o.CreatedAt >= oldestDate);
                    if (orders.Count() >0)
                    {
                        foreach (var item in orders)
                        {
                            item.Status = OrderStatus.Processing;
                        }
                        var result =await _orderService.UpdateAllAsync(orders, "OrderId");
                    }              
        }

    }
}