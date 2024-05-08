using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;

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

    

    public JsonResult ListadoTipoEjerciciosFisicos(int? id)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();
        
        var ejerciciosFisicos = _context.EjerciciosFisicos.ToList();

        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.TipoEjercicioID == id).ToList();    
        }

        var tiposEjercicios = _context.TipoEjercicios.ToList();

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var TipoEjercicio = tiposEjercicios.Where(t => t.TipoEjercicioID == ejercicioFisico.EjercicioFisicoID).Single();

            var ejercicioFisicoMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                TipoEjercicioDescripcion = TipoEjercicio.Descripcion,
                FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy HH:mm"),
                FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy HH:mm"),
                Observaciones = ejercicioFisico.Observaciones
            };
            ejerciciosFisicosMostrar.Add(ejercicioFisicoMostrar);
        }

        return Json(ejerciciosFisicosMostrar);
    }
    


    public JsonResult ObtenerEstadoEmocionalEnum()
    {
        var estadoEmocional = Enum.GetNames(typeof(EstadoEmocional)).ToList();
        return Json(estadoEmocional);
    }

    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string? observaciones)
    {
        string resultado = "";

        // Verificar si se han proporcionado todos los datos necesarios para guardar el ejercicio físico
        if (inicio != DateTime.MinValue && fin != DateTime.MinValue && estadoEmocionalInicio != 0 && estadoEmocionalFin != 0)
        {
            // Verificar si el ejercicio físico es nuevo o existente
            if (ejercicioFisicoID == 0)
            {
                // Crear un nuevo objeto "EjercicioFisico" y guardarlo en la base de datos
                var ejercicioFisico = new EjercicioFisico
                {
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
        else
        {
            // Indicar que todos los datos deben ser ingresados
            resultado = "Debe ingresar todos los datos";
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