using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puc_projeto_eixo_2.Models;
using System.Security.Claims;

namespace puc_projeto_eixo_2.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Perfil - Mostra o perfil do usuário logado
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new PerfilViewModel
            {
                Usuario = usuario,
                IsProprietario = true,
                TreinosCriados = usuario.Treinos?.ToList() ?? new List<Treino>()
            };

            return View(viewModel);
        }

        // GET: Perfil/Ver/5 - Mostra o perfil de outro usuário
        public async Task<IActionResult> Ver(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Treinos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var usuarioLogado = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            var viewModel = new PerfilViewModel
            {
                Usuario = usuario,
                IsProprietario = usuarioLogado?.Id == usuario.Id,
                TreinosCriados = usuario.Treinos?.ToList() ?? new List<Treino>()
            };

            return View("Index", viewModel);
        }

        // GET: Perfil/Editar
        public async Task<IActionResult> Editar()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new EditarPerfilViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                NomeDeUsuario = usuario.NomeDeUsuario,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Genero = usuario.Genero,
                Estado = usuario.Estado,
                Cidade = usuario.Cidade,
                Nascimento = usuario.Nascimento,
                FotoPerfil = usuario.FotoPerfil
            };

            return View(viewModel);
        }

        // POST: Perfil/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarPerfilViewModel model, IFormFile? fotoUpload)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

                if (usuario == null)
                {
                    return NotFound();
                }

                // Atualiza os dados do usuário
                usuario.Nome = model.Nome;
                usuario.NomeDeUsuario = model.NomeDeUsuario;
                usuario.Email = model.Email;
                usuario.Telefone = model.Telefone;
                usuario.Genero = model.Genero;
                usuario.Estado = model.Estado;
                usuario.Cidade = model.Cidade;
                usuario.Nascimento = model.Nascimento;

                // Processa upload da foto se houver
                if (fotoUpload != null && fotoUpload.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "perfis");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{usuario.Id}_{Guid.NewGuid()}{Path.GetExtension(fotoUpload.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await fotoUpload.CopyToAsync(stream);
                    }

                    usuario.FotoPerfil = $"/img/perfis/{fileName}";
                }

                // Atualiza senha se fornecida
                if (!string.IsNullOrEmpty(model.NovaSenha))
                {
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(model.NovaSenha);
                }

                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = "Perfil atualizado com sucesso!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}