Plan de Trabajo: Módulo de Identidad (Desyco.Iam)

Este plan contempla la creación de 3 nuevos proyectos y su integración, aislados en su propio esquema de base de datos (dls) y contexto.

Fase 1: Estructura de Proyectos y Solución
Crearemos tres bibliotecas de clases bajo la carpeta source/ para mantener la arquitectura limpia:

1. `Desyco.Iam.Contracts` (Class Library)
    * Responsabilidad: Contendrá los DTOs (Data Transfer Objects) compartidos y constantes.
    * Contenido: LoginRequest, RegisterRequest, TokenResponse, UserDto.
2. `Desyco.Iam.Infrastructure` (Class Library)
    * Responsabilidad: Implementación de Entity Framework Core, Identity Stores y generación de Tokens.
    * Dependencias: Desyco.Iam.Contracts, Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer.
3. `Desyco.Iam.Web` (Class Library con FrameworkReference)
    * Responsabilidad: Controladores (API), Filtros y métodos de extensión para la inyección de dependencias (IServiceCollection).
    * Dependencias: Desyco.Iam.Infrastructure, Desyco.Iam.Contracts.

Fase 2: Definición de Entidades e Infraestructura (Infrastructure)
Implementación de las tablas personalizadas y el contexto de base de datos.

1. Entidades Custom (`Guid` como Key):
    * ApplicationUser : IdentityUser<Guid>
    * ApplicationRole : IdentityRole<Guid>
    * ApplicationUserClaim, ApplicationRoleClaim, ApplicationUserRole, etc.
2. DbContext (`IamDbContext`):
    * Heredará de IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ...>.
    * Schema: En OnModelCreating, se configurará el esquema por defecto a dls.
    * Tablas: Se renombrarán las tablas de Identity (ej. AspNetUsers -> dls.Users, AspNetRoles -> dls.Roles).
3. Migraciones Separadas:
    * Se configurará el HistoryRepository para usar una tabla de migraciones propia (ej. dls.__IamMigrationsHistory) para no interferir con el ApplicationDbContext
      principal.

Fase 3: Lógica de Autenticación y JWT
Implementación de los servicios de negocio.

1. `JwtTokenGenerator`: Servicio para crear tokens firmados con las Claims del usuario y Roles.
2. `IdentityService`: Facade que encapsula UserManager y SignInManager para manejar el registro y login, retornando resultados definidos en Contracts.

Fase 4: Capa de Presentación (Web)
Exposición de la funcionalidad vía API.

1. `AuthController`: Controlador API con endpoints:
    * POST /api/auth/login
    * POST /api/auth/register
2. Configuración Modular:
    * Crear método de extensión AddIamModule(this IServiceCollection services, IConfiguration config).
    * Este método registrará: IamDbContext (SQL), Identity Core, esquemas de Autenticación (JWT Bearer) y los servicios de dominio.

Fase 5: Integración
Conexión con la solución principal Desyco.Dms.

1. Agregar referencias de los nuevos proyectos a la solución .sln.
2. En Desyco.Dms.Web/Program.cs (o CompositionRoot), llamar a builder.Services.AddIamModule(...).
3. Generar la migración inicial: dotnet ef migrations add InitialIdentity -c IamDbContext.
4. Aplicar la migración a la base de datos.

