using System;
using Newtonsoft.Json;
using pPatronesDiseñoHotel.Patrones.Creacionales.Builder;

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

            // =========================================================================
            // LECTURA DINÁMICA DE MEMORIA:
            // Evaluamos si la lista compartida de reservas no tiene elementos creados
            // =========================================================================
            if (MemoriaHotel.ListaReservas.Count == 0)
            {
                return "[\n  {\n    \"Mensaje\": \"No hay documentos de reservas en la colección de MongoDB aún.\"\n  }\n]";
            }

         
            return JsonConvert.SerializeObject(MemoriaHotel.ListaReservas, Formatting.Indented);
        }

        public string Actualizar()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Fábrica Concreta - MongoDB NoSQL Document Engine]");
            Console.WriteLine($"Ejecutando Comando de Actualización (UpdateOne): {SQL}");
            Console.ResetColor();

            return "Éxito: Documento actualizado en la colección de MongoDB de manera polimórfica.";
        }

        public string Eliminar()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Fábrica Concreta - MongoDB NoSQL Document Engine]");
            Console.WriteLine($"Ejecutando Comando de Eliminación (DeleteOne): {SQL}");
            Console.ResetColor();

            return "Éxito: Documento removido de la colección de MongoDB de manera polimórfica.";
        }
    }
}