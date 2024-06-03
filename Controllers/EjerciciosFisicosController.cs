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
        var tipoEjercicios = _context.TipoEjercicios.ToList();
        tipoEjercicios.Add(new TipoEjercicio{TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]"});
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");
        
        
        var tipoEjerciciosFiltrar = _context.TipoEjercicios.ToList();
        tipoEjerciciosFiltrar.Add(new TipoEjercicio{TipoEjercicioID = 0, Descripcion = "[TODOS]"});
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

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var tipoEjercicio = tiposEjercicios.Where(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID).Single();

            var ejercicioFisicoMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
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

        return Json(ejerciciosFisicos.ToList());
    }
    


    public JsonResult ObtenerEstadoEmocionalEnum()
    {
        List<VistaEstadoEmocional> estadosEmocionalesMostrar= new List<VistaEstadoEmocional>();
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

    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string? observaciones)
    {
        string resultado = "";

        // Validar que todos los campos obligatorios estén presentes
        if (tipoEjercicioID == 0 || inicio == default || fin == default || estadoEmocionalInicio == 0 || estadoEmocionalFin == 0)
        {
            resultado = "Todos los campos son obligatorios";
        }
        else if (!FechasValidas(inicio, fin))
        {
            resultado = "La fecha de fin debe ser posterior a la fecha de inicio.";
        }
        else
        {
            if (ejercicioFisicoID != null)
            {
                // Verificar si el ejercicio físico es nuevo o existente
                if (ejercicioFisicoID == 0)
                {
                    // Crear un nuevo objeto "EjercicioFisico" y guardarlo en la base de datos
                    var ejercicioFisico = new EjercicioFisico
                    {
                        EjercicioFisicoID = ejercicioFisicoID,
                        TipoEjercicioID = tipoEjercicioID,
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
                    var ejercicioFisicoEditar = _context.EjerciciosFisicos.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
                    if (ejercicioFisicoEditar != null)
                    {
                        ejercicioFisicoEditar.TipoEjercicioID = tipoEjercicioID;
                        ejercicioFisicoEditar.Inicio = inicio;
                        ejercicioFisicoEditar.Fin = fin;
                        ejercicioFisicoEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                        ejercicioFisicoEditar.EstadoEmocionalFin = estadoEmocionalFin;
                        ejercicioFisicoEditar.Observaciones = observaciones;
                        _context.SaveChanges();
                        
                        resultado = "Ejercicio físico actualizado correctamente";
                    }
                }
            }
        }
        // Devolver el resultado de la operación en formato JSON
        return Json(resultado);
    }

    private bool FechasValidas(DateTime inicio, DateTime fin)
    {
        // Verificar que la fecha de fin sea posterior a la fecha de inicio
        return inicio < fin;
    }


    public JsonResult EliminarEjercicioFisico(int ejercicioFisicoID)
    {
        var ejercicioFisico = _context.EjerciciosFisicos.Find(ejercicioFisicoID);
        _context.Remove(ejercicioFisico);
        _context.SaveChanges();

        return Json(true);
    }
}