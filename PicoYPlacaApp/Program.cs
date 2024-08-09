using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PicoYPlacaApp.Services;
using PicoYPlacaApp.Models;

namespace PicoYPlacaApp
{
    class Program
    {
        private static IConfiguration _configuration;
        private static DatabaseService _databaseService;
        private static PicoPlacaService _picoPlacaService;

        static void Main(string[] args)
        {
            ConfigureServices();
            EjecutarPrograma();
        }

        private static void ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .AddTransient<DatabaseService>()
                .AddTransient<PicoPlacaService>()
                .BuildServiceProvider();

            _configuration = serviceProvider.GetService<IConfiguration>();
            _databaseService = serviceProvider.GetService<DatabaseService>();
            _picoPlacaService = serviceProvider.GetService<PicoPlacaService>();
        }

        private static void EjecutarPrograma()
        {
            try
            {
                string placa = SolicitarEntrada("Ingrese la placa del vehículo: ");
                DateTime fecha = SolicitarFecha();
                TimeSpan hora = SolicitarHora();

                var resultado = _picoPlacaService.PuedeCircular(placa, fecha, hora);
                GuardarConsulta(placa, fecha, hora, resultado);

                MostrarResultado(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
        }

        private static string SolicitarEntrada(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine();
        }

        private static DateTime SolicitarFecha()
        {
            while (true)
            {
                Console.Write("Ingrese la fecha (formato: yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                {
                    return fecha;
                }
                Console.WriteLine("Formato de fecha inválido. Por favor, use el formato yyyy-MM-dd.");
            }
        }

        private static TimeSpan SolicitarHora()
        {
            while (true)
            {
                Console.Write("Ingrese la hora (formato: HH:mm:ss): ");
                if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan hora))
                {
                    return hora;
                }
                Console.WriteLine("Formato de hora inválido. Por favor, use el formato HH:mm:ss.");
            }
        }

        private static void GuardarConsulta(string placa, DateTime fecha, TimeSpan hora, bool resultado)
        {
            var consulta = new Consulta
            {
                Placa = placa,
                Fecha = fecha,
                Hora = hora,
                Resultado = resultado ? "Permitido" : "No Permitido"
            };

            try
            {
                _databaseService.GuardarConsulta(consulta);
                Console.WriteLine("Consulta guardada exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la consulta: {ex.Message}");
            }
        }

        private static void MostrarResultado(bool resultado)
        {
            Console.WriteLine(resultado ? "El vehículo puede circular." : "El vehículo no puede circular.");
        }
    }
}
