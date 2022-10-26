using Microsoft.AspNetCore.Mvc;

namespace Library.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HttpStatusCodesController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
