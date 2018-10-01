namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order:EntityBase
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderId { get; set; }
        [Column("customer_id")]
        public long? CustomerId { get; set; }

        [StringLength(11)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [Column("folder_image")]
        public string FolderImage { get; set; }

        [Column("credit_card_id")]
        public long? CreditCardId { get; set; }

        [Column("address_id")]
        public long? AddressId { get; set; }

        public virtual Address Address { get; set; }

        public virtual CreditCard CreditCard { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [StringLength(10)]
        [Column("status")]
        public string Status { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
    }
}
