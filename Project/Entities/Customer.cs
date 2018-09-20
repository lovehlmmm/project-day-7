namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer: ModelExtension
    {

        [Key]
        [Column("customer_id")]
        public long CustomerId { get; set; }

        [StringLength(50)]
        [Column("customer_name")]
        public string CustomerName { get; set; }

        [StringLength(11)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [StringLength(6)]
        [Column("gender")]
        public string Gender { get; set; }
 
        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }


        public virtual ICollection<Address> Addresses { get; set; }


        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<CreditCard> CreditCards { get; set; }
    }
}
