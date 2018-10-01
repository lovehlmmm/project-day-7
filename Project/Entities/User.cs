namespace Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User:EntityBase
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

        [Column("customer_id")]
        public long? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [StringLength(10)]
        [Column("status")]
        public string Status { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime? ModifiedAt { get; set; }
    }
}
