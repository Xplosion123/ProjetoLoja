using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjetoLogin.Models;
using ProjetoLogin.Repositorio;
using System.Security.Claims;

namespace ProjetoLogin.Controllers
{
    public class UsuarioController : Controller
    {
        //Cria o acesso ao banco de dados de usuários.
        private readonly ILoginRepositorio _usuarioRepo;

        //O construtor que prepara a ferramenta de banco de dados.
        public UsuarioController(ILoginRepositorio usuarioRepo)
        {
            //Guarda essa ferramenta na variável que criamos acima para ser usada depois.
            _usuarioRepo = usuarioRepo;
        }

        //Abre a tela de login (o formulário) quando você acessa o site.
        [HttpGet]
        public IActionResult Logar() => View(); // Abre a tela de login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logar(Login model) // Alterado para LoginViewModel
        {
            // Verifica se os campos Email e Senha foram preenchidos conforme as regras da ViewModel
            if (!ModelState.IsValid) return View(model);

            //Pergunta ao banco de dados: "Existe alguém com esse e-mail e senha?".
            var usuario = _usuarioRepo.Validar(model.Email, model.Senha);

            //Se o banco de dados encontrar o usuário, inicia a criação do "crachá" de acesso.
            if (usuario != null)
            {
                //As "Claims" são os dados que vão no crachá (Nome, E-mail, Nível de Acesso e ID).
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("NivelAcesso", usuario.Nivel),
            new Claim("UsuarioId", usuario.Id.ToString())
        };
                //Cria a "identidade" oficial do usuário baseada nas informações acima.
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //Comando que efetivamente "loga" o usuário, criando um Cookie de segurança no navegador.
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    //IsPersistent = false: Indica que a sessão acaba quando o navegador for fechado.
                    new AuthenticationProperties { IsPersistent = false }); // Agora o model tem essa propriedade

                return RedirectToAction("Index", "Home");
            }
            //Cria uma mensagem de erro para exibir na tela caso a senha esteja errada.
            ModelState.AddModelError("", "E-mail ou senha inválidos.");
            //Recarrega a página de login mostrando o erro.
            return View(model);
        }

        //Função para o usuário deslogar do sistema.
        public async Task<IActionResult> Sair()
        {
            //Remove o "crachá" (deleta o Cookie) do navegador do usuário
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Manda o usuário de volta para a tela de login.
            return RedirectToAction("Logar");
        }
    }
}
