using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto1_2024.Controllers;

[Authorize]
public class LugaresController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public LugaresController(ApplicationDbContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoLugares(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE LUGARES
        var lugares = _context.Lugares.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN LUGAR EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE LUGARES POR EL LUGAR QUE COINCIDA CON ESE ID
            lugares = lugares.Where(t => t.LugarID == id).ToList();
        }

        return Json(lugares);
    }

    public JsonResult GuardarLugares(int lugarID, string nombre)
    {
        string resultado = "";
        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        if (!String.IsNullOrEmpty(nombre))
        {
            nombre = nombre.ToUpper();
            if (lugarID == 0)
            {
                var existeLugar = _context.Lugares.Where(t => t.Nombre == nombre).Count();
                if (existeLugar == 0)
                {
                    var lugar = new Lugar
                    {
                        Nombre = nombre
                    };
                    _context.Add(lugar);
                    _context.SaveChanges();
                    resultado = "Lugar guardado correctamente";
                }
                else
                {
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var lugarEditar = _context.Lugares.Where(t => t.LugarID == lugarID).SingleOrDefault();
                if (lugarEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO
                    //AL QUE ESTAMOS EDITANDO
                    var existeLugar = _context.Lugares.Where(t => t.Nombre == nombre && t.LugarID != lugarID).Count();
                    if (existeLugar == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO Y ES CORRECTO, ENTONCES CONTINUAMOS CON EL EDITAR
                        lugarEditar.Nombre = nombre;
                        _context.SaveChanges();
                    }
                    else
                    {
                        resultado = "YA EXISTE UN REGISTRO CON LA MISMO NOMBRE";
                    }
                }
            }
        }
        else
        {
            resultado = "DEBE INGRESAR UNA NOMBRE";
        }
        return Json(resultado);
    }

    public JsonResult EliminarLugar(int lugarID)
    {
        // Verificar si existen ejercicios físicos asociados al lugar
        bool tieneEjerciciosAsociados = _context.EjerciciosFisicos.Any(e => e.LugarID == lugarID);

        if (tieneEjerciciosAsociados)
        {
            return Json(new { exito = false, mensaje = "No se puede eliminar el lugar porque tiene ejercicios físicos asociados." });
        }

        // Si no hay ejercicios asociados, proceder con la eliminación
        var lugar = _context.Lugares.Find(lugarID);
        if (lugar != null)
        {
            _context.Remove(lugar);
            _context.SaveChanges();
        }

        return Json(new { exito = true });
    }

}