using EnterprisePortal.Application.Cases;
using EnterprisePortal.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace EnterprisePortal.Web.Controllers;
public class HomeController(CaseService cases) : Controller
{
    // Explicit query parameters — record binding for GET queries is fragile
    // in MVC, so the controller builds the query object itself.
    public async Task<IActionResult> Index(
        CaseStatus? status, CaseType? type, string? search,
        int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var result = await cases.ListAsync(new CaseQuery(status, type, search, page, pageSize), ct);
        return View(result);
    }
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var r = await cases.GetAsync(id, ct);
        return r.Ok ? View(r.Value) : NotFound();
    }
    public IActionResult Error() => View();
}
