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

    public JsonResult GuardarEjerciciosFisicos(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, DateTime fin, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, string? observaciones)
    {
        string resultado = "";
        if(inicio != DateTime.MinValue && fin != DateTime.MinValue && !String.IsNullOrEmpty(observaciones))
        {
            if (ejercicioFisicoID == 0)
            {
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
            }
            
            else
            {
                var ejercicioFisicoEditar = _context.EjerciciosFisicos.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
                if(ejercicioFisicoEditar != null)
                {
                    var existeEjercicioFisico = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == tipoEjercicioID).Count();
                    if(existeEjercicioFisico == 0)
                    {
                        ejercicioFisicoEditar.Inicio = inicio;
                        ejercicioFisicoEditar.Fin = fin;
                        ejercicioFisicoEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                        ejercicioFisicoEditar.EstadoEmocionalFin = estadoEmocionalFin;
                        ejercicioFisicoEditar.Observaciones = observaciones;
                        _context.SaveChanges();
                    }
                }
            }
        }
        else
        {
            resultado = "DEBE INGRESAR TODOS LOS DATOS";
        }
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