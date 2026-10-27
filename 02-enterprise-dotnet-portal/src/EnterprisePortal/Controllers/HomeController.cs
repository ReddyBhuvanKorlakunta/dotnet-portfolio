using EnterprisePortal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePortal.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    // MVC: routing -> controller -> view. No business logic in the view.
    public async Task<IActionResult> Index()
    {
        var cases = await db.Cases.OrderByDescending(c => c.CreatedAt).Take(20).ToListAsync();
        return View(cases);
    }
}
