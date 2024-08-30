using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto1_2024.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1_2024.Controllers;

public class SeguimientoController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public SeguimientoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tipoEjercicios = _context.TipoEjercicios.ToList();
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }

    public JsonResult GraficoTipoEjercicioMes (int TipoEjercicioID, int Mes, int Anio)
    {
        List<EjerciciosPorDia> ejerciciosPorDias = new List<EjerciciosPorDia>();

        var diasDelMes = DateTime.DaysInMonth(Anio, Mes);

        //INICIALIZO UNA VARIABLE DE TIPO FECHA
        DateTime fechaMes = new DateTime();
        //RESTAMOS UN MES SOBRE ESA FECHA
        fechaMes = fechaMes.AddMonths(Mes - 1);

        for (int i = 1; i <= diasDelMes; i++)
        {
            var diaMesMostrar = new EjerciciosPorDia
            {
                Dia = i,
                Mes = fechaMes.ToString("MMM").ToUpper(),
                CantidadMinutos = 0
            };
            ejerciciosPorDias.Add(diaMesMostrar);
        }

        var ejercicios = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == TipoEjercicioID 
        && e.Inicio.Month == Mes && e.Inicio.Year == Anio).ToList();

        
        foreach (var ejercicio in ejercicios.OrderBy(e => e.Inicio))
        {
            //POR CADA EJERCICIO DEBEMOS AGREGAR EN EL LISTADO SI EL DIA DE ESE EJERCICIO NO EXISTE, SINO SUMARLO
            var ejercicioDiaMostrar = ejerciciosPorDias.Where(e => e.Dia == ejercicio.Inicio.Day).SingleOrDefault();
            if (ejercicioDiaMostrar != null)
            {
                ejercicioDiaMostrar.CantidadMinutos += Convert.ToInt32(ejercicio.IntervaloEjercicio.TotalMinutes);
            }
        }
        return Json(ejerciciosPorDias);
    }


    public JsonResult GraficoTortaTipoEjerciciosPorMes (int Mes, int Anio)
    {
        var vistaTipoEjercicioFisico = new List<VistaTipoEjercicioFisico>();

        var tiposEjerciciosFisicos = _context.TipoEjercicios.ToList();
        
        foreach (var tipoEjercicioFisico in tiposEjerciciosFisicos)
        {
            var ejercicios = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID 
            && e.Inicio.Month == Mes && e.Inicio.Year == Anio).ToList();

            foreach (var ejercicio in ejercicios)
            {
                var tipoEjercicioFisicoMostrar = vistaTipoEjercicioFisico.Where(e => e.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID).SingleOrDefault();
                if (tipoEjercicioFisicoMostrar == null)
                {
                    tipoEjercicioFisicoMostrar = new VistaTipoEjercicioFisico
                    {
                        TipoEjercicioID = tipoEjercicioFisico.TipoEjercicioID,
                        Descripcion = tipoEjercicioFisico.Descripcion,
                        CantidadMinutos = Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes)
                    };
                    vistaTipoEjercicioFisico.Add(tipoEjercicioFisicoMostrar);
                }
                else
                {
                    tipoEjercicioFisicoMostrar.CantidadMinutos += Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes);
                }
            }
        }

        return Json (vistaTipoEjercicioFisico);
    }






    public IActionResult Informe()
    {
        


        return View();
    }

public JsonResult ListadoInforme(int? id, DateTime? BuscarInicioActividad, DateTime? BuscarFinActividad)
{
    List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();

    // Obtener la lista de ejercicios físicos
    var ejerciciosFisicos = _context.EjerciciosFisicos.AsQueryable();

    // Aplicar filtro por ID si se proporciona
    if (id != null)
    {
        ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id);
    }

    // Aplicar filtro por rango de fechas si se proporcionan
    if (BuscarInicioActividad != null && BuscarFinActividad != null)
    {
        ejerciciosFisicos = ejerciciosFisicos.Where(e => e.Inicio >= BuscarInicioActividad && e.Inicio <= BuscarFinActividad);
    }

    // Ordenar los ejercicios por la fecha de inicio
    ejerciciosFisicos = ejerciciosFisicos.OrderBy(e => e.Inicio);

    // Obtener la lista de tipos de ejercicios
    var tiposEjercicios = _context.TipoEjercicios.ToList();

    // Construir la lista de VistaEjercicioFisico para mostrar
    foreach (var ejercicioFisico in ejerciciosFisicos)
    {
        var tipoEjercicio = tiposEjercicios.Single(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID);

        var ejercicioFisicosMostrar = new VistaEjercicioFisico
        {
            EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
            TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
            TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
            FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy HH:mm"),
            FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy HH:mm"),
            IntervaloEjercicio = Convert.ToDecimal(ejercicioFisico.IntervaloEjercicio.TotalMinutes),
            EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalInicio),
            EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalFin),
            Observaciones = ejercicioFisico.Observaciones
        };
        ejerciciosFisicosMostrar.Add(ejercicioFisicosMostrar);
    }

    // Retornar los resultados en formato JSON
    return Json(ejerciciosFisicosMostrar);
}


}