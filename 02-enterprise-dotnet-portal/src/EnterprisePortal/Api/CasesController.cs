using EnterprisePortal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePortal.Api;

[ApiController]
[Route("api/cases")]
[Authorize(Roles = "CaseWorker,Admin")] // least privilege; anonymous gets 401, not data
public class CasesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List()
    {
        // Parameterised via EF Core — never string-built SQL.
        var items = await db.Cases
            .Select(c => new { c.Id, c.Reference, c.Status, c.CreatedAt })
            .Take(50).ToListAsync();
        return Ok(items);
    }
}
