using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{ 

    [Table("Notification")]
    public class Notification: ModelExtension
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
        [Column("type")]
        public int? Type { get; set; }
        [StringLength(500)]
        [Column("details")]
        public string Details { get; set; }
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(50)]
        [Column("sent_to")]
        public string SendTo { get; set; }
        [Column("is_read"),DataType("bit")]
        public bool  IsRead { get; set; }
        [Column("is_reminder"), DataType("bit")]
        public bool IsReminder { get; set; }
        [Column("code")]
        [StringLength(100)]
        public string Code { get; set; }
        [Column("notification_type"),DataType("nvarchar")]
        [StringLength(100)]
        public string NotificationType { get; set; }
    }
}
