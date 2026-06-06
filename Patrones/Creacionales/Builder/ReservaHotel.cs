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

        public string Estado { get; set; } = "Activo"; // Por defecto nace Activa
    }

    
    public interface IReservaBuilder
    {
        void DefinirCliente(string nombre);
        void DefinirHabitacion(string tipo, double precioPorDia);
        void DefinirDias(int dias);
        void DefinirMascota(bool conMascota); // Equivale a incluir extras o personalizaciones
        Reserva ObtenReserva();
    }

    // Constructor Concreto implementando el nuevo contrato
    public class ReservaHotelBuilder : IReservaBuilder
    {
        private Reserva _reserva = new Reserva();

        public ReservaHotelBuilder()
        {
            _reserva.IdReserva = "RES-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        public void DefinirCliente(string nombre) => _reserva.Cliente = nombre;

        public void DefinirHabitacion(string tipo, double precioPorDia)
        {
            _reserva.TipoHabitacion = tipo;
            _reserva.PrecioTotal += precioPorDia;
        }

        public void DefinirDias(int dias)
        {
            _reserva.Dias = dias;
            _reserva.PrecioTotal *= dias;
        }

        public void DefinirMascota(bool conMascota)
        {
            _reserva.IncluyeDesayuno = conMascota; // Usamos el flag para simular el extra
            if (conMascota) _reserva.PrecioTotal += 50000; // Costo extra en la simulación
        }

        public Reserva ObtenReserva() => _reserva;
    }

    // El Director: Orquesta los pasos y persiste usando el Broker de forma limpia
    public class DirectorReservas
    {
        public string GuardarReservaEnBD(IReservaBuilder builder, string cliente, string habitacion, int dias, bool conMascota, IAbstractFactoryBD fabricaActiva)
        {
            // 1. El Director orquesta la construcción del objeto complejo en el orden correcto
            builder.DefinirCliente(cliente);
            builder.DefinirHabitacion(habitacion, 120000); // Pasamos habitación y precio base simulado
            builder.DefinirDias(dias);
            builder.DefinirMascota(conMascota);

            // 2. Extraemos el objeto completamente construido
            Reserva nuevaReserva = builder.ObtenReserva();

            // =========================================================================
            //   PERSISTENCIA EN MEMORIA DINÁMICA
            // Guardamos el objeto real en nuestra lista estática global antes de que el 
            // método termine. Esto permite que la Opción 3 acumule múltiples registros.
            // =========================================================================
            MemoriaHotel.ListaReservas.Add(nuevaReserva);

            // 3. Prepara el Broker para realizar la inserción simulada en consola
            brokerHotel broker = new brokerHotel();

            // =========================================================================
            // FORMATEO DINÁMICO: Evaluamos el tipo de fábrica para armar SQL o JSON
            // =========================================================================
            if (fabricaActiva is MongoDbFactory)
            {
                // Si es MongoDB, serializamos el objeto construido a JSON puro usando la librería
                broker.SQL = JsonConvert.SerializeObject(nuevaReserva);
            }
            else
            {
                // Si es SQL Server o SQLite, estructuramos la sentencia relacional estándar
                broker.SQL = $"INSERT INTO Reservas (Id, Cliente, Habitacion, Dias, Total) VALUES ('{nuevaReserva.IdReserva}', '{nuevaReserva.Cliente}', '{nuevaReserva.TipoHabitacion}', {nuevaReserva.Dias}, {nuevaReserva.PrecioTotal});";
            }

            // =========================================================================
            // RETORNO SEGURO: 
            // Se invoca pasando la fábrica activa por parámetro y se retorna el string
            // =========================================================================
            return broker.Insertar(fabricaActiva);
        }


        // =========================================================================
       // Actualiza el estado real en la lista de memoria
        // =========================================================================
        public string ActualizarEstadoEnBD(string idReserva, string nuevoEstado, IAbstractFactoryBD fabricaActiva)
        {
            // 1. Buscamos la reserva real en la lista de memoria usando Linq o un ciclo clásico
            Reserva reservaEncontrada = MemoriaHotel.ListaReservas.Find(r => r.IdReserva.Equals(idReserva, StringComparison.OrdinalIgnoreCase));

            if (reservaEncontrada != null)
            {
                // ¡Modificamos el estado en el objeto real de la memoria!
                reservaEncontrada.Estado = nuevoEstado;
            }

            // 2. Preparamos el Broker para la simulación visual en consola
            brokerHotel broker = new brokerHotel();

            if (fabricaActiva is MongoDbFactory)
            {
                broker.SQL = $"{{\"db.reservas.updateOne\": {{\"id\": \"{idReserva}\"}}, \"$set\": {{\"Estado\": \"{nuevoEstado}\"}}}}";
            }
            else
            {
                broker.SQL = $"UPDATE TBL_RESERVAS SET Estado = '{nuevoEstado}' WHERE IdReserva = '{idReserva}';";
            }

            // 3. Invocamos al broker usando el método dedicado que creamos en el paso anterior
            return broker.Actualizar(fabricaActiva);
        }


    }

    public static class MemoriaHotel
    {
        // Esta lista guardará todas las reservas de forma temporal mientras el programa esté abierto
        public static System.Collections.Generic.List<Reserva> ListaReservas { get; set; }
            = new System.Collections.Generic.List<Reserva>();
    }

}