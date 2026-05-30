using System;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Clases
{
    public class brokerHotel
    {
        // Propiedad para inyectar la sentencia SQL o el documento JSON 
        // generado por los patrones de diseño externos
        public string? SQL { get; set; }

        public string Insertar()
        {
            // Crea la factory y le asigna la base de datos activa globalmente
            BaseDatosFactory factory = new BaseDatosFactory();
            factory.baseDatos = Configuracion.baseDatos;

            // Crea la instancia concreta (SQL Server, MongoDB o SQLite)
            var bd = factory.CrearBaseDatos();
            bd.SQL = this.SQL;

            // Ejecuta y retorna la respuesta del motor activo
            return bd.Insertar();
        }

        public string Consultar()
        {
            BaseDatosFactory factory = new BaseDatosFactory();
            factory.baseDatos = Configuracion.baseDatos;

            var bd = factory.CrearBaseDatos();
            bd.SQL = this.SQL;

            return bd.Consultar();
        }
    }
}
