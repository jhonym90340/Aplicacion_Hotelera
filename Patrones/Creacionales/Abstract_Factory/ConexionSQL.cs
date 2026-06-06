using System;
using Newtonsoft.Json;
using pPatronesDiseñoHotel.Patrones.Creacionales.Builder;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class ConexionSQL : IAbstractFactoryBaseDatos
    {
        // Propiedad requerida por la interfaz para almacenar la consulta
        public string SQL { get; set; } = string.Empty;

        public string Insertar()
        {
            // Cambiamos el color de la consola a cian para identificar SQL Server en la simulación
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[Fábrica Concreta - SQL Server Engine]");
            Console.WriteLine($"Ejecutando Comando: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro insertado correctamente en las tablas de SQL Server.";
        }

        public string Consultar()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[Fábrica Concreta - SQL Server Engine]");
            Console.WriteLine($"Ejecutando Consulta: {SQL}");
            Console.ResetColor();

            // =========================================================================
            // LECTURA DINÁMICA DE MEMORIA:
            // Evaluamos si la lista compartida de reservas no tiene elementos creados
            // =========================================================================
            if (MemoriaHotel.ListaReservas.Count == 0)
            {
                return "[\n  {\n    \"Mensaje\": \"No hay reservas registradas en las tablas de SQL Server aún.\"\n  }\n]";
            }

            // =========================================================================
       
            // Convierte de forma automática la lista completa de reservas acumuladas
            // a formato JSON estructurado con sangrías elegantes.
            // =========================================================================
            return JsonConvert.SerializeObject(MemoriaHotel.ListaReservas, Formatting.Indented);
        }

        public string Actualizar()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[Fábrica Concreta - SQL Server Engine]");
            Console.WriteLine($"Ejecutando Sentencia de Actualización: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro actualizado en SQL Server de manera polimórfica.";
        }

        public string Eliminar()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[Fábrica Concreta - SQL Server Engine]");
            Console.WriteLine($"Ejecutando Sentencia de Eliminación: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro eliminado de SQL Server de manera polimórfica.";
        }
    }
}