Actúa como un Arquitecto de Software y Desarrollador Senior Full-Stack experto en .NET y Vue 3.

Contexto:
Tienes acceso a los proyectos de frontend y backend existentes.
Sigue estrictamente la arquitectura y convenciones existentes.

=====================================
BACKEND (IMPLEMENTAR PRIMERO)
=====================================

Stack Backend (no desviarse):
- ASP.NET Core
- Entity Framework Core
- Patrón CQRS
- FluentValidation
- Riok.Mapperly para mapeo
- Patrón Repository
- Desyco.Mediator, una librería fork de MediatR ya instalada (solo si ya se usa en el proyecto)

Referencia Backend:
- Entidad EF Core: [DayOfWeekEntity]

=====================================
FRONTEND (IMPLEMENTAR DESPUÉS DEL BACKEND)
=====================================

Stack Frontend (no desviarse):
- Vue 3
- Solo Composition API (nada de Options API)
- PrimeVue
- Tailwind CSS
- Axios
- Stores de Pinia

Referencia Frontend:
- Utiliza la imagen pantalla05.png como referencia para crear el nuevo componente.

=====================================
TAREA
=====================================

Objetivo:
Implementar una funcionalidad de mantenimiento completa end-to-end (Backend primero, luego UI)
basada en los patrones y referencias existentes.

=====================================
ANÁLISIS DE LA IMAGEN Y REQUERIMIENTOS DE UI
=====================================
La imagen pantalla05.png, muestra una lista iterable de días de la semana (Friday, Saturday, Sunday...). Para cada día, se presenta un formulario con lógica condicional:

1.  **Iteración:** El formulario se repite para cada día de la semana.
2.  **Campo Control:** "School Day" ( Yes/No) utiliza SelectButton o ToggleButton o ToggleSwitch segun sea mas apropiado.
3.  **Campos Dependientes:** Cuatro filas de horarios (Opens, Starts, Ends, Closes).
    - *Comportamiento:* Si "School Day" es "No", los campos de hora deben ocultarse o deshabilitarse. Si es "Yes", son obligatorios.
4.  **Estilo de los inputs de hora:** La imagen muestra selectores separados para hora y minuto. **IMPORTANTE:** Para modernizar la UX usando PrimeVue, reemplazaremos los dos selectores separados por un único componente `Calendar` en modo `timeOnly`.

=====================================
REGLAS BACKEND
=====================================

- Usa la entidad de EF Core como la única fuente de la verdad.
- NO modifiques la entidad a menos que se solicite explícitamente.
- Sigue estrictamente la separación CQRS (Comandos vs Consultas).
- Crea UN DTO común basado en la entidad.
- Reutiliza el DTO en:
    - Comandos (Commands)
    - Consultas (Queries)
    - Respuestas (cuando sea aplicable)
- NO dupliques DTOs a menos que sea explícitamente necesario.

Estructura CQRS:

Comandos (Commands): (solo las que apliquen o crear las adecuadas)
- Create[NombreEntidad]Command
- Update[NombreEntidad]Command
- Delete[NombreEntidad]Command

Consultas (Queries): (solo las que apliquen o crear las adecuadas)
- Get[NombreEntidad]ByIdQuery
- Get[NombreEntidad]ListQuery

Validación:
- Usa solo FluentValidation.
- La validación debe vivir exclusivamente en los validadores.
- No debe haber lógica de validación dentro de los handlers.

Mapeo:
- Usa solo Riok.Mapperly.
- Mapeos explícitos:
    - Entidad ↔ DTO

Persistencia:
- Usa SIEMPRE el patrón Repository.
- Los Handlers NO deben acceder al DbContext directamente.
- Si ya existe un repositorio:
    - Sigue su interfaz, nomenclatura y estructura.
- Si no existe un repositorio:
    - Crea uno siguiendo exactamente el mismo patrón
      usado por los repositorios existentes en el proyecto.
- Responsabilidades del Repositorio:
    - Solo acceso a datos
    - Nada de lógica de negocio
    - Solo métodos asíncronos (Async)

Seeder:
- Añade la nueva funcionalidad al FeatureSeeder y reordena la secuencia.

=====================================
REGLAS FRONTEND
=====================================
- Genera un componente llamado `WeeklySchedule.vue`.
- Usa `Accordion` y `AccordionPanel` para separar visualmente cada día de la semana.
- Input "School Day": Usa el componente **`SelectButton o ToggleButton o ToggleSwitch segun sea mas apropiado.
- Inputs de Hora: Usa el componente **`DatePicker`** con las propiedades `timeOnly`, `hourFormat="24"` y `showIcon`. (Esto sustituye a los selectores antiguos de la imagen para una mejor UX)
- Botón de guardar: Usa el componente **`Button`**.
- Implementa el frontend SOLO después de que los contratos del backend estén definidos.
- Los modelos del frontend deben coincidir exactamente con los DTOs del backend.
- Usa el componente de referencia como la única fuente de verdad.
- Sigue estrictamente el Modo de UI CRUD seleccionado.
- Reutiliza el diseño (layout), UX y patrones de código cuando sea aplicable.
- NO introduzcas nuevas librerías ni cambios de arquitectura.
- Utiliza la opcion route de menu existente '/school/years-days-times/days-of-the-week' el componente 'SchoolSettings.vue' para mostrar el nuevo componente.
- El route estará vinculado a un componente contenedor y este a su vez comtendrá el componente `WeeklySchedule.vue`.
- La carga de los datos estará configurada en el componente contenedor y luego pasado al `WeeklySchedule.vue` por a travez de propiedades y emits para guardar.

Patrones de UI reutilizables (cuando aplique):
- PrimeVue DataTable (paginación, ordenamiento, filtrado)
- Crear/Editar vía Diálogo (Modo Basado en Modal)
- Crear/Editar vía Vistas de Router (Modo Basado en Rutas)
- Componentes de formulario de PrimeVue
- Estados de carga (loading), error y vacío
- Clases de utilidad de Tailwind
- Store de Pinia (estado, acciones, loading, error)
- Patrón de servicio Axios

Lógica Frontend:
- Crea un estado reactivo que contenga el array de los 7 días.
- Implementa la validación: `StartTime` no puede ser mayor que `EndTime`.

=====================================
REGLAS DE INTEGRACIÓN
=====================================

- Los DTOs del Backend definen el contrato de la API.
- El Frontend debe adaptarse al backend, nunca al revés.
- Consistencia de nomenclatura entre:
    - Entidad
    - DTO
    - Comandos / Consultas
    - Endpoints de API
    - Modelos de Frontend
- Respeta los contratos de paginación, filtrado y ordenamiento.
- NO asumas campos, enums o comportamientos que no estén explícitamente definidos.

=====================================
ORDEN DE IMPLEMENTACIÓN
=====================================

1. Analizar la entidad EF Core.
2. Definir el DTO común del backend.
3. Crear o adaptar el repositorio.
4. Implementar Comandos + Handlers.
5. Implementar Consultas + Handlers.
6. Añadir validadores FluentValidation.
7. Añadir mapper Riok.Mapperly.
8. Verificar contratos del backend.
9. Implementar store, servicios y componentes del frontend.

=====================================
SALIDA (ENTREGABLES)
=====================================

Backend:
- Interfaz e implementación del Repositorio
- DTO Común
- Comandos + Handlers
- Consultas + Handlers
- Validadores FluentValidation
- Mapper de Mapperly

Frontend:
- Componente(s) Vue 3
- Store de Pinia (si se requiere)
- Servicio Axios (si se requiere)

Resumen:
- Modo de UI CRUD seleccionado
- Elementos de backend creados
- Elementos de frontend creados
- Patrones reutilizados
- Omisiones intencionales y sus razones
