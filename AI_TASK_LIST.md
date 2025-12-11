# Lista de Tareas para Gemini:

Desarrolla un plan de trabajo para la implementacion una librería de autorizacion basado en jwt token y utiliznado IdentityCore para este proyecto.
El plan debe contemplar las siguientes caracteristicas:

1. Crea tres proyectos separados, Uno para manejar la parte de la infraestructura (EF), uno para los contratos(dtos)
  y por ultimo uno para los controllers y registro de configuraciones que se utilizaran en el proyecto web.
    * Conte
2. Deseo tener tablas customizadas para IdentityUser, IdentityRole, IdentityRoleClaim, IdentityUserClaim.
3. Las tablas creadas deben tener su propio esquema (dls) y tabla de migracion EF separada.
4. 

/**/

Pero si los necesito porque quiero manejar tres claims  principales por cada vista o pantalla (canWrite(create/update), canRead, canDelete. Tambien quiero tener una
entidad de base de datos (Feature) que me permita representar cada una de las vistas para que de esta manera proporcionar una interfaz al usuario que le permida
conceder permimos de una manera sencilla. la entidad debe tener un campo descripcion(para describir el modulo al que se quiere conceder los permisos), tres campos de
tipo PermissionType(uno para write, uno para read y otro para delete. la entidades userclaims y roleclaims tendran un campo del tipo PermissionType para poder
vincularlos con la tabla feature, Los demas claims si existen de concederan de la forma regular), un campo grupo que permitira agrupar los features, un campor orden
para forzar una secuencia unica en la UI. Las tabla useclaims, roleclaims, roles tendran su propia descripcion para indicar que hace cada claim o role, y para el caso
de los claims que no sean tengan un permissionType definido, se pueda indicar que hacen. La descripcion almacenará translationsKeys para soportar multi legunaje pero
en caso de no encontrar traduccion diponible mostran el texto tal y como esta almacenado. Crea un plan para implementar estos cambios, evalua los pros y cons, sugiere
mejores practicas y sugiere cualquier cambio que aporte mejora y escalabilidad.
