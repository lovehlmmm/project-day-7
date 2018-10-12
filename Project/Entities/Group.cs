using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Group:ModelExtension
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [Column("name")]
        public string GroupName { get; set; }
        [Column("max_item")]
        public int MaxItem { get; set; }
        [StringLength(100)]
        [Column("image")]
        public string Image { get; set; }
        [Column("display")]
        public bool? Display { get; set; }
    }
}
