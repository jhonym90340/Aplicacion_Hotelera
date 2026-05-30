using System;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    public class ConexionMongoDB : IAbstractFactoryBaseDatos
    {
        // Propiedad requerida por la interfaz
        public string SQL { get; set; } = string.Empty;

        public string Insertar()
        {
            // Cambiamos el color de la consola a amarillo para identificar MongoDB NoSQL
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Fábrica Concreta - MongoDB NoSQL Document Engine]");
            Console.WriteLine("Interceptando persistencia agnóstica de datos...");
            Console.WriteLine($"[BSON/JSON Document Stored]: {SQL}");
            Console.ResetColor();

            return "Éxito: Documento JSON indexado correctamente en la colección del clúster de MongoDB.";
        }

        public string Consultar()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Fábrica Concreta - MongoDB NoSQL Document Engine]");
            Console.WriteLine("Buscando documentos en colecciones...");
            Console.ResetColor();

            // Retorna una respuesta simulada en formato JSON de base de datos documental
            return "[{\"_id\": \"64fca12e8b3a4c2198000001\", \"Status\": \"Conexión activa a colecciones MongoDB\"}]";
        }

        public string Actualizar()
        {
            return "Éxito: Documento actualizado en la colección de MongoDB.";
        }

        public string Eliminar()
        {
            return "Éxito: Documento removido de la colección de MongoDB.";
        }
    }
}
