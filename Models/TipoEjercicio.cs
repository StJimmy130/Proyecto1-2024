using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models;

public class TipoEjercicio
{
    [Key]
    public int TipoEjercicioID { get; set; }
    public string? Descripcion { get; set; }
    public bool Eliminado { get; set; }
    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; }
}