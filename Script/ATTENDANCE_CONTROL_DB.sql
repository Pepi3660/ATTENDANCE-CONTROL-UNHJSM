-- ============================================================
-- DATABASE: ATTENDANCE CONTROL SYSTEM
-- DESCRIPTION:
-- Academic attendance management system for students, teachers,
-- groups, classrooms, schedules and attendance control.
-- SQL Server Script
-- ============================================================

CREATE DATABASE ATTENDANCE_CONTROL_DB;
GO

USE ATTENDANCE_CONTROL_DB;
GO

/* ============================================================
   DROP TABLES (EXECUTE ONLY IF NEEDED)
   Order matters because Foreign Keys exist
============================================================ */

IF OBJECT_ID('Tbl_Attendance', 'U') IS NOT NULL DROP TABLE Tbl_Attendance;
IF OBJECT_ID('Tbl_Class', 'U') IS NOT NULL DROP TABLE Tbl_Class;
IF OBJECT_ID('Tbl_Student_Group', 'U') IS NOT NULL DROP TABLE Tbl_Student_Group;
IF OBJECT_ID('Tbl_Document_Student', 'U') IS NOT NULL DROP TABLE Tbl_Document_Student;
IF OBJECT_ID('Tbl_Document_Teacher', 'U') IS NOT NULL DROP TABLE Tbl_Document_Teacher;
IF OBJECT_ID('Tbl_Email_Student', 'U') IS NOT NULL DROP TABLE Tbl_Email_Student;
IF OBJECT_ID('Tbl_Phone_Student', 'U') IS NOT NULL DROP TABLE Tbl_Phone_Student;
IF OBJECT_ID('Tbl_Email_Teacher', 'U') IS NOT NULL DROP TABLE Tbl_Email_Teacher;
IF OBJECT_ID('Tbl_Phone_Teacher', 'U') IS NOT NULL DROP TABLE Tbl_Phone_Teacher;
IF OBJECT_ID('Tbl_Class_Enrollment', 'U') IS NOT NULL DROP TABLE Tbl_Class_Enrollment;
IF OBJECT_ID('Tbl_User_Access_Log', 'U') IS NOT NULL DROP TABLE Tbl_User_Access_Log;
IF OBJECT_ID('Tbl_User_Role', 'U') IS NOT NULL DROP TABLE Tbl_User_Role;
IF OBJECT_ID('Tbl_User', 'U') IS NOT NULL DROP TABLE Tbl_User;
IF OBJECT_ID('Cat_Group', 'U') IS NOT NULL DROP TABLE Cat_Group;
IF OBJECT_ID('Cat_Classroom', 'U') IS NOT NULL DROP TABLE Cat_Classroom;
IF OBJECT_ID('Cat_Building', 'U') IS NOT NULL DROP TABLE Cat_Building;
IF OBJECT_ID('Cat_Teacher', 'U') IS NOT NULL DROP TABLE Cat_Teacher;
IF OBJECT_ID('Cat_Student', 'U') IS NOT NULL DROP TABLE Cat_Student;
IF OBJECT_ID('Cat_Subject', 'U') IS NOT NULL DROP TABLE Cat_Subject;
IF OBJECT_ID('Cat_Career', 'U') IS NOT NULL DROP TABLE Cat_Career;
IF OBJECT_ID('Cat_Shift', 'U') IS NOT NULL DROP TABLE Cat_Shift;
IF OBJECT_ID('Cat_Area', 'U') IS NOT NULL DROP TABLE Cat_Area;
IF OBJECT_ID('Cat_Document_Type', 'U') IS NOT NULL DROP TABLE Cat_Document_Type;
IF OBJECT_ID('Cat_Telephone_Company', 'U') IS NOT NULL DROP TABLE Cat_Telephone_Company;
IF OBJECT_ID('Cat_Attendance_Status', 'U') IS NOT NULL DROP TABLE Cat_Attendance_Status;
IF OBJECT_ID('Cat_Class_Status', 'U') IS NOT NULL DROP TABLE Cat_Class_Status;
IF OBJECT_ID('Cat_Register_Method', 'U') IS NOT NULL DROP TABLE Cat_Register_Method;
IF OBJECT_ID('Cat_User_Profile', 'U') IS NOT NULL DROP TABLE Cat_User_Profile;
IF OBJECT_ID('Cat_User_Role', 'U') IS NOT NULL DROP TABLE Cat_User_Role;
IF OBJECT_ID('Cat_Role', 'U') IS NOT NULL DROP TABLE Cat_Role;
IF OBJECT_ID('Cat_User', 'U') IS NOT NULL DROP TABLE Cat_User;
GO


/* ============================================================
   TABLE: Cat_Career
   DESCRIPTION:
   Academic careers / majors
============================================================ */
CREATE TABLE Cat_Career (
	Career_Id INT PRIMARY KEY NOT NULL,
	Career_Name VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Shift
   DESCRIPTION:
   Study shifts (Morning, Evening, Sunday, etc.)
============================================================ */
CREATE TABLE Cat_Shift (
	Shift_Id INT PRIMARY KEY NOT NULL,
	Shift_Name VARCHAR(50) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Subject
   DESCRIPTION:
   Subjects / courses
============================================================ */
CREATE TABLE Cat_Subject (
	Subject_Id INT PRIMARY KEY NOT NULL,
	Subject_Name VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Area
   DESCRIPTION:
   Campus zones / academic areas
============================================================ */
CREATE TABLE Cat_Area (
	Area_Id INT PRIMARY KEY NOT NULL,
	Area_Name VARCHAR(100) NOT NULL,
	Initial VARCHAR(20) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Building
   DESCRIPTION:
   Buildings inside campus
============================================================ */
CREATE TABLE Cat_Building (
	Building_Id INT PRIMARY KEY NOT NULL,
	Area_Id INT FOREIGN KEY REFERENCES Cat_Area(Area_Id) NOT NULL,
	Building_Name VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Classroom
   DESCRIPTION:
   Physical classrooms
============================================================ */
CREATE TABLE Cat_Classroom (
	Classroom_Id INT PRIMARY KEY NOT NULL,
	Building_Id INT FOREIGN KEY REFERENCES Cat_Building(Building_Id) NOT NULL,
	Classroom_Name VARCHAR(50) NOT NULL,
	Capacity INT,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Group
   DESCRIPTION:
   Academic groups (ISI-IVA-MAT)
============================================================ */
CREATE TABLE Cat_Group (
	Group_Id INT PRIMARY KEY NOT NULL,
	Career_Id INT FOREIGN KEY REFERENCES Cat_Career(Career_Id) NOT NULL,
	Shift_Id INT FOREIGN KEY REFERENCES Cat_Shift(Shift_Id) NOT NULL,
	Group_Name VARCHAR(50) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Cat_Telephone_Company
   DESCRIPTION:
   Mobile phone companies
============================================================ */
CREATE TABLE Cat_Telephone_Company (
	Id_Telephone_Company INT IDENTITY(1,1) PRIMARY KEY,
	Company VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Document_Type
   DESCRIPTION:
   Identification document types
============================================================ */
CREATE TABLE Cat_Document_Type (
	Document_Type_Id INT PRIMARY KEY NOT NULL,
	Document_Type VARCHAR(50) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Student
   DESCRIPTION:
   Student master table
============================================================ */
CREATE TABLE Cat_Student (
	Student_Id INT PRIMARY KEY NOT NULL,
	First_Name VARCHAR(30) NOT NULL,
	Second_Name VARCHAR(30),
	Last_Name VARCHAR(30) NOT NULL,
	Second_SurName VARCHAR(30),
	Gender CHAR(1) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Tbl_Document_Student
   DESCRIPTION:
   Identification document Student
============================================================ */
CREATE TABLE Tbl_Document_Student (
	Id_Document_Student INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
	Document_Type_Id INT FOREIGN KEY REFERENCES Cat_Document_Type(Document_Type_Id) NOT NULL, -- CED- --CARNE
	Document VARCHAR(20) NOT NULL, -- 221249 / 8880201011000L
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Tbl_Phone_Student
   DESCRIPTION:
   Student phones
============================================================ */
CREATE TABLE Tbl_Phone_Student (
	Id_Phone_Student INT IDENTITY(1,1) PRIMARY KEY,
	Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
	Id_Telephone_Company INT FOREIGN KEY REFERENCES Cat_Telephone_Company(Id_Telephone_Company) NOT NULL,
	Phone VARCHAR(30) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Tbl_Email_Student
   DESCRIPTION:
   Student emails
============================================================ */
CREATE TABLE Tbl_Email_Student (
	Id_Email_Student INT IDENTITY(1,1) PRIMARY KEY,
	Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Tbl_Student_Group
   DESCRIPTION:
   Student official academic group
============================================================ */
CREATE TABLE Tbl_Student_Group (
	Id_Student_Group INT IDENTITY(1,1) PRIMARY KEY,
	Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
	Group_Id INT FOREIGN KEY REFERENCES Cat_Group(Group_Id) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Cat_Teacher
   DESCRIPTION:
   Teacher master table
============================================================ */
CREATE TABLE Cat_Teacher (
	Teacher_Id INT PRIMARY KEY NOT NULL,
	First_Name VARCHAR(30) NOT NULL,
	Second_Name VARCHAR(30),
	Last_Name VARCHAR(30) NOT NULL,
	Second_SurName VARCHAR(30),
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Tbl_Document_Teacher
   DESCRIPTION:
   Identification document Teacher
============================================================ */
CREATE TABLE Tbl_Document_Teacher (
	Id_Document_Student INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Teacher_Id INT FOREIGN KEY REFERENCES Cat_Teacher(Teacher_Id) NOT NULL,
	Document_Type_Id INT FOREIGN KEY REFERENCES Cat_Document_Type(Document_Type_Id) NOT NULL, -- CED- --CARNE
	Document VARCHAR(20) NOT NULL, -- 221249 / 8880201011000L
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Tbl_Phone_Teacher
   DESCRIPTION:
   Student phones
============================================================ */
CREATE TABLE Tbl_Phone_Teacher (
	Id_Phone_Teacher INT IDENTITY(1,1) PRIMARY KEY,
	Teacher_Id INT FOREIGN KEY REFERENCES Cat_Teacher(Teacher_Id) NOT NULL,
	Id_Telephone_Company INT FOREIGN KEY REFERENCES Cat_Telephone_Company(Id_Telephone_Company) NOT NULL,
	Phone VARCHAR(30) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Tbl_Email_Teacher
   DESCRIPTION:
   Student emails
============================================================ */
CREATE TABLE Tbl_Email_Teacher (
	Id_Email_Teacher INT IDENTITY(1,1) PRIMARY KEY,
	Teacher_Id INT FOREIGN KEY REFERENCES Cat_Teacher(Teacher_Id) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Class_Status
   DESCRIPTION:
   Open / Closed / Cancelled
============================================================ */
CREATE TABLE Cat_Class_Status (
	Class_Status_Id INT IDENTITY(1,1) PRIMARY KEY,
	Class_Status_Name VARCHAR(30) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Tbl_Class
   DESCRIPTION:
   Real class session by date
============================================================ */
CREATE TABLE Tbl_Class (
	Class_Id INT IDENTITY(1,1) PRIMARY KEY,
	Class_Date DATE NOT NULL,
	Start_Time TIME,
	End_Time TIME,
	Token_Link VARCHAR(150),
	Open_DateTime DATETIME,
	Close_DateTime DATETIME,
	Allow_Self_Register BIT DEFAULT 1,
	Teacher_Id INT FOREIGN KEY REFERENCES Cat_Teacher(Teacher_Id) NOT NULL,
	Group_Id INT FOREIGN KEY REFERENCES Cat_Group(Group_Id)  NOT NULL,
	Subject_Id INT FOREIGN KEY REFERENCES Cat_Subject(Subject_Id) NOT NULL,
	Classroom_Id INT FOREIGN KEY REFERENCES Cat_Classroom(Classroom_Id) NOT NULL,
	Class_Status_Id INT FOREIGN KEY REFERENCES Cat_Class_Status(Class_Status_Id) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Attendance_Status
   DESCRIPTION:
   Present / Late / Absent
============================================================ */
CREATE TABLE Cat_Attendance_Status (
	Attendance_Status_Id INT IDENTITY(1,1) PRIMARY KEY,
	Attendance_Status_Name VARCHAR(30) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_Register_Method
   DESCRIPTION:
   Manual / Link / QR
============================================================ */
CREATE TABLE Cat_Register_Method (
	Register_Method_Id INT IDENTITY(1,1) PRIMARY KEY,
	Register_Method_Name VARCHAR(30) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Tbl_Attendance
   DESCRIPTION:
   Attendance records by class and student
============================================================ */
CREATE TABLE Tbl_Attendance (
	Attendance_Id INT IDENTITY(1,1) PRIMARY KEY,
	Class_Id INT FOREIGN KEY REFERENCES Tbl_Class(Class_Id) NOT NULL,
	Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
	Attendance_Status_Id INT FOREIGN KEY REFERENCES Cat_Attendance_Status(Attendance_Status_Id) NOT NULL,
	Register_Method_Id INT FOREIGN KEY REFERENCES Cat_Register_Method(Register_Method_Id) NOT NULL,
	Is_Justified BIT DEFAULT 0 NOT NULL,
	Observation VARCHAR(250),
	Register_Time DATETIME DEFAULT GETDATE() NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
	UNIQUE(Class_Id, Student_Id) --Para evitar doble asistencia.
);
GO


/* ============================================================
   TABLE: Tbl_Class_Enrollment
   DESCRIPTION:
   Students enrolled in a specific class/session group.
   Supports repeated subjects, summer courses and students
   taking subjects from other academic groups.
============================================================ */
CREATE TABLE Tbl_Class_Enrollment (
    Enrollment_Id INT IDENTITY(1,1) PRIMARY KEY,
    Class_Id INT FOREIGN KEY REFERENCES Tbl_Class(Class_Id) NOT NULL,
    Student_Id INT FOREIGN KEY REFERENCES Cat_Student(Student_Id) NOT NULL,
    IsActive BIT DEFAULT 1 NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
    CONSTRAINT UQ_Class_Enrollment UNIQUE (Class_Id, Student_Id)
);
GO

/* ============================================================
   TABLE: Cat_Role
   DESCRIPTION:
   System roles
============================================================ */
CREATE TABLE Cat_Role (
	Id_Role INT IDENTITY(1,1) PRIMARY KEY,
	Role_Name VARCHAR(50) NOT NULL UNIQUE,          -- ADMIN / TEACHER
	Role_Description VARCHAR(150),
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO


/* ============================================================
   TABLE: Cat_User
   DESCRIPTION:
   Login credentials
============================================================ */

CREATE TABLE Cat_User (
	Id_User INT PRIMARY KEY NOT NULL,
	Teacher_Id INT FOREIGN KEY REFERENCES Cat_Teacher(Teacher_Id) NOT NULL,
	UserName VARCHAR(50) NOT NULL UNIQUE,
	Email VARCHAR(150) NOT NULL UNIQUE,
	Password_Hash VARCHAR(255) NOT NULL,
	Last_Login DATETIME NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO



/* ============================================================
   TABLE: Cat_User_Profile
   DESCRIPTION:
   Personal data of user
============================================================ */
CREATE TABLE Cat_User_Profile (
	Id_User_Profile INT PRIMARY KEY NOT NULL,
	Id_User INT FOREIGN KEY (Id_User) REFERENCES Cat_User(Id_User),
	First_Name VARCHAR(100) NOT NULL,
	Last_Name VARCHAR(100) NOT NULL,
	Phone VARCHAR(20),
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO




/* ============================================================
   TABLE: Tbl_User_Role
   DESCRIPTION:
   Many-to-many user roles
============================================================ */
CREATE TABLE Tbl_User_Role (
	Id_User_Role INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Id_User INT FOREIGN KEY REFERENCES Cat_User(Id_User) NOT NULL,
	Id_Role INT FOREIGN KEY REFERENCES Cat_Role(Id_Role) NOT NULL,
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

/* ============================================================
   TABLE: Tbl_User_Access_Log
   DESCRIPTION:
   Login / logout / actions audit
============================================================ */
CREATE TABLE Tbl_User_Access_Log (
	Id_Access_Log INT IDENTITY(1,1) PRIMARY KEY,
	Id_User INT FOREIGN KEY REFERENCES Cat_User(Id_User) NOT NULL,
	Action_Name VARCHAR(100) NOT NULL,      -- LOGIN / LOGOUT / CREATE_CLASS
	Ip_Address VARCHAR(45),
	Device_Info VARCHAR(200),
	IsActive BIT DEFAULT 1 NOT NULL,
	CreatedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO
