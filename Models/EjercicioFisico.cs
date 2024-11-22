using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models;

public class EjercicioFisico
{
    [Key]
    public int EjercicioFisicoID { get; set; }
    public int TipoEjercicioID { get; set; }
    public int EventoDeportivoID { get; set; }
    public int LugarID { get; set; }
    public int? PersonaID { get; set; }

    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }

    [NotMapped]
    public TimeSpan IntervaloEjercicio { get { return Fin - Inicio; } }

    public EstadoEmocional EstadoEmocionalInicio { get; set; }
    public EstadoEmocional EstadoEmocionalFin { get; set; }
    public string? Observaciones { get; set; }

    public virtual TipoEjercicio TipoEjercicios { get; set; }
    public virtual Lugar Lugares { get; set; }
    public virtual EventoDeportivo EventosDeportivos { get; set; }
    public virtual Persona Personas { get; set; }


}

public class VistaEjercicioFisico
{
    public int EjercicioFisicoID { get; set; }
    public int TipoEjercicioID { get; set; }
    public int EventoDeportivoID { get; set; }
    public int LugarID { get; set; }
    public int PersonaID { get; set; }
    public Genero Genero { get; set; }
    public decimal Peso { get; set; }
    public string? TipoEjercicioDescripcion { get; set; }
    public decimal Met { get; set; }
    public string? LugarString { get; set; }
    public string? EventoDeportivoString { get; set; }
    public string? FechaInicioString { get; set; }
    public string? FechaFinString { get; set; }
    public decimal IntervaloEjercicio { get; set; }
    public string? EstadoEmocionalInicio { get; set; }
    public string? EstadoEmocionalFin { get; set; }
    public string? Observaciones { get; set; }
    public decimal CaloriasQuemadas { get; set; }
}




public enum EstadoEmocional
{
    Feliz = 1,
    Triste,
    Enojado,
    Ansioso,
    Estresado,
    Relajado,
    Aburrido,
    Emocionado,
    Agobiado,
    Confundido,
    Optimista,
    Pesimista,
    Motivado,
    Cansado,
    Eufórico,
    Agitado,
    Satisfecho,
    Desanimado
}



public class VistaEstadoEmocional
{
    public int EstadoEmocionalID { get; set; }
    public string? Descripcion { get; set; }
}


