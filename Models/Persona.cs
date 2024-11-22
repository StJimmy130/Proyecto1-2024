using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models;

public class Persona
{
    [Key]
    public int PersonaID { get; set; }
    public string NombreCompleto { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public Genero Genero { get; set; }
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public string? CuentaID { get; set; }



    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; }
    public virtual ICollection<Lugar> Lugares { get; set; }

}

public enum Genero
{
    Hombre = 1,
    Mujer,
    Otro
}

public class PersonaConRolViewModel
{
    public int PersonaID { get; set; }
    public string? NombreCompleto { get; set; }
    public string FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public decimal Peso { get; set; }
    public decimal Altura { get; set; }
    public string? CuentaID { get; set; }
    public string? Rol { get; set; } // Para almacenar el rol del usuario
    public string? Email { get; set; }
}