using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibreriasContext _dbContext;
        string mensaje;
        public AccountController(LibreriasContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View("Login");
        }
        [HttpPost]
        public IActionResult Login(Usuario model)
        {
            if(ModelState.IsValid)
            {
                var user = _dbContext.Usuarios.FirstOrDefault(u=>u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Username)
                    };

                    var identity = new ClaimsIdentity(claims, "login");

                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(principal).Wait();

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    mensaje = "error de usuario y/o contraseña";
                    ViewData["Mensaje"] = mensaje;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Realizar log out
            await HttpContext.SignOutAsync();

            // Limpiar el contexto de base de datos si es necesario
            _dbContext.Dispose();
         

            // Redirigir a la página de inicio u otra página después del log out
            return RedirectToAction("Index", "Account");
        }

    }
}
