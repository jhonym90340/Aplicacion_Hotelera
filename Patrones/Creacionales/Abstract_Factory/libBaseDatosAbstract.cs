using System;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    //  El Contrato de la Fábrica Abstracta: Cada fábrica se encargará de crear su propio motor independiente
    public interface IAbstractFactoryBD
    {
        IAbstractFactoryBaseDatos CrearMotor();
    }

    //  El Contrato del Producto
    public interface IAbstractFactoryBaseDatos
    {
        string SQL { get; set; }
        string Insertar();
        string Actualizar();
        string Eliminar();
        string Consultar();
    }

    // Enumerador para mantener compatibilidad con tus selecciones en los menús
    public enum eBaseDatos
    {
        SQLServer = 1,
        MongoDB = 2,
        SQLite = 3
    }
}