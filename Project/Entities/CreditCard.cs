namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CreditCard")]
    public partial class CreditCard:ModelExtension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("credit_card_id")]
        public long CreditCardId { get; set; }
        
        [Column("customer_id")]
        public long CustomerId { get; set; }
        [StringLength(100)]
        [Column("credit_number")]
        public string CreditNumber { get; set; }
        [StringLength(5)]
        [Column("credit_expire")]
        public string Expire { get; set; }
        [StringLength(3)]
        [Column("cvc")]
        public string CVC { get; set; }
        public Customer Customer { get; set; }
        
    }
}
