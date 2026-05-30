using System;

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

            // Retorna un formato JSON simulado tal como lo espera el broker
            return "[{\"Mensaje\": \"Datos de reservas cargados desde SQL Server con éxito\"}]";
        }

        public string Actualizar()
        {
            return "Éxito: Registro actualizado en SQL Server.";
        }

        public string Eliminar()
        {
            return "Éxito: Registro eliminado de SQL Server.";
        }
    }
}
