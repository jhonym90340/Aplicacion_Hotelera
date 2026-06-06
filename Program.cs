// Asegúrate de que el namespace coincida con tu carpeta de patrones
using pPatronesDiseñoHotel.Patrones.Creacionales.Abstract_Factory;
using pPatronesDiseñoHotel.Patrones.Creacionales.Builder;
using System;

namespace pPatronesDiseñoHotel
{
    class Program
    {
        static void Main(string[] args)
        {
            // =========================================================================
            //  Declarar la fábrica abstracta global del sistema.
            // Por defecto, inicializamos el hotel apuntando a SQL Server.
            // =========================================================================
            IAbstractFactoryBD fabricaActiva = new SqlServerFactory();

            // Variable para almacenar temporalmente el motor de base de datos construido
            IAbstractFactoryBaseDatos motorBD;

            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("==================================================");
                Console.WriteLine("       SISTEMA DE GESTIÓN HOTELERA VANTIX         ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. Configurar / Cambiar Motor de Base de Datos");
                Console.WriteLine("2. Registrar Nueva Reserva (Insertar)");
                Console.WriteLine("3. Consultar Estado de Ocupación (Consultar)");
                Console.WriteLine("4. Actualizar Datos de Habitación (Actualizar)");
                Console.WriteLine("5. Cancelar Reserva del Hotel (Eliminar)");
                Console.WriteLine("6. Salir del Sistema");
                Console.WriteLine("==================================================");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("--- CONFIGURACIÓN GLOBAL DE PERSISTENCIA ---");
                        Console.WriteLine("1. Activar Microsoft SQL Server");
                        Console.WriteLine("2. Activar MongoDB Cloud Cluster");
                        Console.WriteLine("3. Activar SQLite Local Embedded");
                        Console.Write("Seleccione el motor: ");
                        string motorSeleccionado = Console.ReadLine();

                        // =========================================================================
                        // ARCHITECTURAL STEP 2: El Polimorfismo entra en acción.
                        // Reemplazamos el antiguo 'switch' interno que hacía 'new' de las conexiones.
                        // Ahora asignamos directamente la clase Fábrica Concreta correspondiente.
                        // =========================================================================
                        if (motorSeleccionado == "1")
                        {
                            fabricaActiva = new SqlServerFactory();
                            Console.WriteLine("\n[CONFIG]: Fábrica cambiada exitosamente a SQLServerFactory.");
                        }
                        else if (motorSeleccionado == "2")
                        {
                            fabricaActiva = new MongoDbFactory();
                            Console.WriteLine("\n[CONFIG]: Fábrica cambiada exitosamente a MongoDbFactory.");
                        }
                        else if (motorSeleccionado == "3")
                        {
                            fabricaActiva = new SqliteFactory();
                            Console.WriteLine("\n[CONFIG]: Fábrica cambiada exitosamente a SqliteFactory.");
                        }
                        else
                        {
                            Console.WriteLine("\nOpción inválida. Se mantiene la fábrica anterior.");
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("--- REGISTRAR NUEVA RESERVA (BUILDER + ABSTRACT FACTORY) ---");

                        // 1. Solicitamos los datos dinámicos al usuario por consola
                        Console.Write("Ingrese el nombre del cliente: ");
                        string nombreCliente = Console.ReadLine();

                        Console.Write("Ingrese el tipo de habitación (Ej: Suite, Estándar, Doble): ");
                        string tipoHab = Console.ReadLine();

                        Console.Write("Ingrese la cantidad de días de estadía: ");
                        int cantDias = 1;
                        int.TryParse(Console.ReadLine(), out cantDias);

                        Console.Write("¿Incluye servicios extras/mascota? (S/N): ");
                        string respuestaExtra = Console.ReadLine().ToUpper();
                        bool incluyeExtras = (respuestaExtra == "S" || respuestaExtra == "SI");

                        // 2. Instanciamos el constructor concreto (Builder)
                        IReservaBuilder miBuilder = new ReservaHotelBuilder();

                        // 3. Instanciamos el Director de reservas
                        DirectorReservas director = new DirectorReservas();

                        Console.WriteLine("\n[Procesando]... El Director está orquestando la construcción con el Builder.");

                        // =========================================================================
                        // PASO CLAVE: Llamamos al director pasando los datos capturados 
                        // y le inyectamos la 'fabricaActiva' global para la persistencia pura.
                        // =========================================================================
                        string resultadoInsertar = director.GuardarReservaEnBD(miBuilder, nombreCliente, tipoHab, cantDias, incluyeExtras, fabricaActiva);

                        Console.WriteLine("\n[Respuesta del Motor de Persistencia]:");
                        Console.WriteLine(resultadoInsertar);

                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("--- CONSULTAR RESERVAS ACTIVAS (ABSTRACT FACTORY) ---");
                        Console.WriteLine("Enviando petición de lectura de forma agnóstica a través del Broker...\n");

                        // 1. Instanciamos el broker encargado de la comunicación externa
                        pPatronesDiseñoHotel.Patrones.Clases.brokerHotel brokerConsulta = new pPatronesDiseñoHotel.Patrones.Clases.brokerHotel();

                        // 2. Definimos la consulta base según la especificación del patrón
                        if (fabricaActiva is MongoDbFactory)
                        {
                            brokerConsulta.SQL = "{ db.reservas.find({ estado: 'Activo' }) }";
                        }
                        else
                        {
                            brokerConsulta.SQL = "SELECT IdReserva, Cliente, TipoHabitacion, Total FROM TBL_RESERVAS;";
                        }

                        // =========================================================================
                        // PASO ARQUITECTÓNICO: Inyectamos la fábrica activa por parámetro al método Consultar
                        // =========================================================================
                        string resultadoConsultar = brokerConsulta.Consultar(fabricaActiva);

                        Console.WriteLine("==================================================");
                        Console.WriteLine("[DATOS RECUPERADOS DEL MOTOR DE PERSISTENCIA]:");
                        Console.WriteLine("==================================================");
                        Console.WriteLine(resultadoConsultar);
                        Console.WriteLine("==================================================");

                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("--- ACTUALIZAR REGISTRO DE RESERVA (ABSTRACT FACTORY) ---");

                        // 1. Solicitamos los datos dinámicos al usuario
                        Console.Write("Ingrese el ID de la reserva a modificar (Ej: RES-550E): ");
                        string idReservaModificar = Console.ReadLine();

                        Console.WriteLine("\nEstados sugeridos: [CheckIn], [CheckOut], [Cancelado]");
                        Console.Write("Ingrese el nuevo estado para la reserva: ");
                        string nuevoEstado = Console.ReadLine();

                        // 2. Instanciamos el Director para procesar la regla de negocio y la persistencia
                        DirectorReservas directorActualizar = new DirectorReservas();

                        Console.WriteLine("\n[Procesando]... Enviando comando de actualización de forma polimórfica.");

                        // =========================================================================
                        // CAMBIO CLAVE: El director se encarga de cambiar la memoria y llamar al Broker
                        // =========================================================================
                        string resultadoActualizar = directorActualizar.ActualizarEstadoEnBD(idReservaModificar, nuevoEstado, fabricaActiva);

                        Console.WriteLine("\n[Respuesta del Motor de Persistencia]:");
                        Console.WriteLine(resultadoActualizar);

                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("--- ELIMINAR / CANCELAR RESERVA (ABSTRACT FACTORY) ---");

                        // 1. Solicitamos el ID de forma dinámica al usuario
                        Console.Write("Ingrese el ID de la reserva que desea cancelar: ");
                        string idReservaEliminar = Console.ReadLine();

                        // 2. Instanciamos el broker encargado de la persistencia
                        pPatronesDiseñoHotel.Patrones.Clases.brokerHotel brokerEliminar = new pPatronesDiseñoHotel.Patrones.Clases.brokerHotel();

                        // 3. Formateamos el comando según la base de datos que esté activa
                        if (fabricaActiva is MongoDbFactory)
                        {
                            // Formato Documental NoSQL para MongoDB
                            brokerEliminar.SQL = $"{{\"db.reservas.deleteOne\": {{\"id\": \"{idReservaEliminar}\"}}}}";
                        }
                        else
                        {
                            // Formato Relacional Estándar para SQL Server o SQLite
                            brokerEliminar.SQL = $"DELETE FROM TBL_RESERVAS WHERE IdReserva = '{idReservaEliminar}';";
                        }

                        Console.WriteLine("\n[Procesando]... Enviando comando de eliminación a través del Broker.");

                        // =========================================================================
                        // PASO ARQUITECTÓNICO:
                        // Usamos el método del broker inyectándole la fábrica activa. Como las operaciones 
                        // "No-Query" (Updates/Deletes) no retornan tablas, puedes canalizarlas 
                        // a través de Insertar(fabricaActiva) de manera segura y polimórfica.
                        // =========================================================================
                        string resultadoEliminar = brokerEliminar.Insertar(fabricaActiva);

                        Console.WriteLine("\n[Respuesta del Motor de Persistencia]:");
                        Console.WriteLine(resultadoEliminar);

                        Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                        Console.ReadKey();
                        break;

                    case "6":
                        salir = true;
                        Console.WriteLine("\nCerrando el sistema del hotel de forma segura. ¡Hasta luego!");
                        break;

                    default:
                        Console.WriteLine("\nOpción no válida. Intente de nuevo.");
                        System.Threading.Thread.Sleep(1500);
                        break;
                }
            }
        }
    }
}