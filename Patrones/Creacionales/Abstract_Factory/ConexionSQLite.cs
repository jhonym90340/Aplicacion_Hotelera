using System;

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

            // Retorna un formato JSON simulado tal como lo espera el broker
            return "[{\"Mensaje\": \"Datos de auditoría cargados desde SQLite con éxito\"}]";
        }

        public string Actualizar()
        {
            return "Éxito: Registro modificado en archivo SQLite.";
        }

        public string Eliminar()
        {
            return "Éxito: Registro removido de archivo SQLite.";
        }
    }
}
