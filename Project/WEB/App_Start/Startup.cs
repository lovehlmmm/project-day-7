﻿using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB.ScheduledTasks;



[assembly: OwinStartupAttribute(typeof(WEB.App_Start.Startup))]
namespace WEB.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnectionStringServer");
            RecurringJob.AddOrUpdate(() => JobSchedule.ChangeOrderStatusJob(), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => JobSchedule.SendKeyAdmin(), Cron.Daily);
            app.UseHangfireDashboard();
            app.MapSignalR();
            
        }
    }
}