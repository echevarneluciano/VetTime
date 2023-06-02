using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetTime.Models;
[Table("Turnos")]
public class Turno
{
    public int id { get; set; }
    public TimeOnly? lunes_ini { get; set; }
    public TimeOnly? lunes_fin { get; set; }
    public TimeOnly? martes_ini { get; set; }
    public TimeOnly? martes_fin { get; set; }
    public TimeOnly? miercoles_ini { get; set; }
    public TimeOnly? miercoles_fin { get; set; }
    public TimeOnly? jueves_ini { get; set; }
    public TimeOnly? jueves_fin { get; set; }
    public TimeOnly? viernes_ini { get; set; }
    public TimeOnly? viernes_fin { get; set; }
    public TimeOnly? sabado_ini { get; set; }
    public TimeOnly? sabado_fin { get; set; }
    public TimeOnly? domingo_ini { get; set; }
    public TimeOnly? domingo_fin { get; set; }
    [ForeignKey(nameof(empleadoId))]
    public int? empleadoId { get; set; }
    public int? activo { get; set; }
    public Empleado? empleado { get; set; }
    public Turno()
    {

    }

    public Turno(int id, TimeOnly lunes_ini, TimeOnly lunes_fin, TimeOnly martes_ini, TimeOnly martes_fin,
    TimeOnly miercoles_ini, TimeOnly miercoles_fin, TimeOnly jueves_ini, TimeOnly jueves_fin,
    TimeOnly viernes_ini, TimeOnly viernes_fin, TimeOnly sabado_ini, TimeOnly sabado_fin,
    TimeOnly domingo_ini, TimeOnly domingo_fin, int? activo, int? empleadoId, Empleado? empleado)
    {
        this.id = id;
        this.lunes_ini = lunes_ini;
        this.lunes_fin = lunes_fin;
        this.martes_ini = martes_ini;
        this.martes_fin = martes_fin;
        this.miercoles_ini = miercoles_ini;
        this.miercoles_fin = miercoles_fin;
        this.jueves_ini = jueves_ini;
        this.jueves_fin = jueves_fin;
        this.viernes_ini = viernes_ini;
        this.viernes_fin = viernes_fin;
        this.sabado_ini = sabado_ini;
        this.sabado_fin = sabado_fin;
        this.domingo_ini = domingo_ini;
        this.domingo_fin = domingo_fin;
        this.activo = activo;
        this.empleadoId = empleadoId;
        this.empleado = empleado;
    }

}