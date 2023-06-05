using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetTime.Models;
using Newtonsoft.Json;

namespace VetTime.Controllers;


[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class TareasController : Controller
{
    private readonly DataContext contexto;

    public TareasController(DataContext contexto)
    {
        this.contexto = contexto;
    }

    // GET: api/<controller>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(contexto.Tareas.ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("turnos/{fecha}")]
    [AllowAnonymous]
    public async Task<IActionResult> turnosPorTarea([FromBody] Tarea t, string fecha)
    {
        try
        {
            var fechaConvertida = DateTime.Parse(fecha);
            var dia = fechaConvertida.DayOfWeek;

            List<Empleado> empleados = new List<Empleado>();
            List<Turno> turnos = new List<Turno>();
            HashSet<TimeSpan> listaHorarios = new HashSet<TimeSpan>();
            var buscaTarea = contexto.Empleados_Tareas.Include(x => x.tarea).Where(x => x.tarea.tarea == t.tarea).Include(x => x.empleado).ToList();
            buscaTarea.ForEach(x => empleados.Add(x.empleado));

            var buscaTurnos = contexto.Turnos.Where(x => empleados.Contains(x.empleado)).ToList();

            var property = typeof(Turno).GetProperty($"{dia.ToString().ToLower()}_ini");
            var property2 = typeof(Turno).GetProperty($"{dia.ToString().ToLower()}_fin");
            var desde = new TimeOnly();
            var hasta = new TimeOnly();

            for (int i = 0; i < buscaTurnos.Count; i++)
            {
                if (property.GetValue(buscaTurnos[i]) != null)
                {
                    desde = (TimeOnly)property.GetValue(buscaTurnos[i]);
                    listaHorarios.Add(
                        new TimeSpan(desde.Hour, desde.Minute, 0)
                    );
                    hasta = (TimeOnly)property2.GetValue(buscaTurnos[i]);
                    listaHorarios.Add(
                        new TimeSpan(hasta.Hour, hasta.Minute, 0)
                    );
                }
            }






            return Ok(listaHorarios);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
