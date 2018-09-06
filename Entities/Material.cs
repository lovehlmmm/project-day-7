namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Material : ModelExtension
    {
        public Material()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("material_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaterialId { get; set; }

        [StringLength(50)]
        [Column("material_name")]
        public string MaterialName { get; set; }

        [Column("material_price",TypeName = "money")]
        public decimal? MaterialPrice { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
