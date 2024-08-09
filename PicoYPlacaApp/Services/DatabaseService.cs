using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PicoYPlacaApp.Models;

namespace PicoYPlacaApp.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void GuardarConsulta(Consulta consulta)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = "INSERT INTO PicoYPlacaConsultas (Placa, Fecha, Hora, Resultado) VALUES (@Placa, @Fecha, @Hora, @Resultado)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Placa", consulta.Placa);
                        command.Parameters.AddWithValue("@Fecha", consulta.Fecha);
                        command.Parameters.AddWithValue("@Hora", consulta.Hora);
                        command.Parameters.AddWithValue("@Resultado", consulta.Resultado);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la consulta: {ex.Message}");
                throw;
            }
        }

    }
}
