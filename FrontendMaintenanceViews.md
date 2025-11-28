# Vistas de Mantenimiento para el Frontend del Sistema de Gestión Escolar

Basado en la estructura de entidades analizada y configurada, se propone una clasificación de las vistas de mantenimiento necesarias para el frontend. La clave está en diferenciar entre entidades **Maestras/Principales** (que requieren su propia pantalla de CRUD) y **Detalles/Dependientes** (que se gestionan dentro de la pantalla de otra entidad principal).

## Clasificación de Vistas de Mantenimiento

### 1. Configuración y Catálogos (Tablas Maestras)
Estas vistas son fundamentales para la configuración inicial del sistema y no cambian con frecuencia. Suelen ser interfaces simples con un listado (grid) y un modal o formulario de edición.

*   **Institucional:**
    *   **`AcademicYear` (Años Académicos):** Gestión de periodos escolares (fechas de inicio/fin, estado activo).
    *   **`Shift` (Turnos):** Definición de turnos (Mañana, Tarde, Noche) y sus posibles horarios.
    *   **`Classroom` (Aulas):** Gestión de espacios físicos (nombre, código, edificio, piso, capacidad).
    *   **`ClassroomType` (Tipos de Aula):** Catálogo de tipos de aula (ej. Laboratorio, Salón de clases, Biblioteca).
*   **Académico:**
    *   **`EducationalLevel` (Niveles Educativos):** Configuración de niveles (ej. Preescolar, Primaria, Secundaria).
    *   **`Grade` (Grados):** Definición de grados (ej. 1º, 2º, 3º) vinculados a un Nivel Educativo.
    *   **`SubjectArea` (Áreas de Materias):** Clasificación de materias (ej. Ciencias, Humanidades, Artes).
    *   **`Subject` (Materias):** Catálogo base de asignaturas ofrecidas.
    *   **`GradingScale` (Escalas de Calificación):** Definición de rangos y criterios de evaluación.
    *   **`ScaleLevel` (Niveles de Escala):** Gestión de los niveles específicos dentro de una escala de calificación (ej. "Sobresaliente", "Notable" para una escala cualitativa; o "A", "B", "C" para una cuantitativa). Se gestionan como detalle de `GradingScale`.
    *   **`EvaluationPeriod` (Períodos de Evaluación):** Configuración de los ciclos de evaluación (ej. Bimestre 1, Trimestre 2).
    *   **Estatus (`StudentStatus`, `AttendanceStatus`, `InvoiceStatus`):** Catálogos de estados para estudiantes, asistencia y facturación.
*   **Personas (Catálogos):**
    *   **`Relationship` (Parentescos):** Catálogo de tipos de parentesco (ej. Padre, Madre, Tutor Legal).
    *   **`Speciality` (Especialidades Docentes):** Catálogo de especialidades para los maestros (ej. Matemáticas, Biología, Inglés).
    *   **`PaymentMethod` (Métodos de Pago):** Catálogo de métodos de pago aceptados (ej. Efectivo, Tarjeta de Crédito, Transferencia Bancaria).
    *   **`PaymentConceptType` (Conceptos de Pago):** Catálogo de conceptos por los que se puede recibir un pago (ej. Colegiatura, Inscripción, Materiales).

### 2. Gestión de Personas (Vistas Transaccionales Complejas)
Estas vistas requieren formularios más elaborados, a menudo con múltiples secciones o pestañas, debido a la cantidad de información y sus relaciones.

*   **`Student` (Estudiantes):**
    *   **Vista Principal:** Gestión de datos personales del estudiante.
    *   **Sub-vista/Tab:** **`StudentGuardian`** (Tutores del Estudiante): Gestión de los tutores asociados, incluyendo su parentesco. *No se recomienda una vista CRUD separada para `StudentGuardian`, debe integrarse en la gestión del estudiante.*
    *   **Sub-vista/Tab:** Historial de Inscripciones y Becas.
*   **`Guardian` (Tutores/Familiares):** Gestión de la información de los responsables financieros y/o legales de los estudiantes.
*   **`Teacher` (Docentes):** Gestión de la información del personal docente.
    *   **Sub-vista/Tab:** Asignación de **`TeacherSpecialty`** (Especialidades Docentes).

### 3. Operativa Académica (El día a día de la gestión escolar)
Estas vistas gestionan las relaciones y procesos clave del sistema.

*   **`Section` (Secciones / Grupos):** Creación y gestión de los grupos de estudiantes para un Año Académico específico (ej. "1º Bachillerato - Grupo A - Turno Vespertino").
*   **`Enrollment` (Inscripciones):** Proceso para asignar un estudiante a una sección en un año académico dado.
*   **`TeacherAssignment` (Asignación Docente):** Pantalla para definir qué maestro imparte qué materia en qué sección.
*   **`ScholarshipType` (Tipos de Beca):** Catálogo de los tipos de becas disponibles (ej. Beca por Mérito, Beca Deportiva).
*   **`ScholarshipAward` (Asignación de Beca):** Pantalla para registrar qué beca se ha concedido a una inscripción específica de un estudiante.

### 4. Módulo Financiero
*   **`FeeConcept` (Conceptos de Cobro):** Gestión de los diferentes conceptos por los que se puede generar un cobro (ej. "Colegiatura Mensual", "Cuota de Inscripción").
*   **`Invoice` (Facturación):** Gestión completa de las facturas generadas.
    *   **Sub-vista/Detalle:** **`InvoiceDetail`** (Detalles de la Factura): Los ítems que componen una factura se gestionan directamente dentro de la interfaz de la factura, no como una vista CRUD independiente.
*   **`Payment` (Pagos):** Gestión y registro de los pagos recibidos.
    *   **Sub-vista/Detalle:** **`PaymentDetail`** (Detalles del Pago): Los detalles de un pago (montos, conceptos, a qué facturas aplica) se gestionan dentro de la interfaz del pago.
    *   **Sub-vista/Detalle:** **`PaymentMethodDetail`** (Detalles del Método de Pago): Información específica del método de pago (ej. número de referencia de transferencia).

### Entidades que **NO** deberían tener una vista de mantenimiento CRUD tradicional:

1.  **`ClassSchedule` (Horarios de Clase):** Su gestión no es un CRUD de registros individuales. Requiere una **interfaz de planificación visual** (tipo calendario o arrastrar y soltar) para asignar materias a horas y días en un aula.
2.  **`Attendance` (Asistencia):** No se gestiona registro por registro. Se necesita una **interfaz de "Toma de Asistencia"** donde se presenta una lista de estudiantes de una sección/materia y se marcan las presencias/ausencias.
3.  **`StudentGrade` (Calificaciones de Estudiante):** No se introduce individualmente. Se gestiona a través de una **interfaz de "Libreta de Calificaciones" (Gradebook)** por materia y sección, permitiendo la introducción masiva o contextualizada de notas.
4.  **`InvoiceDetail` y `PaymentDetail` / `PaymentMethodDetail`:** Como se mencionó, son detalles de sus entidades padres (`Invoice` y `Payment`) y deben gestionarse de forma integrada en los formularios de estas.
5.  **`DayOfWeekEntity`:** Es un catálogo muy estático, a menudo mapeando un `enum` a una descripción. Su mantenimiento directo por el usuario no suele ser necesario.
6.  **`StudentGuardianEntity` y `TeacherAssignmentEntity`:** Son tablas de unión que modelan relaciones "muchos a muchos" con posibles atributos adicionales. Su gestión debe integrarse en las pantallas de las entidades principales (`Student` y `Teacher` respectivamente) de forma intuitiva, no con un CRUD propio.

### Resumen de Prioridad para el Desarrollo del Frontend:

1.  **Fase 1 (Configuración Básica):** Años Académicos, Niveles Educativos, Grados, Turnos, Aulas, Tipos de Aula, Áreas de Materias, Parentescos, Especialidades Docentes.
2.  **Fase 2 (Gestión Principal):** Estudiantes, Tutores, Docentes, Secciones, Materias, Escalas de Calificación.
3.  **Fase 3 (Operativa y Transaccional):** Inscripciones, Asignación Docente, Tipos de Beca, Conceptos de Cobro, Facturación, Pagos.
4.  **Fase 4 (Interfaces Avanzadas):** Toma de Asistencia, Libreta de Calificaciones, Planificador de Horarios.
