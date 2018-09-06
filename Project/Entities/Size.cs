namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Size: ModelExtension
    {


        [Key]
        [Column("size_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SizeId { get; set; }

        [StringLength(50)]
        [Column("size_name")]
        [Required]
        public string SizeName { get; set; }

        [Column("size")]
        [StringLength(20)]
        [Required]
        public string SizeDetails { get; set; }

        [Column("size_price",TypeName = "money")]
        public decimal SizePrice { get; set; }

    }
}
