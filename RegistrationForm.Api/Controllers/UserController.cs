using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationForm.Business.Services;
using RegistrationForm.Business.ViewModels;

namespace RegistrationForm.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IService _userService;
        private readonly IAuthService _authService;

        public UserController(IService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<RegistrationViewModel>> GetUser(int id)
        {
            var user = await _userService.GetUserViewModelAsync(id);
            if (user == null) return NotFound();
            return user;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUserViewModelsAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JWTToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            //HttpContext.Session.Clear();

            //await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userService.LoginUserAsync(model))
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    return RedirectToAction("Register");
                }

                var token = _authService.GenerateJwtToken(user);

                if (token != null)
                {
                    Response.Cookies.Append("JWTToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                else
                {
                    HttpContext.Session.SetString("JWTToken", token);
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login");


        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _userService.RegisterUserAsync(model))
                return BadRequest(new { error = "Email already exists" });

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine(userId, id.ToString());

            if (userId != id.ToString())
            {
                return Forbid();
            }


            var user = await _userService.GetUserViewModelAsync(id);

            if (user == null)
                return NotFound();

            var viewModel = new RegistrationViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                // Email = user.Credential.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostalCode = user.PostalCode
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegistrationViewModel model)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(userId, id.ToString());
            if (userId != id.ToString())
                return Forbid();

            if (!await _userService.EditUserAsync(id, model))
                return NotFound();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userService.DeleteUserAsync(id))
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
