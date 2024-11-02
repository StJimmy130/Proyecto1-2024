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

        await InicializarPermisosUsuario();

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





    public async Task<JsonResult> InicializarPermisosUsuario()
    {
        // CREAR ROLES SI NO EXISTEN
        // Verifica si el rol "ADMINISTRADOR" ya existe en la base de datos.
        var nombreRolAdminExiste = _context.Roles.Where(r => r.Name == "ADMINISTRADOR").SingleOrDefault();

        // Si el rol "ADMINISTRADOR" no existe, se crea un nuevo rol con ese nombre.
        if (nombreRolAdminExiste == null)
        {
            var roleAdminResult = await _rolManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
        }

        // CREAR USUARIO PRINCIPAL
        bool creado = false;

        // Busca si ya existe un usuario con el correo "admin@workout.com".
        var usuario = _context.Users.Where(u => u.Email == "admin@workout.com").SingleOrDefault();

        // Si no existe un usuario con ese correo, se procede a crearlo.
        if (usuario == null)
        {
            var user = new IdentityUser { UserName = "admin@workout.com", Email = "admin@workout.com" };

            // Crea el usuario con la contraseña predeterminada "Proyecto2024".
            var result = await _userManager.CreateAsync(user, "Proyecto2024");

            // Asigna el rol "ADMINISTRADOR" al nuevo usuario creado.
            await _userManager.AddToRoleAsync(user, "ADMINISTRADOR");

            // Actualiza la variable "creado" para indicar si la creación fue exitosa.
            creado = result.Succeeded;
        }

        // BUSCAR EL USUARIO SI SE NECESITA
        var superusuario = _context.Users.Where(r => r.Email == "admin@workout.com").SingleOrDefault();
        if (superusuario != null)
        {
            var usuarioID = superusuario.Id;
        }

        // Devuelve un objeto JSON que indica si el usuario fue creado.
        return Json(creado);
    }



}



