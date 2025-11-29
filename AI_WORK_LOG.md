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

### D. Análisis de Frontend (Pendiente de implementación)
Se generó un documento (`FrontendMaintenanceViews.md`) clasificando las vistas necesarias en:
1.  **Catálogos Maestros:** (AcademicYear, Shift, Classroom, etc.) - Grids simples.
2.  **Gestión Compleja:** (Student, Teacher) - Formularios con pestañas/detalles.
3.  **Operativa:** (Enrollment, TeacherAssignment).
4.  **Financiero:** (Invoice, Payment).

## 3. Instrucciones para la Próxima Sesión
1.  **Verificar Migración:** Lo último realizado fue la configuración de Autofac y los arreglos de `DeleteBehavior`. El siguiente paso lógico es intentar ejecutar la migración (`dotnet ef migrations add ...`) para confirmar que el `DbContext` ya es válido.
2.  **Probar Endpoints:** Ejecutar la aplicación y probar el CRUD de `AcademicYears` para asegurar que Autofac está resolviendo correctamente el `Repository` y el `Mapper`.
3.  **Continuar con Controllers:** Generar los controladores restantes siguiendo el patrón de `AcademicYearsController`.

## 4. Rutas Clave
*   **Web/API:** `source/Desyco.Dms.Web`
*   **Domain Entities:** `source/Desyco.Dms.Domain`
*   **Infrastructure Config:** `source/Desyco.Dms.Infrastructure` (Especialmente las clases `*Configuration.cs`).
*   **Modules Autofac:** 
    *   `source/Desyco.Dms.Application/ApplicationModule.cs`
    *   `source/Desyco.Dms.Infrastructure/InfrastructureModule.cs`
