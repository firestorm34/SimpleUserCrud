using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimpleUserCrud.Core.Services;
using SimpleUserCrud.WebApplication.Models;
using System.Diagnostics;

namespace SimpleUserCrud.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        UserService _userService;

		public HomeController(UserService userService)
        {
			_userService = userService;

		}

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            IExceptionHandlerFeature exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandlerFeature != null)
            {
                Exception exception = exceptionHandlerFeature.Error;
                var exceptionMessage = exception.Message;
                return View(new ErrorViewModel( exceptionMessage));
            }
            return View();
        }
    }
}
