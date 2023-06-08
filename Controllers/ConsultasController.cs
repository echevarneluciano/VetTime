using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetTime.Models;

namespace VetTime.Controllers;


[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ConsultasController : Controller
{
    private readonly DataContext contexto;

    public ConsultasController(DataContext contexto)
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
            return Ok(contexto.Consultas.Include(x => x.empleado)
            .Include(x => x.cliente_mascota.cliente)
            .Include(x => x.cliente_mascota.mascota)
            .ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{tarea}")]
    [AllowAnonymous]
    public async Task<IActionResult> nuevaConsulta([FromBody] Consulta c, string tarea)
    {
        try
        {
            DateTime inicio = (DateTime)c.tiempoInicio;
            var inicioC = inicio.ToString("yyyy/MM/dd HH:mm:ss");
            DateTime fin = (DateTime)c.tiempoInicio;
            var finC = fin.ToString("yyyy/MM/dd HH:mm:ss");

            Consulta item = new Consulta();
            using (var command = contexto.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @$"INSERT INTO consultas 
                (empleadoid, tiempoinicio, tiempofin, cliente_mascotaid, estado, detalle)
                SELECT e.id, '{inicioC}', '{finC}', {c.cliente_mascotaId}, 1, '{c.detalle}'
                FROM empleados e JOIN empleados_tareas et ON e.id = et.empleadoId
                JOIN tareas t ON t.id = et.tareaid
                WHERE NOT EXISTS (
                SELECT 1
                FROM consultas c
                WHERE c.empleadoid = e.id
                AND c.tiempoinicio = '{inicioC}')
                AND t.tarea = '{tarea}'
                ORDER BY RAND()
                LIMIT 1;
                SELECT LAST_INSERT_ID();";
                contexto.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        item.id = result.GetInt32(0);
                    }
                }
            }
            var consultaCreada = contexto.Consultas.Find(item.id);
            return Ok(consultaCreada);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("turnos/{fecha}")]
    [AllowAnonymous]
    public async Task<IActionResult> turnosOcupados(string fecha)
    {
        try
        {
            DateTime fechaFormateada;
            DateTime.TryParse(fecha, out fechaFormateada);
            // DateTime fechaSql = DateTime.ParseExact(fecha, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // string fechaFormateada = fechaSql.ToString("yyyy-MM-dd");
            var Consultas = contexto.Consultas.FromSqlInterpolated(@$"select * from consultas where 
            DATE(tiempoInicio) = {fechaFormateada}").ToList();
            return Ok(Consultas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
