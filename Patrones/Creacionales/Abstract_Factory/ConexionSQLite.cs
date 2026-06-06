using System;

using System;
using Newtonsoft.Json;
using pPatronesDiseñoHotel.Patrones.Creacionales.Builder;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class ConexionSQLite : IAbstractFactoryBaseDatos
    {
        // Propiedad requerida por la interfaz para almacenar la consulta
        public string SQL { get; set; } = string.Empty;

        public string Insertar()
        {
            // Cambiamos el color de la consola a verde para identificar SQLite en la simulación
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Fábrica Concreta - SQLite Embedded Engine]");
            Console.WriteLine($"Ejecutando Comando en Archivo Local: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro insertado correctamente en el archivo de base de datos SQLite (.db3).";
        }

        public string Consultar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Fábrica Concreta - SQLite Embedded Engine]");
            Console.WriteLine($"Ejecutando Consulta en Archivo Local: {SQL}");
            Console.ResetColor();

            // =========================================================================
            // LECTURA DINÁMICA DE MEMORIA:
            // Evaluamos si la lista compartida de reservas no tiene elementos creados
            // =========================================================================
            if (MemoriaHotel.ListaReservas.Count == 0)
            {
                return "[\n  {\n    \"Mensaje\": \"No hay reservas registradas en la memoria de SQLite aún.\"\n  }\n]";
            }

            // =========================================================================
          
            // Convierte de forma automática la lista completa de reservas acumuladas
            // a formato JSON estructurado con sangrías elegantes.
            // =========================================================================
            return JsonConvert.SerializeObject(MemoriaHotel.ListaReservas, Formatting.Indented);
        }

        public string Actualizar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Fábrica Concreta - SQLite Embedded Engine]");
            Console.WriteLine($"Ejecutando Sentencia de Actualización: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro modificado en archivo SQLite de manera polimórfica.";
        }

        public string Eliminar()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[Fábrica Concreta - SQLite Embedded Engine]");
            Console.WriteLine($"Ejecutando Sentencia de Eliminación: {SQL}");
            Console.ResetColor();

            return "Éxito: Registro removido de archivo SQLite de manera polimórfica.";
        }
    }
}