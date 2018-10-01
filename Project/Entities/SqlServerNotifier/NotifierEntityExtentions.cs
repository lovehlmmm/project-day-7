using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Repositories.SqlServerNotifier
{
    public static class NotifierEntityExtentions
    {
        public static String ToJson(this NotifierEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("NotifierEntity can not be null!");
            return new JavaScriptSerializer().Serialize(entity);
        }
    }
}
