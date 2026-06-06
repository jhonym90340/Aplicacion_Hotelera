# Sistema de Gestión de Hotel con Patrones de Diseño

Este es un proyecto académico portable desarrollado en **C# y .NET 8.0** para la materia de Arquitectura de Software / Patrones de Diseño. El objetivo principal es demostrar el desacoplamiento arquitectónico entre la lógica del negocio del hotel y la capa de datos mediante el uso integrado de múltiples patrones.

---

##  Requisitos e Instalación

El proyecto fue diseñado bajo el principio de **portabilidad absoluta**, lo que significa que **no necesitas instalar servidores de bases de datos locales** (como instancias reales de SQL Server o MongoDB) ni configurar cadenas de conexión en red o firewalls. 

### Requisitos Mínimos:
1. **Entorno de Desarrollo:** Visual Studio 2022 (versión 17.8 o superior) o Visual Studio Code con el SDK de .NET 8 instalado.
2. **Carga de trabajo necesaria:** En el instalador de Visual Studio, asegúrate de tener marcado el componente: **"Desarrollo de escritorio de .NET"**.
3. **SDK:** .NET 8.0 (Runtime incluido).

---

##  Cómo Ejecutar el Proyecto

Tienes dos formas de poner a correr la aplicación de consola:

### Opción 1: Desde Visual Studio 
1. Clona el repositorio o descarga el código fuente en tu computadora.
2. Abre el archivo de la solución (`Aplicacion_Hotelera.sln`) dándole doble clic.
3. Espera a que Visual Studio restaure los paquetes necesarios (se hace de forma automática).
4. Presiona la tecla **`F5`** o haz clic en el botón de **Play (Triángulo Verde)** en la barra de herramientas superior para iniciar la depuración.
   * *Tip Pro:* Si prefieres que la consola se quede abierta sin detener el flujo al terminar, ejecútalo usando **`Ctrl + F5`** (Iniciar sin depurar).

### Opción 2: Ejecución directa del ejecutable (.exe)
Si quieres probarlo o exponerlo sin abrir el entorno de desarrollo:
1. Navega a la siguiente ruta dentro de la carpeta del proyecto:
   `..\Aplicacion_Hotelera\bin\Debug\net8.0\`
2. Busca el archivo llamado **`Aplicacion_Hotelera.exe`**.
3. Dale doble clic para abrir la consola directamente.

---

##  Arquitectura y Patrones Implementados

El núcleo del software gira en torno a la simulación interactiva de operaciones hoteleras, donde cada funcionalidad expone de manera explícita un patrón de diseño del GoF (*Gang of Four*):

### 1. Abstract Factory (Creacional - Base del Proyecto)
* **Ubicación:** `Patrones/Creacionales/Abstract_Factory/`
* **Propósito:** Permite cambiar dinámicamente el motor de persistencia del sistema en tiempo de ejecución a través de la **Opción 1** del menú. El sistema puede conmutar transparentemente entre **SQL Server, MongoDB y SQLite** cambiando la estructura de las consultas generadas sin alterar las reglas del hotel.

### 2. Builder (Creacional - Adicional)
* **Ubicación:** `Patrones/Creacionales/Builder/`
* **Propósito:** Controla la construcción paso a paso del objeto complejo `ReservaHotel` (Cliente  Habitación  Días  Extras como desayuno buffet). Se ejecuta en la **Opción 2** del menú.

### 3. Adapter (Estructural - Adicional)
* **Ubicación:** `Patrones/Estructurales/Adapter/`
* **Propósito:** Conecta el sistema del hotel con un SDK nativo externo de pasarela de pagos (simulado en inglés). Convierte las peticiones nativas del hotel al formato requerido por el tercero y traduce la respuesta en un log almacenable. Se ejecuta en la **Opción 3** del menú.

### 4. Observer (Comportamiento - Adicional)
* **Ubicación:** `Patrones/Comportamiento/Observer/`
* **Propósito:** Desacopla las alertas internas. Cuando el estado de una habitación cambia (Ocupada, Limpieza, Mantenimiento), el sujeto notifica automáticamente a los observadores adjuntos (como el `AuditorBDObserver`) para registrar la auditoría en la BD activa actual. Se ejecuta en la **Opción 4** del menú.


---
*Nota: Este proyecto maneja persistencia simulada en memoria (Mocking) orientada puramente a la demostración de patrones arquitectónicos, garantizando un despliegue inmediato en cualquier máquina de evaluación.*
