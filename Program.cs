using System;
using pPatronesDiseñoHotel.Patrones.Clases;
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;
using pPatronesDiseñoHotel.Patrones.Creacionales.Builder;
using pPatronesDiseñoHotel.Patrones.Estructurales.Adapter;
using pPatronesDiseñoHotel.Patrones.Comportamiento.Observer;

// ====================================================
// FLUJO PRINCIPAL DE LA CONSOLA (.NET 8.0)
// ====================================================
int opcion = 0;
do
{
    Console.Clear();
    Console.WriteLine("====================================================");
    Console.WriteLine("     SISTEMA DE GESTIÓN DE HOTEL (CON PATRONES)     ");
    Console.WriteLine("====================================================");
    // Muestra dinámicamente cuál base de datos está seleccionada globalmente
    Console.WriteLine($"[BD ACTIVA ACTUALMENTE: {Configuracion.baseDatos}]\n");

    Console.WriteLine("1. Configurar / Cambiar Base de Datos Activa (Abstract Factory)");
    Console.WriteLine("2. Crear Nueva Reserva de Habitación (Builder)");
    Console.WriteLine("3. Registrar Pago de Reserva con Pasarela Externa (Adapter)");
    Console.WriteLine("4. Cambiar Estado de Habitación y Notificar (Observer)");
    Console.WriteLine("5. Salir");
    Console.WriteLine("====================================================");
    Console.Write("Seleccione una opción: ");

    if (int.TryParse(Console.ReadLine(), out opcion))
    {
        switch (opcion)
        {
            case 1:
                ConfigurarBaseDatos();
                break;
            case 2:
                CrearReservaConBuilder();
                break;
            case 3:
                ProcesarPagoConAdapter();
                break;
            case 4:
                SimularCambioEstadoObserver();
                break;
            case 5:
                Console.WriteLine("\nSaliendo del programa... ¡Muchos éxitos en la sustentación!");
                break;
            default:
                Console.WriteLine("\nOpción no válida. Presione ENTER para reintentar.");
                Console.ReadLine();
                break;
        }
    }
} while (opcion != 5);


// ====================================================
// MÉTODOS DE SOPORTE PARA CADA OPCIÓN DEL MENÚ
// ====================================================

void ConfigurarBaseDatos()
{
    Console.Clear();
    Console.WriteLine("--- SELECCIÓN DE BASE DE DATOS (Abstract Factory) ---");
    Console.WriteLine("1. SQL Server");
    Console.WriteLine("2. MongoDB");
    Console.WriteLine("3. SQLite");
    Console.Write("\nSeleccione el motor de persistencia: ");
    string sel = Console.ReadLine();

    if (sel == "1") Configuracion.baseDatos = eBaseDatos.SQLServer;
    else if (sel == "2") Configuracion.baseDatos = eBaseDatos.MongoDB;
    else if (sel == "3") Configuracion.baseDatos = eBaseDatos.SQLite;

    Console.WriteLine($"\n¡Base de datos cambiada con éxito a: {Configuracion.baseDatos}!");
    Console.WriteLine("\nPresione ENTER para volver al menú...");
    Console.ReadLine();
}

void CrearReservaConBuilder()
{
    Console.Clear();
    Console.WriteLine("--- CREACIÓN DE RESERVA (Patrón Builder) ---");
    Console.Write("Nombre del Cliente: ");
    string cliente = Console.ReadLine();

    Console.Write("Tipo de Habitación (Suite / Estandar): ");
    string tipo = Console.ReadLine();

    Console.Write("Cantidad de días de estadía: ");
    int.TryParse(Console.ReadLine(), out int dias);
    if (dias <= 0) dias = 1;

    Console.Write("¿Incluye Desayuno Buffet? (S/N): ");
    bool desayuno = Console.ReadLine().ToUpper() == "S";

    // Invocar al Director del Builder
    DirectorReservas director = new DirectorReservas();
    IReservaBuilder builder = new ReservaHotelBuilder();

    Console.WriteLine("\n[Builder] Construyendo objeto complejo Reserva paso a paso...");

    // Lógica interna que guarda usando el Broker y la fábrica abstracta
    string resultado = director.GuardarReservaEnBD(builder, cliente, tipo, dias, desayuno);

    Console.WriteLine($"\nResultado: {resultado}");
    Console.WriteLine("\nPresione ENTER para volver al menú...");
    Console.ReadLine();
}

void ProcesarPagoConAdapter()
{
    Console.Clear();
    Console.WriteLine("--- PROCESAMIENTO DE PAGO (Patrón Adapter) ---");
    Console.Write("Ingrese el ID de la reserva a pagar: ");
    string idReserva = Console.ReadLine();
    Console.Write("Monto a pagar: $");

    // SOLUCIÓN AL ERROR CS1615 y ReadOnlySpan: Usamos tryParse correctamente para doubles
    double.TryParse(Console.ReadLine(), out double monto);

    // Instanciamos el adaptador
    ITargetProcesadorPago pasarela = new PagoAdapter();

    Console.WriteLine("\n[Adapter] Adaptando respuesta en inglés del SDK externo a logs del sistema...");
    string resultadoBD = pasarela.RegistrarPagoHotel(monto, idReserva);

    Console.WriteLine($"\n{resultadoBD}");
    Console.WriteLine("\nPresione ENTER para volver al menú...");
    Console.ReadLine();
}

void SimularCambioEstadoObserver()
{
    Console.Clear();
    Console.WriteLine("--- CAMBIO ESTADO DE HABITACIÓN (Patrón Observer) ---");
    Console.Write("Número de habitación: ");
    string num = Console.ReadLine();
    Console.WriteLine("\nSeleccione nuevo estado:\n1. Ocupada\n2. En Limpieza\n3. Mantenimiento");
    Console.Write("Opción: ");
    string estSel = Console.ReadLine();
    string nuevoEstado = estSel == "1" ? "Ocupada" : estSel == "2" ? "En Limpieza" : "Mantenimiento";

    // Configurar Sujeto y Observador
    ControlHabitaciones control = new ControlHabitaciones();
    AuditorBDObserver auditorBD = new AuditorBDObserver();

    // Adjuntar el observador encargado de auditar en la BD activa
    control.Adjuntar(auditorBD);

    control.CambiarEstadoHabitacion(num, nuevoEstado);

    Console.WriteLine("\nPresione ENTER para volver al menú...");
    Console.ReadLine();
}