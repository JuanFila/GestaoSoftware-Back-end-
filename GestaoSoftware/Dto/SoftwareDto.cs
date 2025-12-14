using GestaoSoftware.Models.Enums;

namespace GestaoSoftware.Dto;

public class CreateSoftwareDto
{
    public string Name { get; set; }
    public string Observation { get; set; }
    public List<SoftwareVersionDto> Versions { get; set; }
}

public class SoftwareVersionDto
{
    public string VersionNumber { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsDeprecated { get; set; }
}

public class SoftwareResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SoftwareStatus Status { get; set; }
    public string Observation { get; set; }

    public List<SoftwareVersionResponseDto> Versions { get; set; }
}