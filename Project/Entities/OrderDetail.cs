namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        [Column("order_details_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderDetailsId { get; set; }

        [Column("order_id")]
        public long? OrderId { get; set; }

        [Column("quantity")]
        public int? Quantity { get; set; }

        [Column("size_id")]
        public int? SizeId { get; set; }

        [Column("material_id")]
        public int? MaterialId { get; set; }

        public virtual Material Material { get; set; }

        public virtual Order Order { get; set; }

        public virtual Size Size { get; set; }
    }
}
