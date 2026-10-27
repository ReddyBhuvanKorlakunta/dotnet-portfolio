using EnterprisePortal.Application.Cases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EnterprisePortal.Web.Controllers;
/// <summary>Versioned API. MVC UI and API share CaseService, so the UI and the
/// API can never drift into two different business rules.</summary>
[ApiController, Route("api/v1/cases")]
[Authorize]
public class CasesApiController(CaseService cases) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] CaseQuery q, CancellationToken ct)
        => Ok(await cases.ListAsync(q, ct));
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken ct)
    {
        var r = await cases.GetAsync(id, ct);
        return r.Ok ? Ok(r.Value) : NotFound(new { error = r.Error });
    }
    [HttpPost, Authorize(Roles = "CaseWorker,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCaseRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var user = User.Identity?.Name ?? "api";
        var r = await cases.CreateAsync(req, user, ct);
        return r.Ok ? CreatedAtAction(nameof(Get), new { id = r.Value!.Id }, r.Value) : BadRequest(new { error = r.Error });
    }
    [HttpPatch("{id:int}/status"), Authorize(Roles = "CaseWorker,Admin")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusRequest req, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var user = User.Identity?.Name ?? "api";
        var r = await cases.ChangeStatusAsync(id, req, user, ct);
        return r.Ok ? Ok(r.Value) : BadRequest(new { error = r.Error });
    }
}
