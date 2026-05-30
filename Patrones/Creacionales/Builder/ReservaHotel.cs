using System;
using Newtonsoft.Json;
using pPatronesDiseñoHotel.Patrones.Clases;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Creacionales.Builder
{
    // El objeto complejo que queremos construir
    public class Reserva
    {
        public string IdReserva { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string TipoHabitacion { get; set; } = string.Empty;
        public int Dias { get; set; }
        public double PrecioTotal { get; set; }
        public bool IncluyeDesayuno { get; set; }
    }

    // Interfaz del Constructor (Define los pasos)
    public interface IReservaBuilder
    {
        void DefinirCliente(string nombre);
        void ConfigurarHabitacion(string tipo, double precioPorDia);
        void EstablecerDias(int dias);
        void IncluirExtras(bool desayuno);
        Reserva ObtenReserva();
    }

    // Constructor Concreto
    public class ReservaHotelBuilder : IReservaBuilder
    {
        private Reserva _reserva = new Reserva();

        public void DefinirCliente(string nombre) => _reserva.Cliente = nombre;

        public void ConfigurarHabitacion(string tipo, double precio)
        {
            _reserva.TipoHabitacion = tipo;
            _reserva.PrecioTotal += precio;
        }

        public void EstablecerDias(int dias)
        {
            _reserva.Dias = dias;
            _reserva.PrecioTotal *= dias;
        }

        public void IncluirExtras(bool desayuno)
        {
            _reserva.IncluyeDesayuno = desayuno;
            if (desayuno) _reserva.PrecioTotal += 50000; // Costo fijo extra por desayuno
        }

        public Reserva ObtenReserva() => _reserva;
    }

    // El Director: Orquesta los pasos en el orden correcto y persiste usando el Broker
    public class DirectorReservas
    {
        public string GuardarReservaEnBD(IReservaBuilder builder, string cliente, string tipoHab, int dias, bool desayuno)
        {
            builder.DefinirCliente(cliente);
            builder.ConfigurarHabitacion(tipoHab, tipoHab.ToLower() == "suite" ? 200000 : 90000);
            builder.EstablecerDias(dias);
            builder.IncluirExtras(desayuno);

            Reserva nuevaReserva = builder.ObtenReserva();
            nuevaReserva.IdReserva = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();

            // CUMPLIMOS LA REGLA: Invocar el almacenamiento a través del Broker (Abstract Factory)
            brokerHotel broker = new brokerHotel();

            if (Configuracion.baseDatos == eBaseDatos.MongoDB)
            {
                // Si es MongoDB mandamos la estructura en JSON limpio
                broker.SQL = JsonConvert.SerializeObject(nuevaReserva);
            }
            else
            {
                // Si son relacionales (SQL Server / SQLite) mandamos la sentencia SQL
                broker.SQL = $"INSERT INTO Reservas (IdReserva, Cliente, TipoHabitacion, Dias, PrecioTotal) " +
                             $"VALUES ('{nuevaReserva.IdReserva}', '{nuevaReserva.Cliente}', '{nuevaReserva.TipoHabitacion}', {nuevaReserva.Dias}, {nuevaReserva.PrecioTotal})";
            }

            return broker.Insertar();
        }
    }
}
