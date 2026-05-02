# Attendance Control System - UNHSJM

Sistema web para el control académico de asistencia de la Universidad Nacional Héroes San José de las Mulas.

## Descripción

Attendance Control System permite gestionar docentes, estudiantes, carreras, grupos, clases y asistencias académicas.  
El sistema permite a los docentes generar enlaces públicos de asistencia, registrar asistencia por carnet, cédula o celular, cerrar clases, marcar ausentes y generar reportes diarios o consolidados.

## Funcionalidades principales

- Gestión de estudiantes.
- Gestión de docentes.
- Gestión de usuarios por rol.
- Gestión de carreras.
- Gestión de grupos académicos.
- Gestión de clases.
- Registro público de asistencia por enlace.
- Validación de estudiantes activos.
- Control de presentes, tardíos y ausentes.
- Cierre de clase.
- Generación de QR para asistencia.
- Copia rápida de resumen de asistencia.
- Reporte diario en PDF y CSV.
- Reporte consolidado por período en PDF y CSV.
- Control de acceso por roles:
  - Administrador
  - Docente

## Roles del sistema

### Administrador

Puede administrar docentes, estudiantes, carreras, grupos, clases y reportes generales.

### Docente

Puede gestionar sus propias clases, controlar asistencia y generar reportes únicamente de sus clases.

## Tecnologías utilizadas

- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server
- Bootstrap
- QuestPDF
- JavaScript
- HTML/CSS

## Estructura general

```txt
Pages/
├── Attendance/
├── Careers/
├── Classes/
├── Groups/
├── Reports/
├── Students/
├── Teachers/
└── Shared/

Models/
wwwroot/