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
public class EmpleadosTareasController : Controller
{
    private readonly DataContext contexto;

    public EmpleadosTareasController(DataContext contexto)
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
            return Ok(contexto.Empleados_Tareas.Include(c => c.empleado).Include(c => c.tarea).ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
