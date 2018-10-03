using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants;
namespace Helpers
{
    public static class ExchangeTime
    {
        public static string Exchange(DateTime date)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * Time.MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * Time.MINUTE)
                return "a minute ago";

            if (delta < 45 * Time.MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * Time.MINUTE)
                return "an hour ago";

            if (delta < 24 * Time.HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * Time.HOUR)
                return "yesterday";

            if (delta < 30 * Time.DAY)
                return ts.Days + " days ago";

            if (delta < 12 * Time.MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
    }
}
