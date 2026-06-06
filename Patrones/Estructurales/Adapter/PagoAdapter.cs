using System;
using pPatronesDiseñoHotel.Patrones.Clases;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;

namespace pPatronesDiseñoHotel.Patrones.Estructurales.Adapter
{
    // Clase Externa/Incompatible (SDK de PayPal - No se modifica)
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

    // El Adaptador que une ambos mundos y persiste el Log de forma desacoplada
    public class PagoAdapter : ITargetProcesadorPago
    {
        private readonly ExternalPaymentService _externalService = new ExternalPaymentService();

        // Propiedad privada para almacenar la fábrica inyectada
        private readonly IAbstractFactoryBD _factory;

        // =========================================================================
        // CONSTRUCTOR: Inyectamos la fábrica activa al adaptador desde el entorno
        // =========================================================================
        public PagoAdapter(IAbstractFactoryBD factory)
        {
            _factory = factory;
        }

        public string RegistrarPagoHotel(double monto, string idReserva)
        {
            // 1. Ejecuta la transacción en el servicio externo incompatible
            string respuestaExterna = _externalService.ExecuteExternalTransaction(monto, "TK-HOTEL-2026");

            // 2. Prepara el Broker para auditar el resultado
            brokerHotel broker = new brokerHotel();

            // =========================================================================
            // EVALUACIÓN POLIMÓRFICA: Evaluamos el TIPO de la fábrica, eliminando
            // por completo la dependencia de la clase estática 'Configuracion'
            // =========================================================================
            if (_factory is MongoDbFactory)
            {
                // Si la fábrica inyectada es de Mongo, estructuramos un documento JSON
                broker.SQL = $"{{\"Accion\":\"Pago Recibido\", \"Reserva\":\"{idReserva}\", \"Monto\":{monto}, \"Status\":\"{respuestaExterna}\"}}";
            }
            else
            {
                // Para cualquier fábrica relacional (SQLServer o SQLite) usamos Query estándar
                broker.SQL = $"INSERT INTO LogPagos (IdReserva, Descripcion, Monto) VALUES ('{idReserva}', '{respuestaExterna}', {monto})";
            }

           
            string resultadoBD = broker.Insertar(_factory);

            return $"[Respuesta Pasarela]: {respuestaExterna}\n[Persistencia]: {resultadoBD}";
        }
    }
}
