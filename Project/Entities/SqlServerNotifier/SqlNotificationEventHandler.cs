using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SqlServerNotifier
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}
