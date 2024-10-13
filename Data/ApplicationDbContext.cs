using Proyecto1_2024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1_2024.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoEjercicio> TipoEjercicios { get; set; }
    public DbSet<EjercicioFisico> EjerciciosFisicos { get; set; }
    public DbSet<Lugar> Lugares { get; set; }
    public DbSet<EventoDeportivo> EventosDeportivos { get; set; }   
}
