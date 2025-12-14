namespace GestaoSoftware.Dto
{
    public class SoftwareVersionResponseDto
    {
        public int Id { get; set; }
        public string VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsDeprecated { get; set; }
    }
}