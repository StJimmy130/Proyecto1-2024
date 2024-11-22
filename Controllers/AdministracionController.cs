using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1_2024.Controllers;

[Authorize (Roles = "ADMINISTRADOR")]
public class AdministracionController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public AdministracionController(ApplicationDbContext context)
    {
        _context = context;
    }



    public IActionResult InformePersonas()
    {
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[TODOS]" }
        };

        var enumValues = Enum.GetValues(typeof(Genero)).Cast<Genero>();
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));
        ViewBag.Generos = selectListItems.OrderBy(t => t.Text).ToList();

        return View();
    }



    public JsonResult ObtenerPersonasConRoles(Genero genero)
    {
        var personasConRoles = (from persona in _context.Personas
                                join user in _context.Users on persona.CuentaID equals user.Id
                                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                join role in _context.Roles on userRole.RoleId equals role.Id
                                where genero == 0 || persona.Genero == (Genero)genero
                                orderby persona.FechaNacimiento
                                select new
                                {
                                    persona.PersonaID,
                                    user.Email,
                                    persona.NombreCompleto,
                                    persona.FechaNacimiento,
                                    persona.Genero,
                                    persona.Peso,
                                    persona.Altura,
                                    persona.CuentaID,
                                    role.Name
                                }).ToList();

        var resultadoFinal = personasConRoles.Select(p => new PersonaConRolViewModel
        {
            PersonaID = p.PersonaID,
            Email = p.Email,
            NombreCompleto = p.NombreCompleto,
            FechaNacimiento = p.FechaNacimiento.ToString("dd/MM/yyyy"),
            Genero = Enum.GetName(typeof(Genero), p.Genero),
            Peso = p.Peso,
            Altura = p.Altura,
            CuentaID = p.CuentaID,
            Rol = p.Name
        }).ToList();

        return Json(resultadoFinal);
    }


}