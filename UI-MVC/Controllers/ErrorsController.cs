using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UIMVC.Models;

namespace UIMVC.Controllers
{
    public class ErrorsController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Error/{statuscode}")]
        public IActionResult HandleErrorCode(int statuscode, string path)
        {
            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, de gevraagde pagina kon niet worden gevonden.";
                    ViewBag.RouteOfException = path;
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Er is blijkbaar iets misgelopen langs onze kant";
                    ViewBag.RouteOfException = path;
                    break;
            }

            return View();
        }

        [Route("ConfirmAccount")]
        public IActionResult ConfirmAccount()
        {
            return View();
        }
    }
}
