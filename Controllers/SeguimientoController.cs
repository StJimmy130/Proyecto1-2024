using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto1_2024.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Proyecto1_2024.Migrations;

namespace Proyecto1_2024.Controllers;

[Authorize (Roles = "Deportista")]
public class SeguimientoController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public SeguimientoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int PersonaID)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;

        var tipoEjercicios = _context.TipoEjercicios.ToList();
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }

    public JsonResult GraficoTipoEjercicioMes(int TipoEjercicioID, int PersonaID, int Mes, int Anio)
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
        && e.Inicio.Month == Mes && e.Inicio.Year == Anio && e.PersonaID == PersonaID).ToList();


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


    public JsonResult GraficoTortaTipoEjerciciosPorMes(int Mes, int Anio, int PersonaID)
    {
        var vistaTipoEjercicioFisico = new List<VistaTipoEjercicioFisico>();

        var tiposEjerciciosFisicos = _context.TipoEjercicios.ToList();
        
        foreach (var tipoEjercicioFisico in tiposEjerciciosFisicos)
        {
            var ejercicios = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID
            && e.Inicio.Month == Mes && e.Inicio.Year == Anio && e.PersonaID == PersonaID).ToList();

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

        return Json(vistaTipoEjercicioFisico);
    }






    public IActionResult Informe(int PersonaID)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;
        return View();
    }

    public JsonResult ListadoInforme(int? id, int PersonaID, DateTime? BuscarInicioActividad, DateTime? BuscarFinActividad)
    {
        List<VistaEjercicioFisico> ejerciciosFisicosMostrar = new List<VistaEjercicioFisico>();


        var ejerciciosFisicos = _context.EjerciciosFisicos.Where(e => e.Personas.PersonaID == PersonaID).AsQueryable();
        var personas = _context.Personas.ToList();


        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id);
        }


        if (BuscarInicioActividad != null && BuscarFinActividad != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.Inicio >= BuscarInicioActividad && e.Inicio <= BuscarFinActividad);
        }

        ejerciciosFisicos = ejerciciosFisicos.OrderBy(e => e.Inicio);


        var tiposEjercicios = _context.TipoEjercicios.ToList();

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var tipoEjercicio = tiposEjercicios.Single(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID);
            var persona = personas.Where(t => t.PersonaID == ejercicioFisico.PersonaID).Single();

            var calQuemadas = tipoEjercicio.MET * persona.Peso * (decimal)ejercicioFisico.IntervaloEjercicio.TotalHours;

            var ejercicioFisicosMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
                FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy HH:mm"),
                FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy HH:mm"),
                IntervaloEjercicio = Convert.ToDecimal(ejercicioFisico.IntervaloEjercicio.TotalMinutes),
                CaloriasQuemadas = tipoEjercicio.MET * persona.Peso * Convert.ToDecimal(ejercicioFisico.IntervaloEjercicio.TotalMinutes) / 60,
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), ejercicioFisico.EstadoEmocionalFin),
                Observaciones = ejercicioFisico.Observaciones
            };
            ejerciciosFisicosMostrar.Add(ejercicioFisicosMostrar);
        }
        return Json(ejerciciosFisicosMostrar);
    }

    public IActionResult InformeLugar(int PersonaID)
    {   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;

        return View();
    }


    public JsonResult ListadoInformePorLugar(int? id, int PersonaID, DateTime? BuscarInicioActividad, DateTime? BuscarFinActividad)
    {

        List<VistaEjercicioFisico> ejerciciosFisicosPorLugarMostrar = new List<VistaEjercicioFisico>();


        var ejerciciosFisicos = _context.EjerciciosFisicos.Where(e => e.Personas.PersonaID == PersonaID).AsQueryable();
        var personas = _context.Personas.ToList();


        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id);
        }


        if (BuscarInicioActividad != null && BuscarFinActividad != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.Inicio >= BuscarInicioActividad && e.Inicio <= BuscarFinActividad);
        }

        ejerciciosFisicos = ejerciciosFisicos.OrderBy(e => e.Inicio);
        ejerciciosFisicos = ejerciciosFisicos.OrderBy(e => e.LugarID);


        var lugares = _context.Lugares.ToList();
        var tiposEjercicios = _context.TipoEjercicios.ToList();

        foreach (var ejercicioFisico in ejerciciosFisicos)
        {
            var lugar = lugares.Where(t => t.LugarID == ejercicioFisico.LugarID).Single();
            var tipoEjercicio = tiposEjercicios.Where(t => t.TipoEjercicioID == ejercicioFisico.TipoEjercicioID).Single();
            var persona = personas.Where(t => t.PersonaID == ejercicioFisico.PersonaID).Single();

            var calQuemadas = tipoEjercicio.MET * persona.Peso * (decimal)ejercicioFisico.IntervaloEjercicio.TotalHours;

            var ejercicioFisicoPorLugarMostrar = new VistaEjercicioFisico
            {
                EjercicioFisicoID = ejercicioFisico.EjercicioFisicoID,
                TipoEjercicioID = ejercicioFisico.TipoEjercicioID,
                LugarID = ejercicioFisico.LugarID,
                LugarString = lugar.Nombre,
                FechaInicioString = ejercicioFisico.Inicio.ToString("dd/MM/yyyy HH:mm"),
                FechaFinString = ejercicioFisico.Fin.ToString("dd/MM/yyyy HH:mm"),
                IntervaloEjercicio = Convert.ToDecimal(ejercicioFisico.IntervaloEjercicio.TotalMinutes),
                CaloriasQuemadas = tipoEjercicio.MET * persona.Peso * Convert.ToDecimal(ejercicioFisico.IntervaloEjercicio.TotalMinutes) / 60,
                TipoEjercicioDescripcion = tipoEjercicio.Descripcion,
                Observaciones = ejercicioFisico.Observaciones
            };
            ejerciciosFisicosPorLugarMostrar.Add(ejercicioFisicoPorLugarMostrar);
        }
        ejerciciosFisicosPorLugarMostrar = ejerciciosFisicosPorLugarMostrar.ToList();

        return Json(ejerciciosFisicosPorLugarMostrar);
    }



    public IActionResult Informe4Niveles(int PersonaID)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        PersonaID = _context.Personas.Where(t => t.CuentaID == userId).Select(t => t.PersonaID).SingleOrDefault();
        ViewBag.PersonaID = PersonaID;

        return View();
    }

    public JsonResult Informe4Nivel(int PersonaID)
    {
        List<VistaEvento> vistaEventos = new List<VistaEvento>();

        var ejercicioFisicos = _context.EjerciciosFisicos
            .Include(t => t.Lugares)
            .Include(t => t.TipoEjercicios)
            .Include(t => t.EventosDeportivos)
            .Where(t => t.Personas.PersonaID == PersonaID)
            .ToList();
            var personas = _context.Personas.ToList();

            

        foreach (var listadoEventos in ejercicioFisicos)
        {
            var EventosMostrar = vistaEventos
                .Where(t => t.EventoDeportivoID == listadoEventos.EventoDeportivoID)
                .SingleOrDefault();
            
            

            

            if (EventosMostrar == null)
            {
                EventosMostrar = new VistaEvento
                {
                    EventoDeportivoID = listadoEventos.EventoDeportivoID,
                    Descripcion = listadoEventos.EventosDeportivos.Descripcion,
                    VistaLugares = new List<VistaLugar>()
                };
                vistaEventos.Add(EventosMostrar);
            }

            var lugaresMostrar = EventosMostrar.VistaLugares
                .Where(e => e.LugarID == listadoEventos.Lugares.LugarID)
                .SingleOrDefault();

            if (lugaresMostrar == null)
            {
                lugaresMostrar = new VistaLugar
                {
                    LugarID = listadoEventos.Lugares.LugarID,
                    Nombre = listadoEventos.Lugares.Nombre,
                    VistaTiposEjercicios = new List<VistaTipoEjercicio>()
                };
                EventosMostrar.VistaLugares.Add(lugaresMostrar);
            }

            var tipoEjercicioMostrar = lugaresMostrar.VistaTiposEjercicios
                .Where(l => l.TipoEjercicioID == listadoEventos.TipoEjercicioID)
                .SingleOrDefault();

            if (tipoEjercicioMostrar == null)
            {
                tipoEjercicioMostrar = new VistaTipoEjercicio
                {
                    TipoEjercicioID = listadoEventos.TipoEjercicioID,
                    Descripcion = listadoEventos.TipoEjercicios.Descripcion,
                    VistaEjerciciosFisicos = new List<VistaEjercicioFisico>()
                };
                lugaresMostrar.VistaTiposEjercicios.Add(tipoEjercicioMostrar);
            }

            tipoEjercicioMostrar.VistaEjerciciosFisicos.Add(new VistaEjercicioFisico
            {
                EjercicioFisicoID = listadoEventos.EjercicioFisicoID,
                FechaInicioString = listadoEventos.Inicio.ToString("dd/MM/yyyy HH:mm"),
                FechaFinString = listadoEventos.Fin.ToString("dd/MM/yyyy HH:mm"),
                EstadoEmocionalInicio = Enum.GetName(typeof(EstadoEmocional), listadoEventos.EstadoEmocionalInicio),
                EstadoEmocionalFin = Enum.GetName(typeof(EstadoEmocional), listadoEventos.EstadoEmocionalFin),
                IntervaloEjercicio = Convert.ToDecimal(listadoEventos.IntervaloEjercicio.TotalMinutes),
                Observaciones = listadoEventos.Observaciones,
            });
        }

        return Json(vistaEventos);
    }



}