using Microsoft.EntityFrameworkCore;

namespace VetTime.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<Cliente_mascota> Cliente_Mascotas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Empleado_tarea> Empleado_Tareas { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Mascota> Mascotas { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<Turno> Turnos { get; set; }

}