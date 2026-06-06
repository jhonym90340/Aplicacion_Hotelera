using System;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory
{
    //  El Contrato de la Fábrica Abstracta: Cada fábrica se encargará de crear su propio motor independiente
    /*Es la interfaz global que define el contrato para las fábricas*/
    public interface IAbstractFactoryBD
    {
        IAbstractFactoryBaseDatos CrearMotor();
    }

    // IAbstractFactoryBaseDatos El Contrato del Producto
    /* Es la interfaz que define las operaciones permitidas para cualquier base 
     de datos en el sistema. Obliga a que todos tengan los métodos Insertar(), Consultar(), Actualizar() 
     y Eliminar().
    */

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