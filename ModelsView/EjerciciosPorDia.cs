using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Proyecto1_2024.Models.ViewModels
{
    public class EjerciciosPorDia
    {
        public int? Dia { get; set; }
        public string? Mes { get; set; }
        public int CantidadMinutos { get; set; }
    }

    public class VistaTipoEjercicioFisico
    {
        public int TipoEjercicioID { get; set; }
        public string? Descripcion { get; set; }
        public decimal CantidadMinutos { get; set; }
    }
}