using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Serializable]
    [DataContract(IsReference = true)]
    public abstract class EntityBase
    {

    }
}
