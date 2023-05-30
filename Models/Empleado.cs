using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetTime.Models;
[Table("Empleados")]
public class Empleado
{
    public int Id { get; set; }
    public String? Nombre { get; set; }
    public String? Apellido { get; set; }
    public String? Mail { get; set; }
    public String? Pass { get; set; }
    public String? Telefono { get; set; }
    public int? Activo { get; set; }

    [ForeignKey(nameof(RolesId))]
    public int? RolesId { get; set; }
    [ForeignKey(nameof(EspecialidadesId))]
    public int? EspecialidadesId { get; set; }
    [ForeignKey(nameof(SucursalesId))]
    public int? SucursalesId { get; set; }
    public Roles? Roles { get; set; }
    public Especialidades? Especialidades { get; set; }
    public Sucursales? Sucursales { get; set; }

    public Empleado()
    {

    }
    public Empleado(int id, string? nombre, string? apellido, string? mail, string? pass, string? telefono, int? activo, int? rolesId, int? especialidadesId, int? sucursalesId)
    {
        Id = id;
        Nombre = nombre;
        Apellido = apellido;
        Mail = mail;
        Pass = pass;
        Telefono = telefono;
        Activo = activo;
        RolesId = rolesId;
        EspecialidadesId = especialidadesId;
        SucursalesId = sucursalesId;
    }
}