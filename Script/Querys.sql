USE ATTENDANCE_CONTROL_DB;
GO

-- =========================
-- CARRERAS
-- =========================
INSERT INTO Cat_Career (Career_Id, Career_Name)
VALUES
(1, 'Ingeniería en Sistemas de Información'),
(2, 'Ingeniería en Computacion'),
(3, 'Ingeniería en Biotecnología');

-- =========================
-- TURNOS
-- =========================
INSERT INTO Cat_Shift (Shift_Id, Shift_Name)
VALUES
(1, 'Matutino'),
(2, 'Diurno'),
(3, 'Por Encuentro');

-- =========================
-- ASIGNATURAS
-- =========================
INSERT INTO Cat_Subject (Subject_Id, Subject_Name)
VALUES
(1, 'Introducción a la Programación'),
(2, 'Diseño de Base de Datos'),
(3, 'Programación de Base de Datos'),
(4, 'Administración de Base de Datos'),
(5, 'Desarrollo de Aplicaciones I'),
(6, 'Desarrollo de Aplicaciones II'),
(7, 'Desarrollo de Aplicaciones III'),
(8, 'Desarrollo de Aplicaciones IV'),
(9, 'Desarrollo de Aplicaciones V'),
(10, 'Ingeniería del Software I'),
(11, 'Ingeniería del Software II'),
(12, 'Ingeniería del Software III');

-- =========================
-- ÁREAS
-- =========================
INSERT INTO Cat_Area (Area_Id, Area_Name, Initial)
VALUES
(1, 'Dirección de Ciencias Básicas y Tecnología', 'DCByT');

-- =========================
-- EDIFICIOS
-- =========================
INSERT INTO Cat_Building (Building_Id, Area_Id, Building_Name)
VALUES
(1, 1, 'Edificio A'),
(2, 1, 'Edificio D'),
(3, 1, 'Edificio C');

-- =========================
-- AULAS
-- =========================
INSERT INTO Cat_Classroom (Classroom_Id, Building_Id, Classroom_Name, Capacity)
VALUES
(1, 1, 'A-2', 35),
(2, 2, 'D-2', 40),
(3, 2, 'Lab-1', 25);

-- =========================
-- GRUPOS
-- =========================
INSERT INTO Cat_Group (Group_Id, Career_Id, Shift_Id, Group_Name)
VALUES
(1, 1, 1, 'ISI-IB-MAT'),
(2, 1, 1, 'ISI-IIB-MAT'),
(3, 1, 1, 'ISI-IIIB-MAT'),
(4, 1, 1, 'ISI-IVB-MAT'),
(5, 1, 1, 'ISI-VB-MAT'),
(6, 1, 1, 'ISI-IA-MAT'),
(7, 1, 1, 'ISI-IIA-MAT'),
(8, 1, 1, 'ISI-IIIA-MAT'),
(9, 1, 1, 'ISI-IVA-MAT'),
(10, 1, 1, 'ISI-VA-MAT'),
(11, 1, 1, 'ISI-IB-DOM'),
(12, 1, 1, 'ISI-IIB-DOM'),
(13, 1, 1, 'ISI-IIIB-DOM'),
(14, 1, 1, 'ISI-IVB-DOM'),
(15, 1, 1, 'ISI-VB-DOM'),
(16, 1, 1, 'ISI-IA-DOM'),
(17, 1, 1, 'ISI-IIA-DOM'),
(18, 1, 1, 'ISI-IIIA-DOM'),
(19, 1, 1, 'ISI-IVA-DOM'),
(20, 1, 1, 'ISI-VA-DOM');

SELECT * FROM Cat_Group

-- =========================
-- TIPOS DE DOCUMENTO
-- =========================
INSERT INTO Cat_Document_Type (Document_Type_Id, Document_Type)
VALUES
(1, 'Carnet'),
(2, 'Cédula'),
(3, 'Código Persona');

-- =========================
-- COMPAÑÍAS TELEFÓNICAS
-- Esta tabla sí es IDENTITY, no pases ID manual.
-- =========================
INSERT INTO Cat_Telephone_Company (Company)
VALUES
('Claro'),
('Tigo');

-- =========================
-- DOCENTE
-- =========================
INSERT INTO Cat_Teacher (Teacher_Id, First_Name, Second_Name, Last_Name, Second_SurName)
VALUES
(1, 'Marjorie', NULL, 'Victor', 'Rugama'),
(2, 'Tania', NULL, 'Avendaño', NULL),
(3, 'Lesbia', 'Elena', 'Valerio', NULL),
(4, 'Evelyn', NULL, 'Espinoza', 'Aragón'),
(5, 'Harold', NULL, 'Lopez', NULL),
(6, 'Danilo', NULL, 'Gutierrez', NULL),
(7, 'Aldo', 'Mauricio', 'Gonzalez', 'Pitty');

SELECT * FROM Cat_Teacher

-- =========================
-- ESTADOS DE CLASE
-- Esta tabla sí es IDENTITY.
-- =========================
INSERT INTO Cat_Class_Status (Class_Status_Name)
VALUES
('Open'),
('Closed'),
('Cancelled');

-- =========================
-- ESTADOS DE ASISTENCIA
-- Esta tabla sí es IDENTITY.
-- =========================
INSERT INTO Cat_Attendance_Status (Attendance_Status_Name)
VALUES
('Present'),
('Late'),
('Absent');

-- =========================
-- MÉTODOS DE REGISTRO
-- Esta tabla sí es IDENTITY.
-- =========================
INSERT INTO Cat_Register_Method (Register_Method_Name)
VALUES
('Manual'),
('Link'),
('QR');

-- =========================
-- ROLES
-- Esta tabla sí es IDENTITY.
-- =========================
INSERT INTO Cat_Role (Role_Name, Role_Description) 
VALUES
('ADMIN', 'Administrador del sistema'),
('TEACHER', 'Docente que registra asistencia');
GO


INSERT INTO Cat_Student (Student_Id,First_Name,Second_Name, Last_Name,Second_SurName, Gender) 
VALUES 
(1, 'Ricardo', 'Antonio', 'Álvarez', 'Montenegro', 'M');


-- Carnet
INSERT INTO Tbl_Document_Student (Student_Id,Document_Type_Id,Document) 
VALUES (1, 1, '2212494');

-- Cédula
INSERT INTO Tbl_Document_Student (Student_Id,Document_Type_Id, Document) 
VALUES (1, 2, '8880201011000L');

-- Teléfono
INSERT INTO Tbl_Phone_Student (Student_Id,Id_Telephone_Company,Phone) 
VALUES (1, 1, '82758330');

-- Email opcional
INSERT INTO Tbl_Email_Student (Student_Id, Email) 
VALUES (1, 'ralvarez@estudiante.unp.edu.ni');

-- Grupo oficial
INSERT INTO Tbl_Student_Group (Student_Id,Group_Id) 
VALUES (1, 20);
GO


INSERT INTO Tbl_Class (
    Class_Date,
    Start_Time,
    End_Time,
    Token_Link,
    Open_DateTime,
    Close_DateTime,
    Allow_Self_Register,
    Teacher_Id,
    Group_Id,
    Subject_Id,
    Classroom_Id,
    Class_Status_Id
) VALUES (
    CAST(GETDATE() AS DATE),
    '08:00',
    '10:00',
    '123123123',
    DATEADD(MINUTE, -10, GETDATE()),
    DATEADD(HOUR, 3, GETDATE()),
    1,
    1,
    1,
    1,
    2,
    1
);
GO

SELECT * FROM Tbl_Class;
SELECT * FROM Tbl_Class_Enrollment;
SELECT * FROM Tbl_Attendance;



-- Password: admin123
-- SHA256:
-- 240be518fabd2724d0c8cd4185e240c5e3f5d76280f5d9e64ff0635c9650e9a9

INSERT INTO Cat_User (Id_User,Teacher_Id, UserName, Email, Password_Hash, IsActive, CreatedDate)
VALUES (1,1, 'admin','admin@test.com','240be518fabd2724d0c8cd4185e240c5e3f5d76280f5d9e64ff0635c9650e9a9',1,GETDATE());

INSERT INTO Tbl_User_Role (Id_User,Id_Role,IsActive,CreatedDate)
VALUES (1,1,1,GETDATE());

SELECT * FROM Cat_User;
SELECT * FROM Cat_Role;
SELECT * FROM Tbl_User_Role;

-- Password: admin123
-- SHA256:
-- 240be518fabd2724d0c8cd4185e240c5e3f5d76280f5d9e64ff0635c9650e9a9

SELECT 
    Id_User,
    UserName,
    Email,
    Password_Hash,
    LEN(Password_Hash) AS HashLength,
    IsActive
FROM Cat_User;

SELECT 
    R.Id_Role,
    R.Role_Name,
    UR.Id_User,
    UR.IsActive
FROM Tbl_User_Role UR
INNER JOIN Cat_Role R ON UR.Id_Role = R.Id_Role;

UPDATE Cat_User
SET Password_Hash = '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    IsActive = 1
WHERE UserName = 'admin';

SELECT 
    U.Id_User,
    U.UserName,
    U.Password_Hash,
    R.Role_Name,
    UR.IsActive
FROM Cat_User U
INNER JOIN Tbl_User_Role UR ON U.Id_User = UR.Id_User
INNER JOIN Cat_Role R ON UR.Id_Role = R.Id_Role
WHERE U.UserName = 'admin';


IF NOT EXISTS (SELECT 1 FROM Cat_Role WHERE Role_Name = 'TEACHER')
BEGIN
    INSERT INTO Cat_Role (Role_Name, Role_Description, IsActive, CreatedDate)
    VALUES ('TEACHER', 'Docente del sistema', 1, GETDATE());
END

SELECT * FROM Cat_Role


SELECT * FROM Tbl_Document_Teacher

SELECT * FROM Tbl_Email_Teacher

SELECT * FROM Cat_User

SELECT 
    r.session_id,
    r.status,
    r.blocking_session_id,
    r.wait_type,
    r.wait_time,
    r.command,
    t.text AS sql_text
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) t
WHERE r.database_id = DB_ID('ATTENDANCE_CONTROL_DB');

ALTER DATABASE ATTENDANCE_CONTROL_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
ALTER DATABASE ATTENDANCE_CONTROL_DB SET MULTI_USER;



SELECT * FROM Cat_Student

SELECT * FROM Cat_Group
