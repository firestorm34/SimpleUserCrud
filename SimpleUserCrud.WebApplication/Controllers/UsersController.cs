using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleUserCrud.Core.Models;
using SimpleUserCrud.Core.Services;

namespace SimpleUserCrud.WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        public UsersController(UserService userService) {
            _userService = userService;
        }


        public ActionResult Index()
        {
            List<User> users = _userService.GetAll().ToList();
            return View(users);
        }

        public ActionResult Details(Guid id)
        {
            User? user = _userService.GetById(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            UserOperationResult result = _userService.Add(user);
            if (!result.isSuccess)
            {
                foreach (string errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage);

                }
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(Guid id)
        {
            User? user = _userService.GetById(id);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(User user)
        {
            UserOperationResult result = _userService.Update(user);
            if (!result.isSuccess)
            {
                foreach (string errorMessage in result.Errors)
                {
                    ModelState.AddModelError("", errorMessage);

                }
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(Guid id)
        {
            _userService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
