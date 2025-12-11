# Plan de Implementación: Sistema de Permisos Granulares (ACL)

Este plan detalla la evolución del sistema de identidad para soportar permisos granulares por funcionalidad (Feature) y acción (Read, Write, Delete), gestionables desde la UI.

## Objetivo
Permitir la gestión dinámica de permisos mediante una tabla `Feature` (catálogo de pantallas/módulos) vinculada a los `Claims` de usuarios y roles, soportando descripciones multi-idioma y ordenamiento para la UI.

## Estructura de Datos Propuesta

### 1. Entidades Nuevas y Modificadas

#### A. Enum `PermissionOperator` (o `PermissionAction`)
Define el tipo de acceso atómico.
*   `Read`
*   `Write` (Create/Update)
*   `Delete`
*   (Opcional) `Approve`, `Export`, etc., si se requiere extender a futuro.

#### B. Entidad `Feature` (Nueva Tabla)
Representa un módulo, pantalla o recurso del sistema protegido.
*   `Id` (Guid, PK)
*   `Code` (string, Unique): Código interno usado en el backend (ej. `AcademicYears`).
*   `Description` (string): TranslationKey o texto descriptivo.
*   `Group` (string): Para agrupar en la UI (ej. "Configuración Académica").
*   `Order` (int): Para ordenar secuencialmente en la UI.
*   (Auditoría): `CreatedAt`, `CreatedBy`, etc.

#### C. `ApplicationRoleClaim` (Modificación)
Extiende `IdentityRoleClaim<Guid>`.
*   `FeatureId` (Guid?, FK -> Feature): Vínculo opcional. Si es null, es un claim estándar.
*   `PermissionType` (Enum?): Read/Write/Delete.
*   `Description` (string): Explicación del permiso (si no está vinculado a Feature).

#### D. `ApplicationUserClaim` (Modificación)
Extiende `IdentityUserClaim<Guid>`.
*   `FeatureId` (Guid?, FK -> Feature).
*   `PermissionType` (Enum?).
*   `Description` (string).

#### E. `ApplicationRole` (Modificación)
*   `Description` (string): Descripción del rol para la UI.

## Fases de Implementación

### Fase 1: Dominio e Infraestructura
1.  **Crear Enum:** `PermissionAction`.
2.  **Crear Entidad:** `FeatureEntity` en `Desyco.Iam.Infrastructure`.
3.  **Actualizar Entidades:** Modificar `ApplicationRole`, `ApplicationRoleClaim`, `ApplicationUserClaim` para añadir las nuevas propiedades y FKs.
4.  **DbContext:**
    *   Registrar `DbSet<FeatureEntity>`.
    *   Configurar relaciones (FKs) en `OnModelCreating`.
5.  **Migración:** Generar y aplicar `AddGranularPermissions`.

### Fase 2: Seeding (Sembrado de Datos)
1.  **FeatureSeeder:** Servicio que escanea el sistema o usa una lista predefinida para asegurar que todos los módulos (ej. `AcademicYears`) existan en la tabla `Feature` con su grupo y orden correctos.

### Fase 3: Gestión de Permisos (Lógica)
1.  **PermissionService:**
    *   `GetAllFeaturesWithPermissions()`: Retorna la estructura jerárquica para la UI (Feature -> Actions).
    *   `UpdateRolePermissions(roleId, permissions[])`: Actualiza la tabla `RoleClaims` basándose en la selección de la UI.

### Fase 4: Autorización (Runtime)
1.  **Policy Provider Dinámico:** (Opcional pero recomendado)
    *   Permite usar atributos como `[Authorize(Policy = "Permissions.AcademicYears.Read")]`.
    *   El sistema verifica automáticamente si el usuario tiene el Claim correspondiente (FeatureCode="AcademicYears" + Permission=Read).
2.  **Optimización JWT:**
    *   Evaluar si incluir todos los claims en el token o cargarlos en memoria/caché al validar el request para no saturar el token.

## Consideraciones
*   **Multi-idioma:** Las descripciones se tratarán como claves de traducción. El frontend será responsable de traducirlas.
*   **Compatibilidad:** Los claims "estándar" (no vinculados a Features) seguirán funcionando como pares Key-Value normales (FeatureId = null).
