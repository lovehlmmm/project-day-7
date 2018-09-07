using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ModelExtension
    {
        [StringLength(10)]
        [Column("status")]
        public string Status { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
    }
}
