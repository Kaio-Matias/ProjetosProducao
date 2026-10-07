// Arquivo: Models/Usuario.cs
using System.Text.Json.Serialization;

namespace Valedourado.Models
{
    /// <summary>
    /// Representa o modelo de dados de um usuário, correspondendo à API.
    /// </summary>
    public class Usuario
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("cargo")]
        public string? Cargo { get; set; }

        [JsonPropertyName("matricula")]
        public int? Matricula { get; set; }
    }

    /// <summary>
    /// Data Transfer Object (DTO) para a requisição de login.
    /// </summary>
    public class LoginRequest
    {
        public int? Matricula { get; set; }
    }
}
