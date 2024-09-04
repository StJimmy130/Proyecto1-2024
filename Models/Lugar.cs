using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models;

public class Lugar
{
    [Key]
    public int LugarID { get; set; }
    public string? Nombre { get; set; }
    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; }
}