using System;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Clases
{
    public class brokerHotel
    {
        // Propiedad para inyectar la sentencia SQL o el documento JSON 
        // generado por los patrones de diseño externos
        public string? SQL { get; set; }

        /// <summary>
        /// Procesa la inserción de datos en el motor de persistencia activo.
        /// Recibe la fábrica abstracta para cumplir con el patrón Abstract Factory puro.
        /// </summary>
        /// <param name="factory">Fábrica concreta activa (SQLServer, MongoDB o SQLite)</param>
        /// <returns>Mensaje de éxito o estado del motor correspondiente</returns>
        public string Insertar(IAbstractFactoryBD factory)
        {
            // Crea la instancia concreta del motor utilizando el método del Abstract Factory puro
            IAbstractFactoryBaseDatos bd = factory.CrearMotor();

            // Asigna la consulta o documento estructurado al motor construido
            bd.SQL = this.SQL;

            // Ejecuta y retorna de forma polimórfica la respuesta del motor activo
            return bd.Insertar();
        }

        /// <summary>
        /// Procesa la consulta de datos en el motor de persistencia activo de manera agnóstica.
        /// </summary>
        /// <param name="factory">Fábrica concreta activa (SQLServer, MongoDB o SQLite)</param>
        /// <returns>Cadena de texto con los datos simulados en formato JSON</returns>
        public string Consultar(IAbstractFactoryBD factory)
        {
            // Crea la instancia concreta del motor sin condicionales internos
            IAbstractFactoryBaseDatos bd = factory.CrearMotor();

            // Asigna el comando de lectura
            bd.SQL = this.SQL;

            // Retorna la colección de datos estructurada según la tecnología activa
            return bd.Consultar();
        }


        public string Actualizar(IAbstractFactoryBD fabrica)
        {
            IAbstractFactoryBaseDatos motor = fabrica.CrearMotor();
            motor.SQL = this.SQL;
            return motor.Actualizar(); // Llama al método Actualizar de ConexionSQL o ConexionSQLite
        }
    }
}