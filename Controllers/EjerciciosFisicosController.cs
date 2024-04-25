using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto1_2024.Controllers;

//[Authorize]
public class EjerciciosFisicosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EjerciciosFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoEjerciciosFisicos(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjerciciosFisicos.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO FISICO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.EjercicioFisicoID == id).ToList();
        }

        return Json(ejerciciosFisicos);
    }

    public JsonResult GuardarEjerciciosFisicos(int EjercicioFisicoID, int TipoEjercicioID, DateTime Inicio, DateTime Fin, EstadoEmocional EstadoEmocionalInicio, EstadoEmocional EstadoEmocionalFin, string? Observaciones, )
    {
        string resultado = "";
        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
        var tipoEjercicioEditar = _context.TipoEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();
        if (tipoEjercicioEditar != null)
        {
            //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO
            //AL QUE ESTAMOS EDITANDO
            var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion && t.TipoEjercicioID != tipoEjercicioID).Count();
            if (existeTipoEjercicio == 0)
            {
                //QUIERE DECIR QUE EL ELEMENTO Y ES CORRECTO, ENTONCES CONTINUAMOS CON EL EDITAR
                tipoEjercicioEditar.Descripcion = descripcion;
                _context.SaveChanges();
            }
            else
            {
                resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
            }
        }
        return Json(resultado);
    }

    public JsonResult EliminarTipoEjercicio(int tipoEjercicioID)
    {
        var tipoEjercicio = _context.TipoEjercicios.Find(tipoEjercicioID);
        _context.Remove(tipoEjercicio);
        _context.SaveChanges();

        return Json(true);
    }
}