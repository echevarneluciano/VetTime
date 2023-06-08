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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> nuevaConsulta([FromBody] Consulta c)
    {
        try
        {
            return Ok(contexto.Consultas.Add(c));
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
