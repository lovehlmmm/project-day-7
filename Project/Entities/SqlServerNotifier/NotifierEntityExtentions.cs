using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Repositories.SqlServerNotifier
{
    public static class NotifierEntityExtentions
    {
        public static string ToJson(this NotifierEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NotifierEntity can not be null!");
            return JsonConvert.SerializeObject(entity);
        }
    }
}
