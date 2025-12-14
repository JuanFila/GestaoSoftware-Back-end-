using GestaoSoftware.Data;
using GestaoSoftware.Dto;
using GestaoSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/softwares/{softwareId}/[controller]")]
[Authorize]
public class VersionsController : ControllerBase
{
    private readonly AppDbContext _context;
    public VersionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVersion(
    int softwareId,
    [FromBody] CreateSoftwareVersionDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var software = await _context.Softwares
            .FirstOrDefaultAsync(s => s.Id == softwareId && s.UserId == userId);

        if (software == null)
            return NotFound(new { message = "Software não encontrado ou não pertence ao usuário" });

        var version = new SoftwareVersion
        {
            VersionNumber = dto.VersionNumber,
            ReleaseDate = dto.ReleaseDate,
            SoftwareId = softwareId
        };

        _context.Versions.Add(version);
        await _context.SaveChangesAsync();

        return Ok(new SoftwareVersionResponseDto
        {
            Id = version.Id,
            VersionNumber = version.VersionNumber,
            ReleaseDate = version.ReleaseDate,
            IsDeprecated = version.IsDeprecated
        });
    }

    [HttpPut("{versionId}/deprecated")]
    public async Task<IActionResult> UpdateDeprecated(
       int softwareId,
       int versionId,
       [FromBody] UpdateVersionDeprecatedDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

       
        var software = await _context.Softwares
            .FirstOrDefaultAsync(s => s.Id == softwareId && s.UserId == userId);

        if (software == null)
            return NotFound(new { message = "Software não encontrado ou não pertence ao usuário" });

        var version = await _context.Versions
            .FirstOrDefaultAsync(v => v.Id == versionId && v.SoftwareId == softwareId);

        if (version == null)
            return NotFound(new { message = "Versão não encontrada para este software" });

        version.IsDeprecated = dto.IsDeprecated;
        await _context.SaveChangesAsync();

        var response = new SoftwareVersionResponseDto
        {
            Id = version.Id,
            VersionNumber = version.VersionNumber,
            ReleaseDate = version.ReleaseDate,
            IsDeprecated = version.IsDeprecated
        };

        return Ok(response);
    }
}