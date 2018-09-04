using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class SqlHelper
    {
        private SqlConnection _sqlConnection;

        public SqlHelper(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public bool IsConnection
        {
            get
            {
                if (_sqlConnection.State==System.Data.ConnectionState.Closed)
                {
                    _sqlConnection.Open();
                }

                return true;
            }
        }
    }
}
