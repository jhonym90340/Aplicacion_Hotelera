using System;
using pPatronesDiseñoHotel.Patrones.Clases;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Estructurales.Adapter
{
    // Clase Externa/Incompatible (Imagina que es un SDK de PayPal que no podemos modificar)
    public class ExternalPaymentService
    {
        public string ExecuteExternalTransaction(double amount, string token)
        {
            return $"SUCCESS_AUTH_CODE_{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}_AMOUNT_{amount}";
        }
    }

    // Interfaz esperada por nuestro sistema de Hotel
    public interface ITargetProcesadorPago
    {
        string RegistrarPagoHotel(double monto, string idReserva);
    }

    // El Adaptador que une ambos mundos y persiste el Log
    public class PagoAdapter : ITargetProcesadorPago
    {
        private ExternalPaymentService _externalService = new ExternalPaymentService();

        public string RegistrarPagoHotel(double monto, string idReserva)
        {
            // 1. Ejecuta la transacción en el servicio externo incompatible
            string respuestaExterna = _externalService.ExecuteExternalTransaction(monto, "TK-HOTEL-2026");

            // 2. Transforma y audita el resultado en la BD activa mediante el broker
            brokerHotel broker = new brokerHotel();

            if (Configuracion.baseDatos == eBaseDatos.MongoDB)
            {
                broker.SQL = $"{{\"Accion\":\"Pago Recibido\", \"Reserva\":\"{idReserva}\", \"Monto\":{monto}, \"Status\":\"{respuestaExterna}\"}}";
            }
            else
            {
                broker.SQL = $"INSERT INTO LogPagos (IdReserva, Descripcion, Monto) VALUES ('{idReserva}', '{respuestaExterna}', {monto})";
            }

            string resultadoBD = broker.Insertar();
            return $"[Respuesta Pasarela]: {respuestaExterna}\n[Persistencia]: {resultadoBD}";
        }
    }
}
