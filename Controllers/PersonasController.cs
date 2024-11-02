using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto1_2024.Controllers;

[Authorize]
public class PersonasController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public PersonasController(ApplicationDbContext context)
    {
        _context = context;
    }


    public IActionResult Index(int? PersonaID)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;

        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
        };

        var generos = Enum.GetValues(typeof(Genero)).Cast<Genero>();

        selectListItems.AddRange(generos.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));
        ViewBag.Generos = selectListItems.ToList();

        return View();
    }

    
    public IActionResult ObtenerDatosUsuario(int? PersonaID)
    {
        var persona = _context.Personas.Where(t => t.PersonaID == PersonaID).SingleOrDefault();

        return Json(persona);
    }


    public JsonResult GuardarDatosUsuario(int? PersonaID, string? NombreCompleto, DateOnly FechaNacimiento, Genero Genero, decimal Peso, decimal Altura)
    {
        string resultado = "";
        if(NombreCompleto != null)
        {
            var Persona = _context.Personas.Find(PersonaID);
            if(Persona != null)
            {
                Persona.NombreCompleto = NombreCompleto;
                Persona.FechaNacimiento = FechaNacimiento;
                Persona.Genero = Genero;
                Persona.Peso = Peso;
                Persona.Altura = Altura;
                _context.SaveChanges();
                
                
                resultado = "Datos guardadas con éxito";
            }
            else
            {
                resultado = "Usuario no encontrado";
            }
        }
        else
        {
            resultado = "El nombre es obligatorio";
        }


        return Json(resultado);
    }
}