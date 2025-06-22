using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using puc_projeto_eixo_2.Models;

namespace puc_projeto_eixo_2.Controllers
{
    [Authorize] // Garante que apenas usuários logados possam acessar estas actions
    public class TreinosController : Controller
    {
        private readonly AppDbContext _context;

        public TreinosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Treinos
        public async Task<IActionResult> Index()
        {
            var AppDbContext = _context.Treino.Include(t => t.Usuario);
            return View(await AppDbContext.ToListAsync());
        }

        // GET: Treinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treino
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treino == null)
            {
                return NotFound();
            }

            return View(treino);
        }

        // GET: Treinos/Create
        public IActionResult Create()
        {
            // Removemos a linha que envia a lista de usuários para a view.
            return View();
        }

        // POST: Treinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,Avaliacao")] Treino treino)
        {
            // Obtém o e-mail do usuário logado a partir do Claim
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError(string.Empty, "Não foi possível identificar o usuário logado.");
                return View(treino);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                return View(treino);
            }

            // Associa o ID do usuário logado ao treino
            treino.UsuarioId = usuario.Id;

            if (ModelState.IsValid)
            {
                _context.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            // Se o modelo for inválido, retorna para a view sem a necessidade de recarregar a lista de usuários
            return View(treino);
        }

        // GET: Treinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treino.FindAsync(id);
            if (treino == null)
            {
                return NotFound();
            }
            // Mantemos isso caso um admin precise editar e trocar o usuário.
            // Se apenas o próprio usuário pode editar, esta lógica também deve ser alterada.
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cidade", treino.UsuarioId);
            return View(treino);
        }

        // POST: Treinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Avaliacao,UsuarioId")] Treino treino)
        {
            if (id != treino.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreinoExists(treino.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cidade", treino.UsuarioId);
            return View(treino);
        }

        // GET: Treinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treino
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treino == null)
            {
                return NotFound();
            }

            return View(treino);
        }

        // POST: Treinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treino = await _context.Treino.FindAsync(id);
            if (treino != null)
            {
                _context.Treino.Remove(treino);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinoExists(int id)
        {
            return _context.Treino.Any(e => e.Id == id);
        }
    }
}
