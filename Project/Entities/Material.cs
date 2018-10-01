using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Material : ModelExtension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("material_id")]
        public int Id { get; set; }
        [StringLength(50)]
        [Column("material_name")]
        public string Name { get; set; }
        [StringLength(50)]
        [Column("material_details")]
        public string Details { get; set; }
        [Column("material_price", TypeName = "money")]
        public decimal? Price { get; set; }
        [Column("group")]
        public int? Group { get; set; }
        [StringLength(100)]
        [Column("image")]
        public string Image { get; set; }
    }
}
