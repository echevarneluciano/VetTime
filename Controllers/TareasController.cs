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
            DateTime fechaConvertida = DateTime.Parse(fecha);
            var dia = fechaConvertida.DayOfWeek;
            var property = typeof(TurnosPorTarea).GetProperty($"{dia.ToString().ToLower()}_ini");
            var property2 = typeof(TurnosPorTarea).GetProperty($"{dia.ToString().ToLower()}_fin");

            List<TurnosPorTarea> resultList = new List<TurnosPorTarea>();
            Console.WriteLine(dia + " " + fecha);
            using (var command = contexto.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @$"SELECT {dia.ToString().ToLower()}_ini, {dia.ToString().ToLower()}_fin, 
            empleados_tareas.empleadoId, tareas.tiempo  
            FROM turnos  JOIN empleados_tareas  ON	turnos.empleadoId = empleados_tareas.empleadoid
            JOIN tareas  ON empleados_tareas.tareaid = tareas.id
            WHERE tareas.tarea = '{t.tarea}'";
                contexto.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        TurnosPorTarea item = new TurnosPorTarea();
                        var valor = new TimeOnly();
                        if (!result.IsDBNull(0))
                        {
                            TimeSpan timeSpan = ((TimeSpan)result[0]);
                            valor = TimeOnly.FromDateTime(DateTime.Today.Add(timeSpan));
                            property.SetValue(item, valor);

                            item.empleadoId = result.GetInt32(2);
                            TimeSpan timeSpan3 = ((TimeSpan)result[3]);
                            item.tiempoTarea = TimeOnly.FromDateTime(DateTime.Today.Add(timeSpan3));
                        }
                        if (!result.IsDBNull(1))
                        {
                            TimeSpan timeSpan2 = ((TimeSpan)result[1]);
                            valor = TimeOnly.FromDateTime(DateTime.Today.Add(timeSpan2));
                            property2.SetValue(item, valor);
                        }

                        resultList.Add(item);
                    }
                }
            }


            return Ok(resultList);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
