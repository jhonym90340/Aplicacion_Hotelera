using System;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Clases
{
    public static class Configuracion
    {
        // Almacena de forma global e intermedia el motor de base de datos 
        // seleccionado por el usuario en el menú de la consola.
        // Por defecto iniciará apuntando a SQL Server.
        public static eBaseDatos baseDatos { get; set; } = eBaseDatos.SQLServer;
    }
}
