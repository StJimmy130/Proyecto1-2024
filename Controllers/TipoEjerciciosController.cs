using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto1_2024.Models;
using Proyecto1_2024.Data;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto1_2024.Controllers;

//[Authorize]
public class TipoEjerciciosController : Controller
{
    private ApplicationDbContext _context;
    
    //CONSTRUCTOR
    public TipoEjerciciosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoTipoEjercicios(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var tipoDeEjercicios = _context.TipoEjercicios.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
        //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercicioID == id).ToList();    
        }

        return Json(tipoDeEjercicios);
    }

    public JsonResult GuardarTipoEjercicio(int tipoEjercicioID ,string descripcion)
    {
        string resultado = "";
        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        if (!String.IsNullOrEmpty(descripcion))
        {
            descripcion = descripcion.ToUpper(); 
            if (tipoEjercicioID == 0)
            {
                var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion).Count();
                if (existeTipoEjercicio == 0)
                {
                    var tipoEjercicio = new TipoEjercicio
                    {
                        Descripcion = descripcion
                    };
                    _context.Add(tipoEjercicio);
                    _context.SaveChanges();
                }
                else{
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var tipoEjercicioEditar = _context.TipoEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();
                if(tipoEjercicioEditar != null){
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO
                    //AL QUE ESTAMOS EDITANDO
                    var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion && t.TipoEjercicioID != tipoEjercicioID).Count();
                    if(existeTipoEjercicio == 0){
                        //QUIERE DECIR QUE EL ELEMENTO Y ES CORRECTO, ENTONCES CONTINUAMOS CON EL EDITAR
                        tipoEjercicioEditar.Descripcion = descripcion;
                        _context.SaveChanges();
                    }
                    else{
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                    }
                }
            }
        }  
        else{
            resultado = "DEBE INGRESAR UNA DESCRIPCIÓN";
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