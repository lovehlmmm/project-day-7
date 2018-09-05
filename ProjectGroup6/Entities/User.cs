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
            Customers = new HashSet<Customer>();
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

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
