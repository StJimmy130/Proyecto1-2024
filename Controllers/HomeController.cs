using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Data;
using Proyecto1_2024.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto1_2024.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private ApplicationDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    public async Task<IActionResult> Index(int? PersonaID)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;

        // Llamar a InicializarPermisosUsuario
        var permisosInicializados = await InicializarPermisosUsuario();
        if (permisosInicializados == null)
        {
            // Log para saber si ocurrió un error
            Console.WriteLine("Error al inicializar permisos de usuario.");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }




    public async Task<bool> InicializarPermisosUsuario()
{
    try
    {
        // CREAR ROLES SI NO EXISTEN
        if (!await _rolManager.RoleExistsAsync("ADMINISTRADOR"))
        {
            await _rolManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
        }

        if (!await _rolManager.RoleExistsAsync("DEPORTISTA"))
        {
            await _rolManager.CreateAsync(new IdentityRole("DEPORTISTA"));
        }

        // CREAR USUARIO PRINCIPAL
        bool creado = false;
        var usuario = _context.Users.SingleOrDefault(u => u.Email == "admin@workout.com");
        if (usuario == null)
        {
            var user = new IdentityUser { UserName = "admin@workout.com", Email = "admin@workout.com" };
            var result = await _userManager.CreateAsync(user, "Proyecto2024");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "ADMINISTRADOR");
                creado = true;
            }
        }

        return creado;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error al inicializar permisos: " + ex.Message);
        return false;
    }
}



}



