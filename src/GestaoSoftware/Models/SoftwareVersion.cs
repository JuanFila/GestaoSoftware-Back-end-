using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoSoftware.Models
{
    [Table("versions")]
    public class SoftwareVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("version_number")]
        public string VersionNumber { get; set; }

        [Column("release_date")]
        public DateTime ReleaseDate { get; set; }

        [Column("is_deprecated")]
        public bool IsDeprecated { get; set; } = false;

        [Column("software_id")]
        public int SoftwareId { get; set; }

        public Software Software { get; set; }
    }
}
