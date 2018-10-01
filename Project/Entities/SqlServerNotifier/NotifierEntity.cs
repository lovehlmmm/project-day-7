using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Repositories.SqlServerNotifier
{
    public class NotifierEntity
    {
        ICollection<SqlParameter> sqlParameters = new List<SqlParameter>();

        public String SqlQuery { get; set; }

        public String SqlConnectionString { get; set; }

        public ICollection<SqlParameter> SqlParameters
        {
            get
            {
                return sqlParameters;
            }
            set
            {
                sqlParameters = value;
            }
        }

        public static NotifierEntity FromJson(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("NotifierEntity Value can not be null!");

            return JsonConvert.DeserializeObject<NotifierEntity>(value);
        }
    }
}
