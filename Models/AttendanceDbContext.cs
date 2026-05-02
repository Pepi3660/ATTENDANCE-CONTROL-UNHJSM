using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

public partial class AttendanceDbContext : DbContext
{
    public AttendanceDbContext()
    {
    }

    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatArea> CatAreas { get; set; }

    public virtual DbSet<CatAttendanceStatus> CatAttendanceStatuses { get; set; }

    public virtual DbSet<CatBuilding> CatBuildings { get; set; }

    public virtual DbSet<CatCareer> CatCareers { get; set; }

    public virtual DbSet<CatClassStatus> CatClassStatuses { get; set; }

    public virtual DbSet<CatClassroom> CatClassrooms { get; set; }

    public virtual DbSet<CatDocumentType> CatDocumentTypes { get; set; }

    public virtual DbSet<CatGroup> CatGroups { get; set; }

    public virtual DbSet<CatRegisterMethod> CatRegisterMethods { get; set; }

    public virtual DbSet<CatRole> CatRoles { get; set; }

    public virtual DbSet<CatShift> CatShifts { get; set; }

    public virtual DbSet<CatStudent> CatStudents { get; set; }

    public virtual DbSet<CatSubject> CatSubjects { get; set; }

    public virtual DbSet<CatTeacher> CatTeachers { get; set; }

    public virtual DbSet<CatTelephoneCompany> CatTelephoneCompanies { get; set; }

    public virtual DbSet<CatUser> CatUsers { get; set; }

    public virtual DbSet<CatUserProfile> CatUserProfiles { get; set; }

    public virtual DbSet<TblAttendance> TblAttendances { get; set; }

    public virtual DbSet<TblClass> TblClasses { get; set; }

    public virtual DbSet<TblClassEnrollment> TblClassEnrollments { get; set; }

    public virtual DbSet<TblDocumentStudent> TblDocumentStudents { get; set; }

    public virtual DbSet<TblDocumentTeacher> TblDocumentTeachers { get; set; }

    public virtual DbSet<TblEmailStudent> TblEmailStudents { get; set; }

    public virtual DbSet<TblEmailTeacher> TblEmailTeachers { get; set; }

    public virtual DbSet<TblPhoneStudent> TblPhoneStudents { get; set; }

    public virtual DbSet<TblPhoneTeacher> TblPhoneTeachers { get; set; }

    public virtual DbSet<TblStudentGroup> TblStudentGroups { get; set; }

    public virtual DbSet<TblUserAccessLog> TblUserAccessLogs { get; set; }

    public virtual DbSet<TblUserRole> TblUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=Your_Server;Database=Your_Database;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatArea>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK__Cat_Area__425676CEC794553A");

            entity.Property(e => e.AreaId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatAttendanceStatus>(entity =>
        {
            entity.HasKey(e => e.AttendanceStatusId).HasName("PK__Cat_Atte__1DBB8C0712F73E11");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatBuilding>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Cat_Buil__D6D8520AB12653F6");

            entity.Property(e => e.BuildingId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Area).WithMany(p => p.CatBuildings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cat_Build__Area___636EBA21");
        });

        modelBuilder.Entity<CatCareer>(entity =>
        {
            entity.HasKey(e => e.CareerId).HasName("PK__Cat_Care__F4FCE655F9EB57EB");

            entity.Property(e => e.CareerId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatClassStatus>(entity =>
        {
            entity.HasKey(e => e.ClassStatusId).HasName("PK__Cat_Clas__433C28962B685A3E");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatClassroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PK__Cat_Clas__783A000FBABA7E9B");

            entity.Property(e => e.ClassroomId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Building).WithMany(p => p.CatClassrooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cat_Class__Build__68336F3E");
        });

        modelBuilder.Entity<CatDocumentType>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("PK__Cat_Docu__0AD24C61D0C4902A");

            entity.Property(e => e.DocumentTypeId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Cat_Grou__319812099031B1EB");

            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Career).WithMany(p => p.CatGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cat_Group__Caree__6CF8245B");

            entity.HasOne(d => d.Shift).WithMany(p => p.CatGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cat_Group__Shift__6DEC4894");
        });

        modelBuilder.Entity<CatRegisterMethod>(entity =>
        {
            entity.HasKey(e => e.RegisterMethodId).HasName("PK__Cat_Regi__04822DCD5932021B");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatRole>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Cat_Role__34ADFA60F7EC7604");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatShift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__Cat_Shif__527AD6977A5F4E82");

            entity.Property(e => e.ShiftId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatStudent>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Cat_Stud__A2F4E98C14790B37");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Gender).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatSubject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Cat_Subj__D98F54B607BA5803");

            entity.Property(e => e.SubjectId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatTeacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Cat_Teac__92FF4CEBEF79C808");

            entity.Property(e => e.TeacherId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatTelephoneCompany>(entity =>
        {
            entity.HasKey(e => e.IdTelephoneCompany).HasName("PK__Cat_Tele__B85E2C5A5A50E486");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CatUser>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Cat_User__D03DEDCBAAA7B18F");

            entity.Property(e => e.IdUser).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Teacher).WithMany(p => p.CatUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cat_User__Teache__4E3E9311");
        });

        modelBuilder.Entity<CatUserProfile>(entity =>
        {
            entity.HasKey(e => e.IdUserProfile).HasName("PK__Cat_User__7970A39EC09ECC34");

            entity.Property(e => e.IdUserProfile).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.CatUserProfiles).HasConstraintName("FK__Cat_User___Id_Us__5303482E");
        });

        modelBuilder.Entity<TblAttendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Tbl_Atte__57FA49142AB47C28");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RegisterTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AttendanceStatus).WithMany(p => p.TblAttendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Atten__Atten__3FF073BA");

            entity.HasOne(d => d.Class).WithMany(p => p.TblAttendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Atten__Class__3E082B48");

            entity.HasOne(d => d.RegisterMethod).WithMany(p => p.TblAttendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Atten__Regis__40E497F3");

            entity.HasOne(d => d.Student).WithMany(p => p.TblAttendances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Atten__Stude__3EFC4F81");
        });

        modelBuilder.Entity<TblClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Tbl_Clas__B0970537EB01268C");

            entity.Property(e => e.AllowSelfRegister).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ClassStatus).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Class__30AE302A");

            entity.HasOne(d => d.Classroom).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Class__2FBA0BF1");

            entity.HasOne(d => d.Group).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Group__2DD1C37F");

            entity.HasOne(d => d.Subject).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Subje__2EC5E7B8");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblClasses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Teach__2CDD9F46");
        });

        modelBuilder.Entity<TblClassEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Tbl_Clas__4365BD4AC0FDBA95");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Class).WithMany(p => p.TblClassEnrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Class__6339AFF7");

            entity.HasOne(d => d.Student).WithMany(p => p.TblClassEnrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Class__Stude__642DD430");
        });

        modelBuilder.Entity<TblDocumentStudent>(entity =>
        {
            entity.HasKey(e => e.IdDocumentStudent).HasName("PK__Tbl_Docu__8DC53D15F1AA095E");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.TblDocumentStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Docum__Docum__7F16D496");

            entity.HasOne(d => d.Student).WithMany(p => p.TblDocumentStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Docum__Stude__7E22B05D");
        });

        modelBuilder.Entity<TblDocumentTeacher>(entity =>
        {
            entity.HasKey(e => e.IdDocumentStudent).HasName("PK__Tbl_Docu__8DC53D15A6F199CF");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.TblDocumentTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Docum__Docum__18D6A699");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblDocumentTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Docum__Teach__17E28260");
        });

        modelBuilder.Entity<TblEmailStudent>(entity =>
        {
            entity.HasKey(e => e.IdEmailStudent).HasName("PK__Tbl_Emai__CDB8D60B83E7BEB3");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Student).WithMany(p => p.TblEmailStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Email__Stude__09946309");
        });

        modelBuilder.Entity<TblEmailTeacher>(entity =>
        {
            entity.HasKey(e => e.IdEmailTeacher).HasName("PK__Tbl_Emai__65A838EC78772251");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblEmailTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Email__Teach__2354350C");
        });

        modelBuilder.Entity<TblPhoneStudent>(entity =>
        {
            entity.HasKey(e => e.IdPhoneStudent).HasName("PK__Tbl_Phon__C16DC4B7630ABA89");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdTelephoneCompanyNavigation).WithMany(p => p.TblPhoneStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Phone__Id_Te__04CFADEC");

            entity.HasOne(d => d.Student).WithMany(p => p.TblPhoneStudents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Phone__Stude__03DB89B3");
        });

        modelBuilder.Entity<TblPhoneTeacher>(entity =>
        {
            entity.HasKey(e => e.IdPhoneTeacher).HasName("PK__Tbl_Phon__FFD5786773CE2A1E");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdTelephoneCompanyNavigation).WithMany(p => p.TblPhoneTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Phone__Id_Te__1E8F7FEF");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TblPhoneTeachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Phone__Teach__1D9B5BB6");
        });

        modelBuilder.Entity<TblStudentGroup>(entity =>
        {
            entity.HasKey(e => e.IdStudentGroup).HasName("PK__Tbl_Stud__7184ADCF2753BDF1");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Group).WithMany(p => p.TblStudentGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Stude__Group__0F4D3C5F");

            entity.HasOne(d => d.Student).WithMany(p => p.TblStudentGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_Stude__Stude__0E591826");
        });

        modelBuilder.Entity<TblUserAccessLog>(entity =>
        {
            entity.HasKey(e => e.IdAccessLog).HasName("PK__Tbl_User__36189A3C6895D997");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblUserAccessLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_User___Id_Us__5D80D6A1");
        });

        modelBuilder.Entity<TblUserRole>(entity =>
        {
            entity.HasKey(e => e.IdUserRole).HasName("PK__Tbl_User__EB0AD5136DFC2FBD");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TblUserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_User___Id_Ro__58BC2184");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TblUserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tbl_User___Id_Us__57C7FD4B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
