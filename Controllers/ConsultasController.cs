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

    [HttpPost("{empleado}")]
    [AllowAnonymous]
    public async Task<IActionResult> nuevaConsulta([FromBody] Consulta c, string empleado)
    {
        try
        {
            string[] partes = empleado.Split(' ');
            string nombre = partes[0];
            string apellido = partes[1];
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
                FROM empleados e
                WHERE e.nombre = '{nombre}' AND e.apellido = '{apellido}';
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

    [HttpGet("turnos/{fecha}/{empleado}")]
    [AllowAnonymous]
    public async Task<IActionResult> turnosOcupados(string fecha, string empleado)
    {
        try
        {
            string[] partes = empleado.Split(' ');
            string nombre = partes[0];
            string apellido = partes[1];
            List<Consulta> Consultas = new List<Consulta>();
            using (var command = contexto.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @$"select c.id, c.tiempoinicio, c.tiempofin, c.cliente_mascotaid, c.estado, c.detalle
                from consultas c
                JOIN empleados e ON e.id = c.empleadoid
                where DATE(tiempoInicio) = '{fecha}'
                AND	e.nombre = '{nombre}' AND e.apellido = '{apellido}';";
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
