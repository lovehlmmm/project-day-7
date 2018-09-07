using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class PropertyExtension
    {
        public static object GetPropValue(this object obj, string propertyName)
        {
            foreach (string part in propertyName.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(this object obj, string propertyName)
        {
            object retval = GetPropValue(obj, propertyName);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }
    }
}
