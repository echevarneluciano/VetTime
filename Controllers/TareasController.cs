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
            var property = typeof(TurnosPorTarea).GetProperty($"{dia.ToString().ToLower()}_ini");
            var property2 = typeof(TurnosPorTarea).GetProperty($"{dia.ToString().ToLower()}_fin");

            List<TurnosPorTarea> resultList = new List<TurnosPorTarea>();

            using (var command = contexto.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @$"SELECT {dia.ToString().ToLower()}_ini, {dia.ToString().ToLower()}_fin  FROM turnos  
            JOIN empleados_tareas  ON	turnos.empleadoId = empleados_tareas.empleadoid
            JOIN tareas  ON empleados_tareas.tareaid = tareas.id
            WHERE tareas.tarea = '{t.tarea}'";
                contexto.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        TurnosPorTarea item = new TurnosPorTarea();
                        TimeSpan timeSpan = ((TimeSpan)result[0]);
                        var valor = TimeOnly.FromDateTime(DateTime.Today.Add(timeSpan));
                        property.SetValue(item, valor);
                        TimeSpan timeSpan2 = ((TimeSpan)result[1]);
                        valor = TimeOnly.FromDateTime(DateTime.Today.Add(timeSpan2));
                        property2.SetValue(item, valor);

                        resultList.Add(item);
                    }
                }
            }


            return Ok(resultList);

            /*     List<Empleado> empleados = new List<Empleado>();
                List<Turno> turnos = new List<Turno>();

                var buscaTarea = contexto.Empleados_Tareas.Include(x => x.tarea).Where(x => x.tarea.tarea == t.tarea).Include(x => x.empleado).ToList();
                buscaTarea.ForEach(x => empleados.Add(x.empleado));

                var buscaTurnos = contexto.Turnos.Where(x => empleados.Contains(x.empleado)).ToList();

                var property = typeof(Turno).GetProperty($"{dia.ToString().ToLower()}_ini");
                var property2 = typeof(Turno).GetProperty($"{dia.ToString().ToLower()}_fin");
                var desde = new TimeOnly();
                var hasta = new TimeOnly();
                List<TimeSpan> listaHorarios = new List<TimeSpan>();

                for (int i = 0; i < buscaTurnos.Count; i++)
                {
                    if (property.GetValue(buscaTurnos[i]) != null)
                    {
                        desde = (TimeOnly)property.GetValue(buscaTurnos[i]);
                        hasta = (TimeOnly)property2.GetValue(buscaTurnos[i]);

                        TimeSpan intervalo = new TimeSpan(0, 30, 0); // Intervalo de 30 minutos

                        // Agregar intervalos de 30 minutos al HashSet
                        for (TimeOnly tiempo = desde; tiempo < hasta; tiempo = tiempo.Add(intervalo))
                        {
                            listaHorarios.Add(tiempo.ToTimeSpan());
                        }

                    }
                }

                var buscaConsultas = contexto.Consultas.Where(x => empleados.Contains(x.empleado)&&x.tiempoInicio.Value.Date == fechaConvertida.Date).ToList();

                List<TimeSpan> listaHorarios2 = new List<TimeSpan>();

                for (int i = 0; i < buscaConsultas.Count; i++)
                {
                    if (buscaConsultas[i].tiempoFin.Value != null)
                    {
                        desde = (TimeOnly)new TimeOnly(buscaConsultas[i].tiempoInicio.Value.Hour, buscaConsultas[i].tiempoInicio.Value.Minute, 0);
                        hasta = (TimeOnly)new TimeOnly(buscaConsultas[i].tiempoFin.Value.Hour, buscaConsultas[i].tiempoFin.Value.Minute, 0 );

                        TimeSpan intervalo = new TimeSpan(0, 30, 0); // Intervalo de 30 minutos

                        // Agregar intervalos de 30 minutos al HashSet
                        for (TimeOnly tiempo = desde; tiempo < hasta; tiempo = tiempo.Add(intervalo))
                        {
                            listaHorarios2.Add(tiempo.ToTimeSpan());
                        }

                    }
                }

                List<TimeSpan> listaHorarios3 = listaHorarios;

                for(int i = 0; i < listaHorarios2.Count; i++)
                {
                    if(listaHorarios.Contains(listaHorarios2[i]))
                    {
                        listaHorarios3.Remove(listaHorarios2[i]);
                    }
                } */


        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
