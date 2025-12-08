# Bitácora de Desarrollo - Proyecto SGD (Desyco.Dms)
**Última actualización:** 6 de Diciembre, 2025
**Estado:** En desarrollo activo (Backend/Infraestructura)

## 1. Contexto del Proyecto
*   **Tecnologías:** .NET 8, ASP.NET Core Web API, Entity Framework Core (SQL Server).
*   **Arquitectura:** DDD (Domain-Driven Design), CQRS (Command Query Responsibility Segregation).
*   **Librerías Clave:** 
    *   `Desyco.Mediator` (Implementación propia/wrapper de MediatR).
    *   `Autofac` (Inyección de dependencias y escaneo de ensamblados).
    *   `Riok.Mapperly` (Mapeo de objetos).
    *   `Serilog` (Logging).

## 2. Logros y Cambios Recientes

### A. Implementación de Autofac (Inyección de Dependencias)
Se reemplazó el contenedor DI nativo por **Autofac** para solucionar problemas de resolución de servicios y limpiar el `CompositionRoot`.
*   **Archivos Creados:**
    *   `source/Desyco.Dms.Application/ApplicationModule.cs`: Registra automáticamente todos los `*Mapper` (Singleton).
    *   `source/Desyco.Dms.Infrastructure/InfrastructureModule.cs`: Registra automáticamente todos los `*Repository` como sus interfaces (Scoped).
*   **Configuración:** Se modificó `Program.cs` para usar `UseServiceProviderFactory(new AutofacServiceProviderFactory())`.
*   **Limpieza:** Se eliminaron los registros manuales en `CompositionRoot.cs`, `AppComposition.cs` e `InfrastructureComposition.cs`.

### B. Correcciones en Entity Framework Core (Migraciones)
Se resolvieron varios errores bloqueantes para la creación de migraciones:
1.  **Incompatibilidad de Tipos (Enum vs Int):**
    *   `SectionEntity`: Se cambió `ShiftId` de `int` a `ShiftType` (Enum).
    *   `StudentStatusEntity`: Se cambió la herencia a `EntityBase<StudentStatus>` para coincidir con la propiedad en `StudentEntity`.
2.  **Ciclos de Borrado en Cascada (Multiple Cascade Paths):**
    *   Se configuró `OnDelete(DeleteBehavior.Restrict)` masivamente en las configuraciones de entidades para evitar que SQL Server bloquee la creación de tablas por ciclos de borrado.
    *   **Entidades Afectadas (Configuraciones modificadas):** 
        *   `Enrollment` (Inscripciones)
        *   `Section` (Secciones)
        *   `StudentGrade` (Calificaciones)
        *   `TeacherAssignment` (Asignaciones Docentes)
        *   `ClassSchedule` (Horarios)
        *   `Attendance` (Asistencia)
        *   `Invoice`, `InvoiceDetail` (Facturación)
        *   `Payment`, `PaymentDetail` (Pagos)
        *   `StudentGuardian`, `ScholarshipAward`.

### C. Desarrollo de API (Controllers)
*   **`AcademicYearsController`:** Se implementó el controlador RESTful completo usando `IMediator`.
    *   Endpoints: `GetAll`, `GetById`, `Create`, `Update`, `Delete`.
    *   Ubicación: `source/Desyco.Dms.Web/Controllers/AcademicYearsController.cs`.

### D. Implementación de Scrima.OData (Filtrado y Paginación)
*   **Verificación:** Se confirmó que los paquetes `Scrima` están instalados y configurados correctamente en todas las capas (`Web`, `Infrastructure`, `Domain`).
*   **Configuración:** Se verificó la llamada `services.AddODataQuery()` en `CompositionRoot`.
*   **CQRS con OData:**
    *   `GetAllAcademicYearsQuery`: Modificado para aceptar `QueryOptions` y retornar `QueryResult<AcademicYearDto>`.
    *   `GetAllAcademicYearsQueryHandler`: Actualizado para usar `GetResultListAsync` del repositorio (implementación de Scrima en EF Core).
*   **Mapeo Avanzado:** Se añadió un método `partial` en `AcademicYearMapper` para mapear automáticamente de `QueryResult<Entity>` a `QueryResult<Dto>` usando Riok.Mapperly.
*   **Controller:** `AcademicYearsController.GetAll` ahora acepta `[FromQuery] QueryOptions` y soporta sintaxis OData (filter, orderby, skip, top).

### E. Análisis de Frontend (Pendiente de implementación)
Se generó un documento (`FrontendMaintenanceViews.md`) clasificando las vistas necesarias en:
1.  **Catálogos Maestros:** (AcademicYear, Shift, Classroom, etc.) - Grids simples.
2.  **Gestión Compleja:** (Student, Teacher) - Formularios con pestañas/detalles.
3.  **Operativa:** (Enrollment, TeacherAssignment).
4.  **Financiero:** (Invoice, Payment).

### F. Implementación de Scalar UI
Se reemplazó **Swagger UI** con **Scalar UI** para una documentación de API más moderna.
*   **Paquete:** Instalado `Scalar.AspNetCore` (v2.11.0).
*   **Configuración:** Se actualizó `Program.cs` para usar `app.MapScalarApiReference()`.
*   **Acceso:** La documentación ahora está disponible (ruta por defecto `/scalar/v1`).

### G. Implementación de API Versioning
*   **Paquetes:** Se instalaron `Microsoft.AspNetCore.Mvc.Versioning` y `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer`.
*   **Configuración:** Se habilitó `AddApiVersioning` y `AddVersionedApiExplorer` en `Program.cs` para soportar versionado por URL (`/v{version}/`).
*   **Controllers:** Se aplicó `[ApiVersion("1.0")]` y la ruta versionada al `AcademicYearsController`.
*   **Corrección:** Se revirtieron cambios en `Scrima` dentro del controlador para respetar la implementación original, manteniendo solo la capa de versionado.

### H. Verificación General y Estabilización
*   **Compilación:** La solución compila exitosamente tras corregir errores en `Program.cs`.
*   **Base de Datos:** Se creó la migración `Fix_Enums_And_Cascades`.
*   **Ejecución:** Se aplicaron las migraciones exitosamente.
*   **Autofac:** Confirmado el funcionamiento correcto.

### I. Utilidades del Dominio
*   **SequentialGuidGenerator:** Se añadió la clase `source/Desyco.Dms.Domain/Common/SequentialGuidGenerator.cs`.
    *   **Propósito:** Generar GUIDs en C# que sean amigables con los índices de SQL Server (evitando fragmentación) mediante el algoritmo COMB.
    *   **Uso Previsto:** Facilitar una futura refactorización de entidades transaccionales (Facturas, Pagos) de `int` a `Guid` para optimizar inserciones masivas.

### J. Modificaciones en el Modelo de Datos (EducationalLevel)
*   **Nueva Configuración:** `EducationalLevelTypeConfiguration.cs` para la entidad `EducationalLevelTypeEntity` (Tabla: `EducationalLevelType`).
*   **Relación:** Se actualizó `EducationalLevelConfiguration.cs` para establecer la relación uno a muchos con `EducationalLevelTypeEntity` usando `LevelTypeId` y `DeleteBehavior.Restrict`.
*   **Migración:** Se generó la migración `AddEducationalLevelTypeEntity` para aplicar los cambios del modelo de datos.

### K. Implementación de ITranslationKey en Enums
*   **Objetivo:** Permitir soporte multiidioma en tablas de catálogo (Enum-Driven).
*   **Cambios:** Se implementó `ITranslationKey` y se añadió la propiedad `TranslationKey` en:
    *   `AttendanceStatusEntity`
    *   `EducationalLevelTypeEntity`
    *   `PaymentConceptTypeEntity`
    *   `PaymentMethodEntity`
*   **Configuración:** Se configuró `MaxLength(100)` para `TranslationKey` en sus respectivas clases de configuración.
*   **Migración:** Se generó `AddTranslationKeyToEnums`.

### M. Incorporación de GradingScaleType y EnrollmentStatus al Seeding
*   **Objetivo:** Convertir `GradingScaleType` y `EnrollmentStatus` en tablas de catálogo sincronizadas.
*   **Implementación:**
    *   Se crearon `GradingScaleTypeEntity` y `EnrollmentStatusEntity` en el dominio, implementando `ITranslationKey`.
    *   Se crearon `GradingScaleTypeConfiguration` y `EnrollmentStatusConfiguration` en la infraestructura.
    *   Se actualizaron `GradingScaleConfiguration` y `EnrollmentConfiguration` para establecer las relaciones `HasOne` con `DeleteBehavior.Restrict`.
    *   Se generó la migración `AddGradingScaleTypeAndEnrollmentStatus`.
*   **Seeders:** Se crearon `GradingScaleTypeSeeder` y `EnrollmentStatusSeeder`.

### N. Tablas de Idiomas y Traducciones
*   **Objetivo:** Soportar la funcionalidad multiidioma de `TranslationKey`.
*   **Implementación:**
    *   **`LanguageEntity`:** Entidad para gestionar idiomas (`Id`, `Name`, `Code`, `IsActive`).
    *   **`LanguageConfiguration`:** Configuración de EF Core para `LanguageEntity`, incluyendo `HasData` para idiomas iniciales ("en", "es").
    *   **`TranslationEntity`:** Entidad para almacenar las traducciones (`Id`, `Key`, `LanguageId`, `Value`).
    *   **`TranslationConfiguration`:** Configuración de EF Core para `TranslationEntity`, con un índice único compuesto en `(Key, LanguageId)` y sin propiedades de navegación.
    *   Se generó la migración `AddLanguageAndTranslationTables`.
*   **Corrección:** Se restauró la configuración de la relación FK en `TranslationConfiguration.cs` para `LanguageId` (sin propiedad de navegación).
*   **Corrección:** Se eliminó un comentario en línea inapropiado en `LanguageEntity.cs`.

### O. Actualización de AcademicYearEntity con Status (Enum)
Se modificó `AcademicYearEntity` para utilizar un `enum` para el estado del período lectivo, reemplazando la propiedad `IsActive`.

*   **Entidades y Enums Creados:**
    *   `source/Desyco.Dms.Domain/AcademicYears/AcademicYearStatus.cs`: Enum con `Upcoming`, `Current`, `Closed`.
    *   `source/Desyco.Dms.Domain/AcademicYears/AcademicYearStatusEntity.cs`: Entidad de catálogo para el enum, implementando `ITranslationKey`.
*   **Modificaciones de Entidades y DTOs:**
    *   `source/Desyco.Dms.Domain/AcademicYears/AcademicYearEntity.cs`: Se eliminó `IsActive` y se añadió `Status` (tipo `AcademicYearStatus`) con un valor por defecto de `Upcoming`.
    *   `source/Desyco.Dms.Application/AcademicYears/DTOs/AcademicYearDto.cs`: Se actualizó el DTO para reemplazar `IsActive` con `Status`.
*   **Configuración de Infraestructura:**
    *   `source/Desyco.Dms.Infrastructure/AcademicYears/AcademicYearStatusConfiguration.cs`: Configuración de EF Core para `AcademicYearStatusEntity`.
    *   `source/Desyco.Dms.Infrastructure/AcademicYears/AcademicYearConfiguration.cs`: Se añadió la relación `HasOne` para la nueva propiedad `Status`.
*   **Seeding:**
    *   `source/Desyco.Dms.Infrastructure/Seeders/AcademicYearStatusSeeder.cs`: Se creó el seeder para poblar la tabla `AcademicYearStatus`.
*   **Migración:**
    *   Se generó y aplicó la migración `AddAcademicYearStatus`. La migración fue modificada manualmente para asegurar la inserción de los valores del `enum` en la tabla `AcademicYearStatus` antes de añadir la columna `Status` y la llave foránea en `AcademicYear`, con un valor por defecto de `Upcoming` (1) para las filas existentes.

### P. Implementación de AcademicYear en ClientApp (Frontend)
Se implementó la gestión de Años Académicos en el frontend (`ClientApp`).

*   **Store (Pinia):**
    *   Se creó `source/Desyco.Dms.Web/ClientApp/src/stores/academicYearStore.ts` para manejar el estado (`academicYears`, `loading`, `totalRecords`) y las acciones (CRUD) utilizando `AcademicYearService`.
*   **Componente (Vue):**
    *   Se reescribió `source/Desyco.Dms.Web/ClientApp/src/views/school/years-days-times/AcademicYear.vue`.
    *   **UI:** Se implementó un `DataTable` con paginación "lazy" (server-side), ordenamiento y filtrado simple. Se añadieron diálogos para creación/edición y confirmación de borrado.
    *   **Estilos:** Se utilizaron clases de **Tailwind CSS** para el layout, reemplazando PrimeFlex.
    *   **Integración:** Se conectó con el store y se manejaron los estados de carga y notificaciones (`Toast`).
*   **Verificación:**
    *   Se confirmó que `AcademicYearService.ts` y los tipos en `types/academic-year.ts` estaban alineados con el backend.
    *   Se verificó la ruta de la API (`/api/v1/academic-years`), la cual coincide con la configuración de `axios` y el controlador versionado.

### Q. Configuración de Proxy en Frontend
Se configuró el proxy de desarrollo en Vite para redirigir las peticiones de API al backend local.
*   **Archivo:** `source/Desyco.Dms.Web/ClientApp/vite.config.ts`.
*   **Configuración:**
    *   Puerto del servidor de desarrollo: `5173`.
    *   Proxy `/api`: Redirige a `https://localhost:5001` (Backend).
    *   Proxy `/scalar`: Redirige a `https://localhost:5001` (Documentación API).
    *   Opciones: `changeOrigin: true`, `secure: false` (para aceptar certificados auto-firmados de desarrollo).

### R. Corrección en Modelo de Datos Frontend
Se corrigió un error de mapeo en la respuesta de la API que impedía cargar el DataTable.
*   **Problema:** La interfaz `QueryResult<T>` definía la propiedad como `list`, pero la API retornaba `results`.
*   **Corrección:**
    *   Se actualizó `source/Desyco.Dms.Web/ClientApp/src/types/common.ts`: `list` -> `results`.
    *   Se actualizó `source/Desyco.Dms.Web/ClientApp/src/stores/academicYearStore.ts` para consumir `response.data.results`.

### S. Mejora de UX/UI en AcademicYear.vue
Se optimizó el diseño de la vista de gestión de Años Académicos para ser más práctico y visualmente limpio.
*   **Cabecera Unificada:** Se eliminó el componente `Toolbar` externo.
*   **Acciones Integradas:** El título, el buscador y el botón "New" (ahora primario) se integraron en una sola fila dentro del encabezado de la tabla (`DataTable #header`).
*   **Estilos:** Se aplicaron clases de utilidad de Tailwind (`flex`, `gap`, `w-full`) para asegurar un diseño responsivo y campos de formulario que ocupan todo el ancho en los diálogos modales.

### T. Implementación de Breadcrumbs
Se añadió un sistema de breadcrumbs dinámico para mejorar la navegación del usuario.
*   **Configuración de Rutas:** Se modificó `source/Desyco.Dms.Web/ClientApp/src/router/index.ts` para añadir la propiedad `meta: { breadcrumb: '...' }` a las rutas relevantes (`/school-settings`, `/school-settings/academic-year`), permitiendo al sistema identificar los nombres de los elementos en el breadcrumb.
*   **Composable `useBreadcrumbs`:** Se creó `source/Desyco.Dms.Web/ClientApp/src/layout/composables/useBreadcrumbs.ts`. Este composable escucha los cambios de ruta y genera dinámicamente un array de elementos de breadcrumb basados en la información `meta` de las rutas.
*   **Integración en Layout:** Se añadió el componente `<Breadcrumb :home="home" :model="items" class="mb-4" />` a `source/Desyco.Dms.Web/ClientApp/src/layout/AppLayout.vue`, asegurando que aparezca de forma consistente en la parte superior de la sección de contenido de cada página.

### U. Integración Profunda de OData en Frontend
Se implementó una arquitectura robusta para manejar consultas OData desde la UI.
*   **Utilidades:**
    *   `useDataTableUtils.ts`: Composable que centraliza la conversión de eventos de PrimeVue (`page`, `sort`, `filter`) a un objeto `RequestParamsPayload`.
    *   `useDebounce.ts`: Composable para mejorar el rendimiento de la búsqueda global.
    *   `QueryStringBuilder.ts`: Utilidad para transformar `RequestParamsPayload` en cadenas de consulta OData válidas.
*   **Refactorización de `AcademicYear.vue`:**
    *   Se delegó la lógica de paginación y filtrado a `useDataTableUtils`.
    *   Se eliminó la construcción manual de query strings.
    *   Se implementó `useDebounce` para la búsqueda.
*   **Actualización de Capa de Servicio:**
    *   `AcademicYearService.ts` ahora acepta `RequestParamsPayload` y utiliza `QueryStringBuilder` para construir la URL.
    *   `academicYearStore.ts` se actualizó para manejar los tipos correctos.

### V. Mejoras Adicionales de UX y Robustez en `AcademicYear.vue`
Se aplicaron las siguientes sugerencias para mejorar la experiencia de usuario y la calidad del código:
*   **Feedback Visual:** Se agregó un estado de carga (`isSaving`) a los botones de "Guardar" y "Eliminar" en los diálogos para indicar al usuario que una operación está en curso.
*   **Manejo de Errores Mejorado:** Los mensajes de error ahora intentan extraer información más detallada de la respuesta de la API (`error.response?.data?.message`) para ofrecer un feedback más útil al usuario.
*   **Manejo de Fechas:** Se refinó la lógica de conversión de fechas en los formularios. Ahora los objetos `Date` se utilizan para el `v-model` del componente `Calendar` y se transforman a strings ISO (YYYY-MM-DD) solo al enviar los datos al backend, mitigando posibles problemas de formato o zona horaria.
*   **Diálogo de Confirmación Estándar:** Se refactorizó la confirmación de eliminación para utilizar el `ConfirmationService` y el componente `<ConfirmDialog>` de PrimeVue, eliminando un diálogo manual y estandarizando la experiencia.

### W. Corrección de Estilo y Linting
Se ejecutó el comando de linting y se resolvieron los errores detectados para asegurar la calidad del código.
*   **Configuración ESLint:** Se instaló `@vue/eslint-config-typescript` y se actualizó `.eslintrc.cjs` para soportar correctamente el análisis de archivos TypeScript.
*   **Correcciones:**
    *   `AppLayout.vue`: Se corrigió una advertencia de variable no utilizada asegurando el uso correcto del composable `useBreadcrumbs`.
    *   `useBreadcrumbs.ts`: Se eliminaron importaciones y variables no utilizadas (`router`).
    *   `router/index.ts`: Se cambió la declaración de `router` de `let` a `const`.
*   **Formato:** Se aplicaron correcciones automáticas de estilo (`--fix`) en todo el frontend.

### X. Simplificación de Tipos y Refactorización Final
Se realizó una refactorización importante para simplificar el manejo de tipos en el frontend.
*   **Unificación de DTOs:** Se consolidaron las interfaces `AcademicYearDto`, `CreateAcademicYearDto` y `UpdateAcademicYearDto` en una única interfaz `AcademicYearDto` (con `id` opcional). Esto reduce la duplicación de código y facilita el mantenimiento.
*   **Actualización de Capas:** Se actualizaron `AcademicYearService.ts`, `academicYearStore.ts` y `AcademicYear.vue` para utilizar el tipo unificado.
*   **Corrección de Errores:** Se solucionó un error de sintaxis crítico en `academicYearStore.ts` causado por contenido duplicado durante la edición anterior.
*   **Verificación:** Se confirmó que el proyecto pasa todas las reglas de linting sin errores.

### Y. Transición a Page-Driven UI y Mejoras de Formulario
Se completó la refactorización del módulo `AcademicYear` cambiando de un enfoque basado en modales (Dialog-driven) a páginas dedicadas (Page-driven), mejorando la usabilidad y navegabilidad.

*   **Nuevos Componentes:**
    *   `AcademicYearForm.vue`: Componente reutilizable con diseño horizontal, validación visual de campos requeridos y manejo tipado de fechas.
    *   `AcademicYearCreate.vue`: Vista envolvente para la creación.
    *   `AcademicYearEdit.vue`: Vista envolvente para la edición (carga de datos por ID).
*   **Refactorización de Lista:** `AcademicYear.vue` se simplificó para ser una vista de solo lectura con navegación y eliminación.
*   **Rutas:** Se actualizaron las rutas en `router/index.ts` para soportar `/create` y `/:id/edit`.
*   **UX/UI:**
    *   Alineación a la derecha de botones y cabecera en la columna de acciones.
    *   Indicadores visuales (*) para campos obligatorios.
    *   Manejo robusto de tipos de fecha (Date vs string) para compatibilidad con PrimeVue Calendar.

### Z. Script de Formato con Prettier
Se añadió un script `format` en `package.json` para ejecutar Prettier, permitiendo un formato de código consistente en todo el proyecto.

### AA. Reversión de Transiciones de Ruta
Se intentó implementar transiciones `fade-in` en `AppLayout.vue` y `SchoolSettings.vue`. Sin embargo, debido a problemas con la renderización de rutas anidadas (vistas desapareciendo), se decidió revertir estos cambios para mantener la estabilidad de la navegación mientras se investiga una solución más robusta.

### AB. Optimización de Búsqueda (Debounce)
Se implementó una lógica de debounce reutilizable para mejorar el rendimiento de la búsqueda en listas.
*   **Utilidad `useDebounce.ts`:** Se refactorizó para exportar una función de utilidad estándar que envuelve callbacks con un temporizador (`setTimeout`), simplificando su uso y eliminando dependencias reactivas complejas.
*   **Integración en `AcademicYear.vue`:** Se aplicó esta utilidad al campo de búsqueda global, asegurando que las peticiones a la API solo se realicen después de que el usuario deje de escribir, manteniendo el código del componente limpio.

### AC. Corrección de Validación de Fechas en Frontend (Yup)
*   **Problema:** La validación de Yup no marcaba como requeridos los campos de fecha (`startDate`, `endDate`) cuando el valor era `null`.
*   **Solución:** Se modificó el esquema de validación en `source/Desyco.Dms.Web/ClientApp/src/views/school/years-days-times/AcademicYearForm.vue`.
    *   Se eliminó `.nullable()` de los campos de fecha.
    *   Se añadió `.typeError('Start/End Date is required')` para capturar explícitamente el caso de valor `null` como error de tipo y mostrar el mensaje adecuado.

### AD. Reestructuración de Carpetas Frontend (Academic Years)
*   **Cambio:** Se movieron las vistas relacionadas con `AcademicYear` a una carpeta dedicada para mejorar la organización.
*   **Ubicación Antigua:** `source/Desyco.Dms.Web/ClientApp/src/views/school/years-days-times/`
*   **Ubicación Nueva:** `source/Desyco.Dms.Web/ClientApp/src/views/school/academic-years/`
*   **Archivos Movidos:** `AcademicYear.vue`, `AcademicYearCreate.vue`, `AcademicYearEdit.vue`, `AcademicYearForm.vue`.
*   **Actualizaciones:** Se actualizaron las importaciones en `router/index.ts` para reflejar la nueva ruta.

### AE. Refactorización de Rutas Frontend (Módulos)
*   **Objetivo:** Evitar que `router/index.ts` crezca descontroladamente y mejorar la mantenibilidad.
*   **Acción:** Se extrajeron las rutas de `SchoolSettings` a su propio archivo de módulo.
*   **Nuevo Archivo:** `source/Desyco.Dms.Web/ClientApp/src/router/modules/school.ts`.
*   **Actualización:** `router/index.ts` ahora importa y propaga (`...schoolRoutes`) las rutas desde el módulo.

### AF. Implementación de Lazy Loading en Rutas
*   **Objetivo:** Mejorar el rendimiento de carga inicial de la aplicación, evitando cargar componentes de rutas que no son visitadas.
*   **Acción:** Se modificó `source/Desyco.Dms.Web/ClientApp/src/router/modules/school.ts` para que `AcademicYear.vue` se cargue de forma perezosa (`lazy loading`) en lugar de estáticamente.
*   **Cambio:** La importación estática de `AcademicYear.vue` fue removida, y su uso en la definición de la ruta `academic-year-list` ahora utiliza una función `import()` dinámica.

### AG. Corrección de Fechas en Calendar (Zona Horaria)
*   **Problema:** Al editar un año académico, el componente `Calendar` de PrimeVue no reconocía correctamente la fecha seleccionada en el popup, debido a problemas de conversión `string` -> `Date` y desplazamientos de zona horaria (UTC vs Local).
*   **Solución:** Se implementó una función auxiliar `parseLocalDate` en `AcademicYearForm.vue`.
    *   Esta función detecta cadenas formato `YYYY-MM-DD` y construye el objeto `Date` usando el constructor local `new Date(year, monthIndex, day)`, evitando la conversión automática a UTC que restaba un día.
    *   Se eliminó la inicialización directa con `...props.initialData` en `useForm` para garantizar que todos los datos pasen por la lógica de transformación del `watch`.

### AH. Actualización de Componente PrimeVue Calendar a DatePicker
*   **Problema:** El componente `Calendar` de PrimeVue está deprecated a partir de la versión 4.
*   **Solución:** Se reemplazó el uso de `<Calendar>` por `<DatePicker>` en `source/Desyco.Dms.Web/ClientApp/src/views/school/academic-years/AcademicYearForm.vue` para los campos `startDate` y `endDate`. Esto alinea la aplicación con las últimas directrices de PrimeVue v4.

### AI. Corrección de Sincronización DatePicker (Proxies)
*   **Problema:** El componente `DatePicker` de PrimeVue no mostraba la fecha seleccionada en el popup si el valor subyacente (`v-model`) era un `string` (ej. "2025-01-01"), aunque el input mostrara el texto correctamente.
*   **Solución:**
    *   Se crearon propiedades computadas (`startDateProxy`, `endDateProxy`) en `AcademicYearForm.vue`.
    *   El `get` de estos proxies utiliza `parseLocalDate` para garantizar que el `DatePicker` siempre reciba un objeto `Date` real, independientemente de cómo `vee-validate` almacene el estado interno.
    *   El `set` actualiza directamente el campo de `vee-validate`.
    *   Esto asegura una sincronización bidireccional robusta entre el formulario y el componente visual.

### AJ. Modificación de Componente FormField (Hint Text)
*   **Objetivo:** Permitir la inclusión de un texto explicativo opcional (hint) en los campos de formulario para mejorar la usabilidad.
*   **Acción:** Se modificó `source/Desyco.Dms.Web/ClientApp/src/components/common/FormField.vue`.
    *   Se añadió una nueva prop opcional `hint?: string;` a `defineProps`.
    *   Se agregó un `<small v-if="hint" ... >` en el template para renderizar el texto de ayuda debajo del campo de entrada y antes del mensaje de error, utilizando la clase `text-surface-500` para su estilo.

## 3. Instrucciones para la Próxima Sesión
1.  **Continuar con Controllers:** Generar los controladores restantes siguiendo el patrón de `AcademicYearsController` (Versionado + Scrima).

## 4. Rutas Clave
*   **Web/API:** `source/Desyco.Dms.Web`
*   **Domain Entities:** `source/Desyco.Dms.Domain`
*   **Infrastructure Config:** `source/Desyco.Dms.Infrastructure` (Especialmente las clases `*Configuration.cs`).
*   **Modules Autofac:** 
    *   `source/Desyco.Dms.Application/ApplicationModule.cs`
    *   `source/Desyco.Dms.Infrastructure/InfrastructureModule.cs`