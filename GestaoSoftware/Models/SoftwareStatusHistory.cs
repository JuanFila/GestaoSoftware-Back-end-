using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestaoSoftware.Models.Enums;

namespace GestaoSoftware.Models
{
    [Table("software_status_history")]
    public class SoftwareStatusHistory
    {
        [Key]
        public int Id { get; set; }

        [Column("software_id")]
        public int SoftwareId { get; set; }
        public Software Software { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("old_status")]
        public SoftwareStatus OldStatus { get; set; }

        [Column("new_status")]
        public SoftwareStatus NewStatus { get; set; }

        [Column("changed_at")]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
