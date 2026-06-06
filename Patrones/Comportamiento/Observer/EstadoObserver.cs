using System;
using System.Collections.Generic;
using pPatronesDiseñoHotel.Patrones.Clases;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Comportamiento.Observer
{
    // Interfaz del Observador
    public interface IObservadorHotel
    {
        void ActualizarEstado(string habitacion, string nuevoEstado);
    }

    // El Sujeto (Sujeto Observado)
    public class ControlHabitaciones
    {
        private List<IObservadorHotel> _observadores = new List<IObservadorHotel>();

        public void Adjuntar(IObservadorHotel observador) => _observadores.Add(observador);

        public void CambiarEstadoHabitacion(string numeroHabitacion, string estado)
        {
            Console.WriteLine($"\n[Sujeto] Habitación {numeroHabitacion} cambió a estado: {estado}. Notificando observadores...");

            // Notifica a todos los suscritos
            foreach (var obs in _observadores)
            {
                obs.ActualizarEstado(numeroHabitacion, estado);
            }
        }
    }

    // Observador Concreto: Encargado de registrar la auditoría en la BD usando la Factory
    public class AuditorBDObserver : IObservadorHotel
    {
        public void ActualizarEstado(string habitacion, string nuevoEstado)
        {
            brokerHotel broker = new brokerHotel();

            if (Configuracion.baseDatos == eBaseDatos.MongoDB)
            {
                broker.SQL = $"{{\"Evento\":\"Cambio Estado Habitacion\", \"Habitacion\":\"{habitacion}\", \"Estado\":\"{nuevoEstado}\", \"Fecha\":\"{DateTime.Now:yyyy-MM-dd}\"}}";
            }
            else
            {
                broker.SQL = $"INSERT INTO HistorialHabitaciones (Habitacion, Estado, FechaLog) VALUES ('{habitacion}', '{nuevoEstado}', '{DateTime.Now:yyyy-MM-dd HH:mm:ss}')";
            }

            broker.Insertar(new pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory.SqlServerFactory());
        }
    }
}
