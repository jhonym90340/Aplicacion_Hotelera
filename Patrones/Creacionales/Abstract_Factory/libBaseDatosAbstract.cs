using System;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    // Interfaz para la fábrica abstracta de bases de datos
    public interface IAbstractFactoryBD
    {
        eBaseDatos baseDatos { get; set; }
        IAbstractFactoryBaseDatos CrearBaseDatos();
    }

    // Interfaz para los productos (los motores de bases de datos)
    public interface IAbstractFactoryBaseDatos
    {
        string SQL { get; set; }
        string Insertar();
        string Actualizar();
        string Eliminar();
        string Consultar();
    }

    // Enumerador con los motores seleccionados para el proyecto del hotel
    public enum eBaseDatos
    {
        SQLServer = 1,
        MongoDB = 2,
        SQLite = 3
    }

    // Fábrica concreta que instancia las conexiones según la configuración global
    public class BaseDatosFactory : IAbstractFactoryBD
    {
        public eBaseDatos baseDatos { get; set; }

        public IAbstractFactoryBaseDatos? CrearBaseDatos()
        {
            switch (baseDatos)
            {
                case eBaseDatos.SQLServer:
                    return new ConexionSQL();
                case eBaseDatos.MongoDB:
                    return new ConexionMongoDB();
                case eBaseDatos.SQLite:
                    return new ConexionSQLite();
                default:
                    return null;
            }
        }
    }
}
