namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User:ModelExtension
    {
        public User()
        {
        }

        [Key]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [Column("password")]
        public string Password { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [StringLength(10)]
        [Column("role")]
        public string Role { get; set; }

        [Column("active_mail", TypeName = "varchar")]
        [StringLength(100)]
        public string ActiveMail { get; set; }
        [Column("last_online")]
        public DateTime? LastOnline { get; set; }      
        [Column("customer_id")]
        public long? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
