namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address:ModelExtension
    {
        public Address()
        {
            Orders = new HashSet<Order>();
        }
        [Key]
        [Column("address_id")]
        public long AddressId { get; set; }

        [StringLength(255)]
        [Column("address_details")]
        public string AddressDetails { get; set; }

        [Column("customer_id")]
        public long? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
