using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
