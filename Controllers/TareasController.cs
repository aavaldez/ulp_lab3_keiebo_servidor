using ulp_lab3_keiebo_servidor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ulp_lab3_keiebo_servidor.Controllers
{
	[Route("[Controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class TareasController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration configuracion;
		public TareasController(DataContext context, IConfiguration config)
		{
			contexto = context;
			configuracion = config;
		}

		// GET: Tareas/Obtener/{reunion_id}
		[HttpGet("Obtener/{reunion_id}")]
		[Authorize]
		public IActionResult obtenerPorReunion(int reunion_id)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);

				var usuario = User.Identity != null
					? contexto.Usuarios.Find(userId)
					: null;
				if (usuario == null) return NotFound();

				var reunion = contexto.Reuniones.Include(c => c.Inmueble).FirstOrDefault(c => c.Id == reunion_id);
				if (reunion == null) return NotFound();

				if (reunion.Inmueble.PropietarioId != usuario.Id) return Unauthorized();

				var pagos = contexto.Pagos.Where(p => p.ReunionId == reunion_id);

				return Ok(pagos);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// GET: Tareas/Obtener/{usuario_id}
		[HttpGet("Obtener/{usuario_id}")]
		[Authorize]
		public IActionResult obtenerPorUsuario(int usuario_id)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);

				var usuario = User.Identity != null
					? contexto.Usuarios.Find(userId)
					: null;
				if (usuario == null) return NotFound();

				var reunion = contexto.Reuniones.Include(c => c.Inmueble).FirstOrDefault(c => c.Id == usuario_id);
				if (reunion == null) return NotFound();

				if (reunion.Inmueble.PropietarioId != usuario.Id) return Unauthorized();

				var pagos = contexto.Pagos.Where(p => p.ReunionId == reunion_id);

				return Ok(pagos);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// PUT: Tareas/Cambiar_Estado/{id}
		[HttpPut("Cambiar_Estado")]
		[Authorize]
		public IActionResult PutEstado([FromBody] Tarea t)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);
				var usuario = User.Identity != null
					? contexto.Propietarios.Find(userId)
					: null;

				if (usuario == null) return NotFound();

				var tarea = contexto.Tareas.Find(t.Id);

				if (tarea == null) return NotFound();

				if (tarea.UsuarioId != usuario.Id) return Forbid();

				tarea.Estado = !i.Estado;
				contexto.Update(tarea);
				contexto.SaveChanges();

				return Ok(tarea);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

	}
}