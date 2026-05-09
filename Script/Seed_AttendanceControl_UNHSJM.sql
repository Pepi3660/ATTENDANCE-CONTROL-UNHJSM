/* ============================================================
   SEED DATA - ATTENDANCE CONTROL SYSTEM UNHSJM
   Compatible with ATTENDANCE_CONTROL_DB.sql
   Uso recomendado: ejecutar después de crear la BD y tablas.
   Contraseñas de prueba:
     admin      / admin123
     mvictor    / teacher123
     lvalerio   / teacher123
     hlopez     / teacher123
============================================================ */

USE ATTENDANCE_CONTROL_DB;
GO


/* ========================================================
	CATÁLOGOS BASE
======================================================== */

IF NOT EXISTS (SELECT 1 FROM Cat_Career WHERE Career_Id = 1)
	INSERT INTO Cat_Career (Career_Id, Career_Name, IsActive, CreatedDate)
	VALUES (1, 'Ingeniería en Sistemas de Información', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Career WHERE Career_Id = 2)
	INSERT INTO Cat_Career (Career_Id, Career_Name, IsActive, CreatedDate)
	VALUES (2, 'Ingeniería en Computación', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Career WHERE Career_Id = 3)
	INSERT INTO Cat_Career (Career_Id, Career_Name, IsActive, CreatedDate)
	VALUES (3, 'Ingeniería en Biotecnología', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Shift WHERE Shift_Id = 1)
	INSERT INTO Cat_Shift (Shift_Id, Shift_Name, IsActive, CreatedDate)
	VALUES (1, 'Regular', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Shift WHERE Shift_Id = 1)
	INSERT INTO Cat_Shift (Shift_Id, Shift_Name, IsActive, CreatedDate)
	VALUES (2, 'Nocturno', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Shift WHERE Shift_Id = 3)
	INSERT INTO Cat_Shift (Shift_Id, Shift_Name, IsActive, CreatedDate)
	VALUES (3, 'Sabatino', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Shift WHERE Shift_Id = 4)
	INSERT INTO Cat_Shift (Shift_Id, Shift_Name, IsActive, CreatedDate)
	VALUES (4, 'Por Encuentro', 1, GETDATE());
-----------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 1)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (1, 'Matemática Introductoria', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 2)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (2, 'Técnicas y Estrategias de Comunicación', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 3)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (3, 'Fundamentos de Tecnologías y Sistemas', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 4)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (4, 'Filosofía', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 5)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (5, 'Inglés Técnico', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 6)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (6, 'Historia de Nicaragua', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 7)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (7, 'Curso Libre de Integración', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 8)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (8, 'Composición y Redacción Técnica', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 9)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (9, 'Cálculo I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 10)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (10, 'Introducción a la Electrónica', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 11)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (11, 'Curso Libre General y Humanístico', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 12)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (12, 'Fundamentos de Programación', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 13)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (13, 'Cálculo II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 14)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (14, 'Desarrollo de Aplicaciones I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 15)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (15, 'Diseño de Bases de Datos', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 16)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (16, 'Estadística General', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 17)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (17, 'Sistemas Electrónicos', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 18)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (18, 'Género en la Vida Cotidiana', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 19)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (19, 'Programación de Bases de Datos', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 20)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (20, 'Desarrollo de Aplicaciones II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 21)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (21, 'Arquitectura de Computadoras', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 22)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (22, 'Ingeniería Económica', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 23)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (23, 'Administración de Bases de Datos', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 24)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (24, 'Prácticas de Familiarización', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 25)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (25, 'Principios de Administración', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 26)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (26, 'Sistemas Operativos', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 27)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (27, 'Ingeniería del Software I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 28)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (28, 'Métodos y Técnicas de Investigación', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 29)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (29, 'Telecomunicaciones y Redes I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 30)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (30, 'Cátedras Tecnológicas II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 31)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (31, 'Ingeniería del Software II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 32)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (32, 'Desarrollo de Aplicaciones III', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 33)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (33, 'Metodología de la Investigación', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 34)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (34, 'Gerencia con Liderazgo', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 35)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (35, 'Administración de Sistemas de Información', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 36)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (36, 'Desarrollo de Aplicaciones IV', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 37)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (37, 'Telecomunicaciones y Redes II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 38)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (38, 'Cultura de Paz', 1, GETDATE());
	
IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 39)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (39, 'Ingeniería del Software III', 1, GETDATE());
	
IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 40)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (40, 'Desarrollo de Aplicaciones V', 1, GETDATE());
	
IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 41)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (41, 'Configuración y Administración de Servidores I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 42)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (42, 'Alfabetización Tecnológica', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 43)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (43, 'Seguridad de Tecnologías', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 44)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (44, 'Gerencia de Tecnologías', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 45)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (45, 'Curso Libre Profesional I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 46)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (46, 'Auditoría de Tecnologías', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 47)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (47, 'Curso Libre Profesional II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 48)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (48, 'Configuración y Administración de Servidores II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 49)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (49, 'Optativa Profesional I', 1, GETDATE());
	
IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 50)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (50, 'Optativa Profesional II', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 51)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (51, 'Optativa Profesional III', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 52)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (52, 'Curso Libre Profesional III', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 53)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (53, 'Prácticas Profesionales', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Subject WHERE Subject_Id = 54)
	INSERT INTO Cat_Subject (Subject_Id, Subject_Name, IsActive, CreatedDate)
	VALUES (54, 'Formas de Culminación de Estudios', 1, GETDATE());

------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Area WHERE Area_Id = 1)
	INSERT INTO Cat_Area (Area_Id, Area_Name, Initial, IsActive, CreatedDate)
	VALUES (1, 'Dirección de Ciencias Básicas y Tecnología', 'DCByT', 1, GETDATE());

------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 1)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (1, 1, 'Edificio A', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 2)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (2, 1, 'Edificio B', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 3)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (3, 1, 'Edificio C', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 4)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (4, 1, 'Edificio D', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 5)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (5, 1, 'Edificio E', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 6)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (6, 1, 'Edificio F', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 7)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (7, 1, 'Edificio G', 1, GETDATE());
		
IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 8)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (8, 1, 'Edificio H', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 9)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (9, 1, 'Edificio I', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 10)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (10, 1, 'Edificio J', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Building WHERE Building_Id = 11)
    INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name, IsActive, CreatedDate)
    VALUES (11, 1, 'Edificio K', 1, GETDATE());


----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Classroom WHERE Classroom_Id = 1)
    INSERT INTO Cat_Classroom (Classroom_Id, Building_Id, Classroom_Name, Capacity, IsActive, CreatedDate)
    VALUES (1, 1, 'A-1', 35, 1, GETDATE());


IF NOT EXISTS (SELECT 1 FROM Cat_Classroom WHERE Classroom_Id = 2)
    INSERT INTO Cat_Classroom (Classroom_Id, Building_Id, Classroom_Name, Capacity, IsActive, CreatedDate)
    VALUES (2, 1, 'A-2', 35, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Classroom WHERE Classroom_Id = 3)
    INSERT INTO Cat_Classroom (Classroom_Id, Building_Id, Classroom_Name, Capacity, IsActive, CreatedDate)
    VALUES (3, 1, 'Laboratorio A', 35, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Classroom WHERE Classroom_Id = 4)
    INSERT INTO Cat_Classroom (Classroom_Id, Building_Id, Classroom_Name, Capacity, IsActive, CreatedDate)
    VALUES (4, 1, 'Taller de Hardware', 35, 1, GETDATE());


----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_Group WHERE Group_Id = 1)
    INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name, IsActive, CreatedDate)
    VALUES (1, 1, 1, 'ISI-IA-MAT', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Group WHERE Group_Id = 2)
    INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name, IsActive, CreatedDate)
    VALUES (2, 1, 1, 'ISI-IIA-MAT', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Group WHERE Group_Id = 3)
    INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name, IsActive, CreatedDate)
    VALUES (3, 1, 1, 'ISI-IIIA-MAT', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Group WHERE Group_Id = 4)
    INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name, IsActive, CreatedDate)
    VALUES (4, 1, 1, 'ISI-IVA-MAT', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Group WHERE Group_Id = 5)
    INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name, IsActive, CreatedDate)
    VALUES (5, 1, 1, 'ISI-VA-MAT', 1, GETDATE());
   
----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_Telephone_Company WHERE Company = 'Claro')
    INSERT INTO Cat_Telephone_Company (Company, IsActive, CreatedDate)
    VALUES ('Claro', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Telephone_Company WHERE Company = 'Tigo')
    INSERT INTO Cat_Telephone_Company (Company, IsActive, CreatedDate)
    VALUES ('Tigo', 1, GETDATE());

   
----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_Document_Type WHERE Document_Type_Id = 1)
    INSERT INTO Cat_Document_Type (Document_Type_Id, Document_Type, IsActive, CreatedDate)
    VALUES (1, 'Carnet', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Document_Type WHERE Document_Type_Id = 2)
    INSERT INTO Cat_Document_Type (Document_Type_Id, Document_Type, IsActive, CreatedDate)
    VALUES (2, 'Cédula', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Class_Status WHERE Class_Status_Name = 'Open')
    INSERT INTO Cat_Class_Status (Class_Status_Name, IsActive, CreatedDate)
    VALUES ('Open', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Class_Status WHERE Class_Status_Name = 'Closed')
    INSERT INTO Cat_Class_Status (Class_Status_Name, IsActive, CreatedDate)
    VALUES ('Closed', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Class_Status WHERE Class_Status_Name = 'Cancelled')
    INSERT INTO Cat_Class_Status (Class_Status_Name, IsActive, CreatedDate)
    VALUES ('Cancelled', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Attendance_Status WHERE Attendance_Status_Name = 'Present')
    INSERT INTO Cat_Attendance_Status (Attendance_Status_Name, IsActive, CreatedDate)
    VALUES ('Present', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Attendance_Status WHERE Attendance_Status_Name = 'Late')
    INSERT INTO Cat_Attendance_Status (Attendance_Status_Name, IsActive, CreatedDate)
    VALUES ('Late', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Attendance_Status WHERE Attendance_Status_Name = 'Absent')
    INSERT INTO Cat_Attendance_Status (Attendance_Status_Name, IsActive, CreatedDate)
    VALUES ('Absent', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Register_Method WHERE Register_Method_Name = 'Manual')
    INSERT INTO Cat_Register_Method (Register_Method_Name, IsActive, CreatedDate)
    VALUES ('Manual', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Register_Method WHERE Register_Method_Name = 'Link')
    INSERT INTO Cat_Register_Method (Register_Method_Name, IsActive, CreatedDate)
    VALUES ('Link', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Register_Method WHERE Register_Method_Name = 'QR')
    INSERT INTO Cat_Register_Method (Register_Method_Name, IsActive, CreatedDate)
    VALUES ('QR', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Cat_Role WHERE Role_Name = 'ADMIN')
    INSERT INTO Cat_Role (Role_Name, Role_Description, IsActive, CreatedDate)
    VALUES ('ADMIN', 'Administrador del sistema', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Role WHERE Role_Name = 'TEACHER')
    INSERT INTO Cat_Role (Role_Name, Role_Description, IsActive, CreatedDate)
    VALUES ('TEACHER', 'Docente del sistema', 1, GETDATE());

----------------------------------------------------------------------------------------------------
    /* ========================================================
       DOCENTES Y USUARIOS
    ======================================================== */

----------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 1)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (1, 'Gladys', NULL, 'Aguilar', 'Flores', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 2)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (2, 'Arlen', NULL, 'Gutiérrez', 'Zambrana', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 3)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (3, 'Sonia', NULL, 'Guillen', 'Avendaño', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 4)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (4, 'Xiomara', NULL, 'Cantillano', 'Saballos', 1, GETDATE());

----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 5)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (5, 'Tania', NULL, 'Sequeira', 'Altamirano', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 6)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (6, 'Marjorie', NULL, 'Victor', 'Rugama', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 7)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (7, 'Danilo', NULL, 'Rodríguez', 'Guerrero', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 8)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (8, 'Soraya', NULL, 'Gutiérrez', 'Toruña', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id = 9)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (9, 'Yamil', NULL, 'Durán', 'Sanabria', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_Teacher WHERE Teacher_Id =10)
    INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName, IsActive, CreatedDate)
    VALUES (10, 'Harold', NULL, 'Gonzalez', 'Villareyna', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 1 AND Email = 'gaguilar@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (1, 'gaguilar@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 2 AND Email = 'agutierrez@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (2, 'agutierrez@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 3 AND Email = 'sguillen@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (3, 'sguillen@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 4 AND Email = 'xcantillano@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (4, 'xcantillano@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 5 AND Email = 'tsequeira@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (5, 'tsequeira@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 6 AND Email = 'mvictor@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (6, 'mvictor@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 7 AND Email = 'drodriguez@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (7, 'drodriguez@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 8 AND Email = 'sgutierrez@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (8, 'sgutierrez@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 9 AND Email = 'yduran@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (9, 'yduran@docente.unp.edu.ni', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Teacher WHERE Teacher_Id = 10 AND Email = 'hgonzalez@docente.unp.edu.ni')
    INSERT INTO Tbl_Email_Teacher (Teacher_Id, Email, IsActive, CreatedDate) VALUES (10, 'hgonzalez@docente.unp.edu.ni', 1, GETDATE());


----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Tbl_Phone_Teacher WHERE Teacher_Id = 4)
    INSERT INTO Tbl_Phone_Teacher (Teacher_Id, Id_Telephone_Company, Phone, IsActive, CreatedDate) VALUES (1, 1, '', 1, GETDATE());

----------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM Tbl_Document_Teacher WHERE Teacher_Id = 1 AND Document_Type_Id = 2)
    INSERT INTO Tbl_Document_Teacher (Teacher_Id, Document_Type_Id, Document, IsActive, CreatedDate) VALUES (1, 1, '', 1, GETDATE());

----------------------------------------------------------------------------------------------------


IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 1)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (1, 1, 'gaguilar', 'gaguilar@docente.unp.edu.ni', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 2)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (2, 2, 'agutierrez', 'agutierrez@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 3)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (3, 3, 'sguillen', 'sguillen@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 4)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (4, 4, 'xcantillano', 'xcantillano@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 5)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (5, 5, 'tsequeira', 'tsequeira@docente.unp.edu.ni', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 6)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (6, 6, 'mvictor', 'mvictor@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 7)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (7, 7, 'drodriguez', 'drodriguez@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 8)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (8, 8, 'sgutierrez', 'sgutierrez@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 9)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (9, 9, 'yduran', 'yduran@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM Cat_User WHERE Id_User = 10)
    INSERT INTO Cat_User (Id_User, Teacher_Id, UserName, Email, Password_Hash, Last_Login, IsActive, CreatedDate)
    VALUES (10, 10, 'hgonzalez', 'hgonzalez@docente.unp.edu.ni', 'cde383eee8ee7a4400adf7a15f716f179a2eb97646b37e089eb8d6d04e663416', NULL, 1, GETDATE());


----------------------------------------------------------------------------------------------------

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 1 AND Id_Role = 1)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (1, 1, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 2 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (2, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 3 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (3, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 4 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (4, 2, 1, GETDATE());

	IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 5 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (5, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 6 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (6, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 7 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (7, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 8 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (8, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 9 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (9, 2, 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_User_Role WHERE Id_User = 10 AND Id_Role = 2)
        INSERT INTO Tbl_User_Role (Id_User, Id_Role, IsActive, CreatedDate) VALUES (10, 2, 1, GETDATE());


----------------------------------------------------------------------------------------------------

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 1)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (1, 1, 'Administrador', 'Sistema', NULL, 1, GETDATE());

		SELECT * FROM Cat_Teacher
    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 2)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (2, 2, 'Arlen', 'Gutiérrez', '82758330', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 3)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (3, 3, 'Sonia', 'Guillen', '82758330', 1, GETDATE());

   IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 4)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (4, 4, 'Xiomara', 'Cantillano', '82758330', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 5)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (5, 5, 'Tania', 'Sequeira', '82758330', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 6)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (6, 6, 'Marjorie', 'Victor', '82758330', 1, GETDATE());

   IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 7)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (7, 7, 'Danilo', 'Rodríguez', '82758330', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 8)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (8, 8, 'Soraya', 'Gutiérrez', '82758330', 1, GETDATE());

   IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 9)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (9, 9, 'Yamil', 'Durán', '82758330', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Cat_User_Profile WHERE Id_User_Profile = 10)
        INSERT INTO Cat_User_Profile (Id_User_Profile, Id_User, First_Name, Last_Name, Phone, IsActive, CreatedDate)
        VALUES (10, 10, 'Harold', 'Gonzalez', '82758330', 1, GETDATE());


    /* ========================================================
       ESTUDIANTES
    ======================================================== */

    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 1)
        INSERT INTO Cat_Student VALUES (1, 'Axel', 'Eduardo', 'Gallo', 'Aguilar', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 2)
        INSERT INTO Cat_Student VALUES (2, 'Jarod', 'Enrique', 'Hidalgo', 'Toval', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 3)
        INSERT INTO Cat_Student VALUES (3, 'Norma', 'Lucila', 'Arauz', 'Delgado', 'F', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 4)
        INSERT INTO Cat_Student VALUES (4, 'Ricardo', 'Antonio', 'Álvarez', 'Montenegro', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 5)
        INSERT INTO Cat_Student VALUES (5, 'Kevin', 'Antonio', 'Mendoza', 'Gutiérrez', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 6)
        INSERT INTO Cat_Student VALUES (6, 'Ana', 'Gabriela', 'López', 'Ruiz', 'F', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 7)
        INSERT INTO Cat_Student VALUES (7, 'Luis', 'Fernando', 'Martínez', 'Pérez', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 8)
        INSERT INTO Cat_Student VALUES (8, 'María', 'José', 'Castillo', 'Mairena', 'F', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 9)
        INSERT INTO Cat_Student VALUES (9, 'José', 'Daniel', 'Rodríguez', 'Torres', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 10)
        INSERT INTO Cat_Student VALUES (10, 'Sofía', 'Isabel', 'Hernández', 'García', 'F', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 11)
        INSERT INTO Cat_Student VALUES (11, 'Miguel', 'Ángel', 'Reyes', 'Sánchez', 'M', 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Cat_Student WHERE Student_Id = 12)
        INSERT INTO Cat_Student VALUES (12, 'Valeria', 'Alejandra', 'Flores', 'Luna', 'F', 1, GETDATE());

    DECLARE @i INT = 1;
    WHILE @i <= 12
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM Tbl_Document_Student WHERE Student_Id = @i AND Document_Type_Id = 1)
            INSERT INTO Tbl_Document_Student (Student_Id, Document_Type_Id, Document, IsActive, CreatedDate)
            VALUES (@i, 1, CAST(221490 + @i AS VARCHAR(20)), 1, GETDATE());

        IF NOT EXISTS (SELECT 1 FROM Tbl_Phone_Student WHERE Student_Id = @i)
            INSERT INTO Tbl_Phone_Student (Student_Id, Id_Telephone_Company, Phone, IsActive, CreatedDate)
            VALUES (@i, CASE WHEN @i % 2 = 0 THEN 1 ELSE 2 END, CAST(86000000 + @i AS VARCHAR(30)), 1, GETDATE());

        IF NOT EXISTS (SELECT 1 FROM Tbl_Email_Student WHERE Student_Id = @i)
            INSERT INTO Tbl_Email_Student (Student_Id, Email, IsActive, CreatedDate)
            VALUES (@i, CONCAT('estudiante', @i, '@estudiante.unp.edu.ni'), 1, GETDATE());

        SET @i += 1;
    END

	SELECT * FROM Tbl_Document_Student

    IF NOT EXISTS (SELECT 1 FROM Tbl_Document_Student WHERE Student_Id = 1 AND Document_Type_Id = 2)
        INSERT INTO Tbl_Document_Student (Student_Id, Document_Type_Id, Document, IsActive, CreatedDate)
        VALUES (1, 1, '', 1, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM Tbl_Document_Student WHERE Student_Id = 1 AND Document_Type_Id = 2)
        INSERT INTO Tbl_Document_Student (Student_Id, Document_Type_Id, Document, IsActive, CreatedDate)
        VALUES (1, 2, '', 1, GETDATE());

    -- Grupos oficiales
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 1 AND Group_Id = 1) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (1, 1, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 2 AND Group_Id = 2) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (2, 2, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 3 AND Group_Id = 3) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (3, 3, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 4 AND Group_Id = 4) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (4, 4, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 5 AND Group_Id = 5) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (5, 5, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 6 AND Group_Id = 1) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (6, 1, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 7 AND Group_Id = 2) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (7, 2, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 8 AND Group_Id = 3) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (8, 3, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 9 AND Group_Id = 4) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (9, 4, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 10 AND Group_Id = 5) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (10, 6, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 11 AND Group_Id = 1) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (11, 1, 1, GETDATE());
    IF NOT EXISTS (SELECT 1 FROM Tbl_Student_Group WHERE Student_Id = 12 AND Group_Id = 1) INSERT INTO Tbl_Student_Group (Student_Id, Group_Id, IsActive, CreatedDate) VALUES (12, 2, 1, GETDATE());

/* ============================================================
   VALIDACIONES RÁPIDAS
============================================================ */
SELECT 'Carreras' AS Catalogo, COUNT(*) AS Total FROM Cat_Career
UNION ALL SELECT 'Turnos', COUNT(*) FROM Cat_Shift
UNION ALL SELECT 'Asignaturas', COUNT(*) FROM Cat_Subject
UNION ALL SELECT 'Grupos', COUNT(*) FROM Cat_Group
UNION ALL SELECT 'Docentes', COUNT(*) FROM Cat_Teacher
UNION ALL SELECT 'Estudiantes', COUNT(*) FROM Cat_Student
UNION ALL SELECT 'Clases', COUNT(*) FROM Tbl_Class
UNION ALL SELECT 'Asistencias', COUNT(*) FROM Tbl_Attendance;

SELECT U.UserName, U.Email, R.Role_Name, U.IsActive
FROM Cat_User U
INNER JOIN Tbl_User_Role UR ON U.Id_User = UR.Id_User
INNER JOIN Cat_Role R ON UR.Id_Role = R.Id_Role
ORDER BY U.Id_User;

SELECT * FROM Cat_Teacher
SELECT * FROM Tbl_Document_Teacher
SELECT * FROM Tbl_Phone_Teacher
SELECT * FROM Tbl_Email_Teacher
SELECT * FROM Cat_User;
SELECT * FROM Tbl_User_Role;
SELECT * FROM Tbl_Class;
SELECT * FROM Tbl_Attendance
