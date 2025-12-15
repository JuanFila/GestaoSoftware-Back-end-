using GestaoSoftware.Data;
using GestaoSoftware.Dto;
using GestaoSoftware.Models;
using GestaoSoftware.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SoftwaresController : ControllerBase
{
    private readonly AppDbContext _context;
    public SoftwaresController(AppDbContext context)
    {
        _context = context;
    }
    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSoftware([FromBody] CreateSoftwareDto dto)
    {
        var userId = GetUserId();

        var software = new Software
        {
            Name = dto.Name,
            Status = SoftwareStatus.Ativo,
            Observation = dto.Observation,
            UserId = userId,
            Versions = dto.Versions.Select(v => new SoftwareVersion
            {
                VersionNumber = v.VersionNumber,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated
            }).ToList()
        };

        _context.Softwares.Add(software);
        await _context.SaveChangesAsync();

        var response = new SoftwareResponseDto
        {
            Id = software.Id,
            Name = software.Name,
            Status = software.Status,
            Observation = software.Observation,
            Versions = software.Versions.Select(v => new SoftwareVersionResponseDto
            {
                Id = v.Id,
                VersionNumber = v.VersionNumber,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated
            }).ToList()
        };

        return CreatedAtAction(nameof(GetAll), new { id = software.Id }, response);
    }
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelSoftware(int id)
    {
        var userId = GetUserId();

        var software = await _context.Softwares
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

        if (software == null)
            return NotFound(new { message = "Software não encontrado ou não pertence ao usuário" });

        if (software.Status == SoftwareStatus.Cancelado)
            return BadRequest(new { message = "Software já está cancelado" });

        var history = new SoftwareStatusHistory
        {
            SoftwareId = software.Id,
            UserId = userId,
            OldStatus = software.Status,
            NewStatus = SoftwareStatus.Cancelado,
            ChangedAt = DateTime.UtcNow
        };

        software.Status = SoftwareStatus.Cancelado;

        _context.SoftwareStatusHistories.Add(history);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Software cancelado com sucesso" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();

        var softwares = await _context.Softwares
            .Where(s =>
                s.Status == SoftwareStatus.Ativo &&
                s.UserId == userId
            )
            .Include(s => s.Versions)
            .ToListAsync();

        var response = softwares.Select(s => new SoftwareResponseDto
        {
            Id = s.Id,
            Name = s.Name,
            Status = s.Status,
            Observation = s.Observation,
            Versions = s.Versions.Select(v => new SoftwareVersionResponseDto
            {
                Id = v.Id,
                VersionNumber = v.VersionNumber,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated
            }).ToList()
        });

        return Ok(response);
    }

    [HttpGet("cancel-history")]
    public async Task<IActionResult> GetCancelHistory()
    {
        var userId = GetUserId();

        var history = await _context.SoftwareStatusHistories
            .Where(h =>
                h.NewStatus == SoftwareStatus.Cancelado &&
                h.UserId == userId
            )
            .Include(h => h.User)
            .Include(h => h.Software)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();

        var response = history.Select(h => new SoftwareStatusHistoryResponseDto
        {
            SoftwareName = h.Software.Name,
            UserName = h.User.Name,
            UserEmail = h.User.Email,
            OldStatus = h.OldStatus.ToString(),
            NewStatus = h.NewStatus.ToString(),
            ChangedAt = h.ChangedAt
        });

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSoftware(
    int id,
    [FromBody] UpdateSoftwareDto dto)
    {
        var userId = GetUserId();

        var software = await _context.Softwares
            .Include(s => s.Versions)
            .FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.UserId == userId
            );

        if (software == null)
            return NotFound(new { message = "Software não encontrado ou não pertence ao usuário" });

        software.Name = dto.Name;
        software.Observation = dto.Observation;

        await _context.SaveChangesAsync();

        var response = new SoftwareResponseDto
        {
            Id = software.Id,
            Name = software.Name,
            Status = software.Status,
            Observation = software.Observation,
            Versions = software.Versions.Select(v => new SoftwareVersionResponseDto
            {
                Id = v.Id,
                VersionNumber = v.VersionNumber,
                ReleaseDate = v.ReleaseDate,
                IsDeprecated = v.IsDeprecated
            }).ToList()
        };

        return Ok(response);
    }


}