using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using puc_projeto_eixo_2.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace puc_projeto_eixo_2.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // ===========================================  I N D E X  =============================================================================================

        // Index - retorna todos os dados da tabela - privado para acesso dos admin com senha
        public IActionResult Index()
        {

            if (TempData["AcessoLiberado"] == null || (bool)TempData["AcessoLiberado"] != true)
            {
                return RedirectToAction("Senha");
            }

            var dados = _context.Usuarios.ToList();
            return View(dados);
        }


        // Action para exibir a página de senha
        [HttpGet]
        public IActionResult Senha()
        {
            return View();
        }

        // Action para validar a senha enviada pelo Post
        [HttpPost]
        public IActionResult VerificarSenha(string senhaDigitada)
        {
            string senhaCorreta = "12345"; // senha

            if (senhaDigitada == senhaCorreta)
            {
                TempData["AcessoLiberado"] = true;

                return RedirectToAction("Index");
            }

            ViewBag.Erro = "Senha inválida.";
            return View("Senha");

        }

        // ===========================================  C R E A T E  =============================================================================================

        // Create - Método get, exibe o formulário pro usuário
        public IActionResult Create()
        {
            return View();
        }

        // Create - Método post, recebe os dados enviados por um formulário e salva um novo usuário no banco de dados
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid) // Verifica se o modelo de dados é válido
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _context.Usuarios.Add(usuario); // Se o modelo for válido, ou seja, estiver com todos os dados preenchidos corretamente, será adicionado na tabela de usuários.

                await _context.SaveChangesAsync();

                // Autentica o usuário após o cadastro
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome)
            };

                var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identidade);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                //Console.WriteLine("Usuário autenticado com sucesso!");

                TempData["MensagemLogin"] = $"Bem-vindo, {usuario.Nome}!";

                return RedirectToAction("Index", "Home");
            }

            return View(usuario);
        }

        // ===========================================  E D I T  =============================================================================================

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // Atualiza apenas os campos permitidos (não altera a senha)
                usuarioExistente.Nome = usuario.Nome;
                usuarioExistente.NomeDeUsuario = usuario.NomeDeUsuario;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Telefone = usuario.Telefone;
                usuarioExistente.Genero = usuario.Genero;
                usuarioExistente.Estado = usuario.Estado;
                usuarioExistente.Cidade = usuario.Cidade;
                usuarioExistente.Nascimento = usuario.Nascimento;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(usuario);
        }

        // ===========================================  D E T A I L S  =============================================================================================

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // ===========================================  D E L E T E  =============================================================================================

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        // ===========================================  L O G I N  =============================================================================================

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            // Busca pelo e-mail no banco
            var dados = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == usuario.Email);

            if (dados == null || !BCrypt.Net.BCrypt.Verify(usuario.Senha, dados.Senha))
            {
                ViewBag.Message = "Usuário e/ou senha inválidos";
                return View(); // Volta para a mesma página com a mensagem
            }

            // Se senha está OK, cria os claims e loga o usuário
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, dados.Email),
                new Claim(ClaimTypes.Name, dados.Nome)
            };

            var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identidade);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            Console.WriteLine("Usuário autenticado com sucesso!");

            TempData["MensagemLogin"] = $"Bem-vindo, {dados.Nome}!";

            return RedirectToAction("TesteMensagem", "Home");
        }

        // ===========================================  L O G O U T  =============================================================================================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

            return RedirectToAction("Index", "Home");
        }

        // ===========================================  M É T O D O S   D E   T E S T E  =============================================================================================

        // MÉTODO TEMPORÁRIO - Criar usuário de teste para desenvolvimento
        [HttpGet]
        public async Task<IActionResult> CriarUsuarioTeste()
        {
            try
            {
                // Verifica se já existe um usuário com este email
                if (await _context.Usuarios.AnyAsync(u => u.Email == "teste@teste.com"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Usuário já existe",
                        email = "teste@teste.com",
                        senha = "123456",
                        loginUrl = "/Usuarios/Login"
                    });
                }

                // Cria o usuário de teste
                var usuario = new Usuario
                {
                    Nome = "João Silva",
                    NomeDeUsuario = "joao123",
                    Email = "teste@teste.com",
                    Telefone = "11999999999",
                    Genero = Genero.Masculino,
                    Estado = Estado.SãoPaulo,
                    Cidade = "São Paulo",
                    Nascimento = new DateTime(1990, 1, 1),
                    Senha = BCrypt.Net.BCrypt.HashPassword("123456"), // Senha será "123456"
                    Perfil = Perfil.User
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Usuário criado com sucesso!",
                    email = "teste@teste.com",
                    senha = "123456",
                    loginUrl = "/Usuarios/Login",
                    perfilUrl = "/Perfil"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Erro ao criar usuário: " + ex.Message
                });
            }
        }

        // MÉTODO TEMPORÁRIO - Criar vários usuários de teste
        [HttpGet]
        public async Task<IActionResult> CriarUsuariosTeste()
        {
            try
            {
                var usuarios = new List<Usuario>
                {
                    new Usuario
                    {
                        Nome = "João Silva",
                        NomeDeUsuario = "joao123",
                        Email = "joao@teste.com",
                        Telefone = "11999999999",
                        Genero = Genero.Masculino,
                        Estado = Estado.SãoPaulo,
                        Cidade = "São Paulo",
                        Nascimento = new DateTime(1990, 1, 1),
                        Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
                        Perfil = Perfil.User
                    },
                    new Usuario
                    {
                        Nome = "Maria Santos",
                        NomeDeUsuario = "maria456",
                        Email = "maria@teste.com",
                        Telefone = "11888888888",
                        Genero = Genero.Feminino,
                        Estado = Estado.RiodeJaneiro,
                        Cidade = "Rio de Janeiro",
                        Nascimento = new DateTime(1995, 5, 15),
                        Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
                        Perfil = Perfil.User
                    },
                    new Usuario
                    {
                        Nome = "Admin Teste",
                        NomeDeUsuario = "admin",
                        Email = "admin@teste.com",
                        Telefone = "11777777777",
                        Genero = Genero.Masculino,
                        Estado = Estado.SãoPaulo,
                        Cidade = "São Paulo",
                        Nascimento = new DateTime(1985, 3, 10),
                        Senha = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        Perfil = Perfil.Admin
                    }
                };

                foreach (var usuario in usuarios)
                {
                    if (!await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                    {
                        _context.Usuarios.Add(usuario);
                    }
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Usuários de teste criados com sucesso!",
                    usuarios = new[]
                    {
                        new { email = "joao@teste.com", senha = "123456", tipo = "User" },
                        new { email = "maria@teste.com", senha = "123456", tipo = "User" },
                        new { email = "admin@teste.com", senha = "admin123", tipo = "Admin" }
                    },
                    loginUrl = "/Usuarios/Login"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Erro ao criar usuários: " + ex.Message
                });
            }
        }
    }
}