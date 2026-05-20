using MySql.Data.MySqlClient;
using ProjetoLogin.Models;

namespace ProjetoLogin.Repositorio
{
    public class LoginRepositorio : ILoginRepositorio
    {
        private readonly string _connectionString;

        public LoginRepositorio(IConfiguration config) =>
            _connectionString = config.GetConnectionString("Conexao");

        public Login? Validar(string email, string senha)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM Usuarios WHERE Email = @e AND Senha = @s";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@s", senha);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Login
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    Nivel = reader["Nivel"].ToString()!
                };
            }
            return null;
        }

    }
}
