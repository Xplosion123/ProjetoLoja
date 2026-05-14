using MySql.Data.MySqlClient;
using ProjetoLogin.Models;
using ProjetoLogin.Interfaces;
using System.Data;

namespace ProjetoLogin.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly string _connectionString;

        public ProdutoRepositorio(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default");
        }

        //Método lista todos
        public IEnumerable<Produto> ListarTodos()
        {
            var lista = new List<Produto>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Produtos", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                });
            }
            return lista;
        }

        //Método lista por id
        public Produto? ObterPorId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Produtos WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nome = reader["Nome"].ToString()!,
                    Preco = Convert.ToDecimal(reader["Preco"])
                };
            }
            return null;
        }

        //Método cadastrar
        public void Adicionar(Produto p)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO Produtos (Nome, Preco) VALUES (@n, @p)", conn);
            cmd.Parameters.AddWithValue("@n", p.Nome);
            cmd.Parameters.AddWithValue("@p", p.Preco);
            cmd.ExecuteNonQuery();
        }

        //Métodoeditar
        public void Atualizar(Produto p)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("UPDATE Produtos SET Nome = @n, Preco = @p WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@n", p.Nome);
            cmd.Parameters.AddWithValue("@p", p.Preco);
            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.ExecuteNonQuery();
        }

        //Método excluir
        public void Excluir(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Produtos WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}