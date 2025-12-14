using GestaoSoftware.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoSoftware.Models
{
    [Table("softwares")]
    public class Software
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("status")]
        public SoftwareStatus Status { get; set; } = SoftwareStatus.Ativo;

        [Column("observation")]
        public string Observation { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<SoftwareVersion> Versions { get; set; }
    }
}
