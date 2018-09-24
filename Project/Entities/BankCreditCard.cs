using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Table("CreditCard")]    public class BankCreditCard : ModelExtension    {        [Key]        [Column("credit_number")]        public string CreditNumber { get; set; }        [Column("expire")]        [StringLength(5)]        [Required]        public string Expire { get; set; }        [Column("cvc")]        [StringLength(3)]        [Required]        public string CVC { get; set; }        [Column("Balance"), DataType("money")]        public decimal Balance { get; set; }    }
}
