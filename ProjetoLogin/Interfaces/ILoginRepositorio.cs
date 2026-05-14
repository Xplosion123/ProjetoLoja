using ProjetoLogin.Models;

namespace ProjetoLogin.Repositorio
{
    public interface ILoginRepositorio
    {
        Login? Validar(string email, string senha);
    }
}
