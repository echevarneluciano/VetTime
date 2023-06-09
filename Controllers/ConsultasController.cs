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
            DateTime fin = (DateTime)c.tiempoFin;
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

    [HttpGet("turnos/{fecha}/{tarea}")]
    [AllowAnonymous]
    public async Task<IActionResult> turnosOcupados(string fecha, string tarea)
    {
        try
        {
            List<Consulta> Consultas = new List<Consulta>();
            using (var command = contexto.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @$"select c.id, c.tiempoinicio, c.tiempofin, c.cliente_mascotaid, c.estado, c.detalle
                from consultas c
                JOIN empleados_tareas et ON c.empleadoid = et.empleadoid	
                JOIN tareas t ON et.tareaid = t.id
                where DATE(tiempoInicio) = '{fecha}'
                AND	t.tarea = '{tarea}';";
                contexto.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        Consulta item = new Consulta();
                        item.id = result.GetInt32(0);
                        item.tiempoInicio = result.GetDateTime(1);
                        item.tiempoFin = result.GetDateTime(2);
                        item.cliente_mascotaId = result.GetInt32(3);
                        item.estado = result.GetInt32(4);
                        item.detalle = result.GetString(5);

                        Consultas.Add(item);
                    }
                }
            }

            return Ok(Consultas);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
