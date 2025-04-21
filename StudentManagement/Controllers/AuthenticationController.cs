using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentManagement.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserService _userService;

        public AuthenticationController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,PasswordHash")] User user)
        {
            if (!ModelState.IsValid)
                return View(user);
            try
            {
                var authenticateState = await _userService.AuthenticateUserAsync(user.Username, user.PasswordHash);
                if (authenticateState)
                {
                    var authenticatedUser= await _userService.GetUserByUsernameAsync(user.Username);
                    HttpContext.Session.SetString("Username", authenticatedUser.Username);
                    HttpContext.Session.SetString("Role", authenticatedUser.Role.ToString());
                    return RedirectToAction("Index", "Students"); //till idk where to redirect
                }
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(user);
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,PasswordHash,Role,StudentId")] User user)
        {
            if (!ModelState.IsValid)
                return View(user);
            try
            {
                await _userService.AddUserAsync(user);
                return RedirectToAction("Login");
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}