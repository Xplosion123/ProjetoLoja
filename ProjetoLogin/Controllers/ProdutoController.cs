using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoLogin.Models;
using ProjetoLogin.Repositorio;

namespace ProjetoLogin.Controllers
{
    public class ProdutoController : Controller
    {
        // Use apenas uma variável privada para evitar confusão
        private readonly IProdutoRepositorio _produtoRepositorio;

        // O construtor recebe a interface injetada pelo Program.cs
        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        // Listagem: Agora usando a variável correta que foi instanciada
        public IActionResult Index()
        {
            // O Controller busca os dados e envia para Views/Produto/Index.cshtml
            var produtos = _produtoRepositorio.ListarTodos();
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Criar() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Produto vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Mapeamento manual: ViewModel -> Entidade
            var produto = new Produto
            {
                Nome = vm.Nome,
                Preco = vm.Preco
            };

            _produtoRepositorio.Adicionar(produto);

            return RedirectToAction(nameof(Index));
        }


        //Método Editar Post que pega
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var produto = _produtoRepositorio.ObterPorId(id);
            if (produto == null) return NotFound();

            // Mapeamento: Entidade -> ViewModel
            var viewModel = new Produto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco
            };

            return View(viewModel);
        }

        //Método Editar Post que envia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Produto model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var produto = new Produto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Preco = model.Preco
                };


                _produtoRepositorio.Atualizar(produto);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            _produtoRepositorio.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}