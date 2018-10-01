namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product: EntityBase
    {


        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [StringLength(50)]
        [Column("product_name")]
        [Required]
        public string ProductName { get; set; }
        [Column("product_size")]
        [StringLength(20)]
        [Required]
        public string ProductSize { get; set; }
        [Column("product_price",TypeName = "money")]
        public decimal ProductPrice { get; set; }
        [StringLength(10)]
        [Column("status")]
        public string Status { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }

    }
}
