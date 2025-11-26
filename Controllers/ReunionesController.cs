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
	public class ReunionesController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration configuracion;
		public ReunionesController(DataContext context, IConfiguration config)
		{
			contexto = context;
			configuracion = config;
		}

		// GET: Reuniones/
		[HttpGet("Todos")]
		[Authorize]
		public IActionResult GetTodos()
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);
				var usuario = User.Identity != null
					? contexto.Propietarios.Find(userId)
					: null;

				if (usuario == null) return NotFound();

				return Ok(contexto.Reuniones.Include(i => i.Propietario).Where(e => e.Propietario.Id == usuario.Id));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// GET: Reuniones/Obtener/{usuario_id}
		[HttpGet("Obtener/{usuario_id}")]
		[Authorize]
		public IActionResult ObtenerPorUsuario(int usuario_id)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);

				var usuario = User.Identity != null
					? contexto.Propietarios.Find(userId)
					: null;

				if (usuario == null) return NotFound();

				var reunion = contexto.Reuniones
					.FirstOrDefault(i => i.InmuebleId == inmueble_id);

				if (reunion == null) return NotFound();

				Console.WriteLine(usuario_id);
				Console.WriteLine(reunion);

				return Ok(reunion);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		// POST: Reuniones/Crear
		[HttpPost("Crear")]
		[Authorize]
		public async Task<IActionResult> PostCrear([FromForm] Reunion reunion)
		{
			try
			{
				int.TryParse(User.FindFirstValue("Id"), out int userId);
				var usuario = User.Identity != null
					? contexto.Propietarios.Find(userId)
					: null;

				if (usuario == null)
					return NotFound();

				reunion.PropietarioId = usuario.Id;
				reunion.Propietario = usuario;
				reunion.Estado = false;
				contexto.Reuniones.Add(reunion);

				contexto.SaveChanges();
				if (reunion.ImagenFileName != null && reunion.Id > 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					string fileName = "imagen_" + reunion.Id + Path.GetExtension(reunion.ImagenFileName.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					reunion.Imagen = Path.Combine("/Uploads", fileName);
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						reunion.ImagenFileName.CopyTo(stream);
					}
					contexto.Update(reunion);
					contexto.SaveChanges();
				}

				return Ok(reunion);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}