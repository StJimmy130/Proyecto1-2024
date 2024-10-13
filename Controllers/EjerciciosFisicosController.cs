using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto1_2024.Controllers;

[Authorize]
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

        var tipoEjercicios = _context.TipoEjercicios.Where(te => !te.Eliminado).ToList();
        tipoEjercicios.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");


        var lugares = _context.Lugares.ToList();
        lugares.Add(new Lugar { LugarID = 0, Nombre = "[SELECCIONE...]" });
        ViewBag.lugarID = new SelectList(lugares.OrderBy(c => c.Nombre), "LugarID", "Nombre");



        var eventosDeportivos = _context.EventosDeportivos.Where(ed => !ed.Eliminado).ToList();
        eventosDeportivos.Add(new EventoDeportivo { EventoDeportivoID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.EventoDeportivoID = new SelectList(eventosDeportivos.OrderBy(c => c.Descripcion), "EventoDeportivoID", "Descripcion");

        var tipoEjerciciosFiltrar = _context.TipoEjercicios.ToList();
        tipoEjerciciosFiltrar.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[TODOS]" });
        ViewBag.TipoEjercicioFiltrarID = new SelectList(tipoEjerciciosFiltrar.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");



        return View();
    }



    public JsonResult ListadoTipoEjerciciosFisicos(int? id, int tipoEjercicioFiltrarID, DateTime? fechaDesde, DateTime? fechaHasta)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();

        var ejerciciosFisicos = _context.EjerciciosFisicos.ToList();

        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id).ToList();
        }

        if (fechaDesde != null && fechaHasta != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= fechaDesde && t.Inicio <= fechaHasta).ToList();
        }
        if (tipoEjercicioFiltrarID != 0)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.TipoEjercicioID == tipoEjercicioFiltrarID).ToList();
        }

        var tiposEjercicios = _context.TipoEjercicios.ToList();
        var lugares = _context.Lugares.ToList();
        var eventosDeportivos = _context.EventosDeportivos.ToList();

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var tipoEjercicio = tiposEjercicios.Where(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID).Single();
            var lugar = lugares.Where(t => t.LugarID == ejercicioFisico.LugarID).Single();
            var eventoDeportivo = eventosDeportivos.Where(t => t.EventoDeportivoID == ejercicioFisico.EventoDeportivoID).Single();

            var ejercicioFisicoMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                LugarID = ejercicioFisico.LugarID,
                EventoDeportivoID = ejercicioFisico.EventoDeportivoID,
                TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
                LugarString = lugar.Nombre,
                EventoDeportivoString = eventoDeportivo.Descripcion,
                FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy HH:mm"),
                FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy HH:mm"),
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalFin),
                Observaciones = ejercicioFisico.Observaciones
            };
            ejerciciosFisicosMostrar.Add(ejercicioFisicoMostrar);
        }

        return Json(ejerciciosFisicosMostrar);
    }


    public JsonResult EjerciciosFisicos(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var ejerciciosFisicos = _context.EjerciciosFisicos.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.EjercicioFisicoID == id).ToList();
        }

        return Json(ejerciciosFisicos);
    }




    public JsonResult ObtenerEstadoEmocionalEnum()
    {
        List<VistaEstadoEmocional> estadosEmocionalesMostrar = new List<VistaEstadoEmocional>();
        var estadoEmocional = Enum.GetNames(typeof(EstadoEmocional)).ToList();
        for (int i = 0; i < estadoEmocional.Count; i++)
        {
            var estadoEmocionalMostrar = new VistaEstadoEmocional
            {
                EstadoEmocionalID = i + 1,
                Descripcion = estadoEmocional[i]
            };
            estadosEmocionalesMostrar.Add(estadoEmocionalMostrar);
        }

        return Json(estadosEmocionalesMostrar);
    }

    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, int lugarID, int eventoDeportivoID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string? observaciones)
    {
        string resultado = "";


        if (ejercicioFisicoID == 0)
        {
            var ejercicioFisico = new EjercicioFisico
            {
                TipoEjercicioID = tipoEjercicioID,
                LugarID = lugarID,
                EventoDeportivoID = eventoDeportivoID,
                Inicio = inicio,
                Fin = fin,
                EstadoEmocionalInicio = estadoEmocionalInicio,
                EstadoEmocionalFin = estadoEmocionalFin,
                Observaciones = observaciones
            };
            _context.Add(ejercicioFisico);
            _context.SaveChanges();

            resultado = "Ejercicio físico guardado correctamente";
        }
        else
        {
            // Obtener un ejercicio físico existente en la base de datos y editarlo
            var ejercicioFisicoEditar = _context.EjerciciosFisicos.SingleOrDefault(e => e.EjercicioFisicoID == ejercicioFisicoID);
            if (ejercicioFisicoEditar != null)
            {
                ejercicioFisicoEditar.TipoEjercicioID = tipoEjercicioID;
                ejercicioFisicoEditar.LugarID = lugarID;
                ejercicioFisicoEditar.EventoDeportivoID = eventoDeportivoID;
                ejercicioFisicoEditar.Inicio = inicio;
                ejercicioFisicoEditar.Fin = fin;
                ejercicioFisicoEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                ejercicioFisicoEditar.EstadoEmocionalFin = estadoEmocionalFin;
                ejercicioFisicoEditar.Observaciones = observaciones;
                _context.SaveChanges();

                resultado = "Ejercicio físico actualizado correctamente";
            }
            else
            {
                resultado = "Ejercicio físico no encontrado";
            }
        }
        // Devolver el resultado de la operación en formato JSON
        return Json(resultado);
    }




    public JsonResult EliminarEjercicioFisico(int ejercicioFisicoID)
    {
        var ejercicioFisico = _context.EjerciciosFisicos.Find(ejercicioFisicoID);
        _context.Remove(ejercicioFisico);
        _context.SaveChanges();

        return Json(true);
    }
}