# Bitácora de Desarrollo - Proyecto SGD (Desyco.Dms)
**Última actualización:** 28 de Noviembre, 2025
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

## 3. Instrucciones para la Próxima Sesión
1.  **Ejecutar la Aplicación:** Iniciar el proyecto Web (`dotnet run --project source/Desyco.Dms.Web/Desyco.Dms.Web.csproj`).
2.  **Explorar Scalar UI:** Navegar a `/scalar/v1` para ver la documentación de la API versionada.
3.  **Probar Endpoints:** Verificar que la ruta `/api/v1/academic-years` funciona correctamente.
4.  **Continuar con Controllers:** Generar los controladores restantes siguiendo el patrón de `AcademicYearsController` (Versionado + Scrima).

## 4. Rutas Clave
*   **Web/API:** `source/Desyco.Dms.Web`
*   **Domain Entities:** `source/Desyco.Dms.Domain`
*   **Infrastructure Config:** `source/Desyco.Dms.Infrastructure` (Especialmente las clases `*Configuration.cs`).
*   **Modules Autofac:** 
    *   `source/Desyco.Dms.Application/ApplicationModule.cs`
    *   `source/Desyco.Dms.Infrastructure/InfrastructureModule.cs`
