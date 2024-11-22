using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models;

public class TipoEjercicio
{
    [Key]
    public int TipoEjercicioID { get; set; }
    public string? Descripcion { get; set; }
    public bool Eliminado { get; set; }
    public decimal MET { get; set; }
    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; }
    
}

public class VistaTipoEjercicio
{
    public int TipoEjercicioID { get; set; }
    public string? Descripcion { get; set; }
    public string? MET { get; set; }
    public List<VistaEjercicioFisico> VistaEjerciciosFisicos { get; set; }
}