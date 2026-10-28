using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EnterprisePortal.Web.Controllers;

public class HomeController(CaseService cases) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CaseStatus? status, CaseType? type, string? search, int page = 1, CancellationToken ct = default)
    {
        ViewData["Search"] = search; ViewData["Status"] = status; ViewData["Type"] = type;
        return View(await cases.ListAsync(new(status, type, search, page, 10), ct));
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var result = await cases.GetAsync(id, ct); return result.Ok ? View(result.Value) : NotFound();
    }
    [Authorize(Roles = "CaseWorker"), HttpGet] public IActionResult Create() => View(new CreateCaseRequest());
    [Authorize(Roles = "CaseWorker"), HttpPost]
    public async Task<IActionResult> Create(CreateCaseRequest model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await cases.CreateAsync(model, User.Identity!.Name!, ct);
        TempData["Notice"] = "Case submitted successfully.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }
    [Authorize(Roles = "CaseWorker"), HttpPost]
    public async Task<IActionResult> ChangeStatus(int id, ChangeStatusRequest model, CancellationToken ct)
    {
        var existing = await cases.GetAsync(id, ct); if (!existing.Ok) return NotFound();
        if (ModelState.IsValid)
        {
            var result = await cases.ChangeStatusAsync(id, model, User.Identity!.Name!, ct);
            if (result.Ok) { TempData["Notice"] = "Case status updated."; return RedirectToAction(nameof(Details), new { id }); }
            ModelState.AddModelError("", result.Error!);
        }
        return View("Details", existing.Value);
    }
    [AllowAnonymous] public IActionResult Error() { Response.StatusCode = 500; return View(); }
    public IActionResult DesignSystem() => View();
}
