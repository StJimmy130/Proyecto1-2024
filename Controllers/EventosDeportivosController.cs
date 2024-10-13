using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1_2024.Controllers;


public class EventosDeportivosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EventosDeportivosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoEventosDeportivos(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE LUGARES
        var eventosDeportivos = _context.EventosDeportivos.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN LUGAR EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE LUGARES POR EL LUGAR QUE COINCIDA CON ESE ID
            eventosDeportivos = eventosDeportivos.Where(t => t.EventoDeportivoID == id).ToList();
        }

        return Json(eventosDeportivos);
    }

    public JsonResult GuardarEventoDeportivo(int eventoDeportivoID, string descripcion)
    {
        string resultado = "";
        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        if (!String.IsNullOrEmpty(descripcion))
        {
            descripcion = descripcion.ToUpper();
            if (eventoDeportivoID == 0)
            {
                var existeEventoDeportivo = _context.EventosDeportivos.Where(t => t.Descripcion == descripcion).Count();
                if (existeEventoDeportivo == 0)
                {
                    var eventoDeportivo = new EventoDeportivo
                    {
                        Descripcion = descripcion
                    };
                    _context.Add(eventoDeportivo);
                    _context.SaveChanges();
                    resultado = "Evento deportivo guardado correctamente";
                }
                else
                {
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var eventoDeportivoEditar = _context.EventosDeportivos.Where(t => t.EventoDeportivoID == eventoDeportivoID).SingleOrDefault();
                if (eventoDeportivoEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO
                    //AL QUE ESTAMOS EDITANDO
                    var existeEventoDeportivo = _context.EventosDeportivos.Where(t => t.Descripcion == descripcion && t.EventoDeportivoID != eventoDeportivoID).Count();
                    if (existeEventoDeportivo == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO Y ES CORRECTO, ENTONCES CONTINUAMOS CON EL EDITAR
                        eventoDeportivoEditar.Descripcion = descripcion;
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
            resultado = "DEBE INGRESAR UNA DESCRIPCIÓN";
        }
        return Json(resultado);
    }

    public JsonResult EliminarEventoDeportivo(int eventoDeportivoID)
{
    var eventoDeportivo = _context.EventosDeportivos
                                  .Include(e => e.EjerciciosFisicos)
                                  .FirstOrDefault(e => e.EventoDeportivoID == eventoDeportivoID);

    if (eventoDeportivo == null)
        return Json(new { eliminado = false, mensaje = "El evento deportivo no fue encontrado." });

    if (eventoDeportivo.EjerciciosFisicos.Count != 0)
    {
        return Json(new { eliminado = false, mensaje = "El evento deportivo está relacionado con un ejercicio físico y no puede desactivarse." });
    }

    // Cambiar el estado de 'Eliminado' al contrario de su estado actual
    eventoDeportivo.Eliminado = !eventoDeportivo.Eliminado;
    _context.SaveChanges();

    // Devolver un mensaje basado en el nuevo estado de 'Eliminado'
    var mensaje = eventoDeportivo.Eliminado ? "El evento deportivo ha sido desactivado." : "El evento deportivo ha sido activado.";
    return Json(new { eliminado = eventoDeportivo.Eliminado, mensaje });
}


}