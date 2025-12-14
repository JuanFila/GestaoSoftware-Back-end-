using System;

namespace GestaoSoftware.Dto
{
    public class CreateSoftwareVersionDto
    {
        public string VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
