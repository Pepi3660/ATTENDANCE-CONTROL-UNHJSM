using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AttendanceControl.Web.Pages.Classes
{
    public class AttendanceListModel : PageModel
    {
        private readonly AttendanceDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AttendanceListModel(AttendanceDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public TblClass? ClassInfo { get; set; }
        public IList<TblAttendance> Attendances { get; set; } = new List<TblAttendance>();
        public IList<CatStudent> AbsentStudents { get; set; } = new List<CatStudent>();

        public int Total { get; set; }
        public int MaleTotal { get; set; }
        public int FemaleTotal { get; set; }

        public int TotalEnrolled { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent => TotalEnrolled - TotalPresent;

        public double AttendancePercentage =>
            TotalEnrolled == 0 ? 0 : (double)TotalPresent / TotalEnrolled * 100;

        public string? AttendanceUrl { get; set; }
        public string QuickSummaryText { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var loaded = await LoadPageDataAsync(id);

            if (!loaded)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostMarkAbsentsAsync(int id)
        {
            await MarkAbsentStudentsAsync(id);
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostCloseClassAsync(int id)
        {
            var classSession = await _context.TblClasses
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classSession == null)
                return NotFound();

            await MarkAbsentStudentsAsync(id);

            var closedStatus = await _context.CatClassStatuses
                .FirstOrDefaultAsync(s => s.ClassStatusName == "Closed");

            if (closedStatus != null)
                classSession.ClassStatusId = closedStatus.ClassStatusId;

            classSession.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }


        public async Task<IActionResult> OnGetExportCsvAsync(int id)
        {
            var loaded = await LoadPageDataAsync(id);

            if (!loaded || ClassInfo == null)
                return NotFound();

            var csv = new StringBuilder();

            csv.AppendLine("Reporte Diario de Asistencia");
            csv.AppendLine($"Asignatura,{EscapeCsv(ClassInfo.Subject.SubjectName)}");
            csv.AppendLine($"Docente,{EscapeCsv($"{ClassInfo.Teacher.FirstName} {ClassInfo.Teacher.LastName}")}");
            csv.AppendLine($"Fecha,{ClassInfo.ClassDate:dd/MM/yyyy}");
            csv.AppendLine($"Grupo,{EscapeCsv(ClassInfo.Group.GroupName)}");
            csv.AppendLine($"Turno,{EscapeCsv(ClassInfo.Group.Shift.ShiftName)}");
            csv.AppendLine($"Aula,{EscapeCsv(ClassInfo.Classroom.ClassroomName)}");
            csv.AppendLine($"Area,{EscapeCsv(ClassInfo.Classroom.Building.Area.Initial)}");
            csv.AppendLine($"Edificio,{EscapeCsv(ClassInfo.Classroom.Building.BuildingName)}");
            csv.AppendLine();

            csv.AppendLine("Carnet,Estudiante,Genero,Estado,Hora,Justificada,Observacion");

            foreach (var item in Attendances)
            {
                var carnet = GetStudentCarnet(item.Student);
                var fullName = $"{item.Student.FirstName} {item.Student.LastName}";
                var status = item.AttendanceStatus.AttendanceStatusName;
                var justified = item.IsJustified ? "Si" : "No";

                csv.AppendLine(
                    $"{EscapeCsv(carnet)}," +
                    $"{EscapeCsv(fullName)}," +
                    $"{EscapeCsv(item.Student.Gender)}," +
                    $"{EscapeCsv(status)}," +
                    $"{item.RegisterTime:HH:mm}," +
                    $"{justified}," +
                    $"{EscapeCsv(item.Observation ?? string.Empty)}"
                );
            }

            csv.AppendLine();
            csv.AppendLine("Resumen");
            csv.AppendLine($"Total inscritos,{TotalEnrolled}");
            csv.AppendLine($"Presentes/Tardes,{TotalPresent}");
            csv.AppendLine($"Ausentes,{TotalAbsent}");
            csv.AppendLine($"Masculino,{MaleTotal}");
            csv.AppendLine($"Femenino,{FemaleTotal}");
            csv.AppendLine($"Porcentaje asistencia,{AttendancePercentage:0.0}%");

            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();

            var fileName = $"asistencia_clase_{id}_{DateTime.Now:yyyyMMddHHmm}.csv";

            return File(bytes, "text/csv", fileName);
        }

        public async Task<IActionResult> OnGetExportPdfAsync(int id)
        {
            var loaded = await LoadPageDataAsync(id);

            if (!loaded || ClassInfo == null)
                return NotFound();

            QuestPDF.Settings.License = LicenseType.Community;

            var logoBytes = GetLogoBytes();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(28);
                    page.Size(PageSizes.A4.Landscape());

                    page.Header().Column(header =>
                    {
                        header.Item().Row(row =>
                        {
                            if (logoBytes != null)
                            {
                                row.ConstantItem(180)
                                    .Image(logoBytes)
                                    .FitWidth();
                            }

                            row.RelativeItem().AlignMiddle().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Reporte Diario de Asistencia")
                                    .Bold()
                                    .FontSize(16)
                                    .FontColor(Colors.Blue.Darken3);

                                col.Item().AlignCenter().Text("Universidad Nacional Héroes San José de las Mulas")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken2);

                                col.Item().AlignCenter().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });

                        header.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Blue.Darken3);
                    });

                    page.Content().PaddingVertical(10).Column(content =>
                    {
                        content.Item().Text("Datos generales de la clase")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            AddInfoRow(table, "Asignatura", ClassInfo.Subject.SubjectName, "Docente", $"{ClassInfo.Teacher.FirstName} {ClassInfo.Teacher.LastName}");
                            AddInfoRow(table, "Fecha", ClassInfo.ClassDate.ToString("dd/MM/yyyy"), "Horario", $"{ClassInfo.StartTime:HH\\:mm} - {ClassInfo.EndTime:HH\\:mm}");
                            AddInfoRow(table, "Grupo", ClassInfo.Group.GroupName, "Turno", ClassInfo.Group.Shift.ShiftName);
                            AddInfoRow(table, "Área", ClassInfo.Classroom.Building.Area.Initial, "Edificio / Aula", $"{ClassInfo.Classroom.Building.BuildingName} / {ClassInfo.Classroom.ClassroomName}");
                        });

                        content.Item().PaddingTop(12).Text("Resumen de asistencia")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Row(row =>
                        {
                            AddSummaryBox(row, "Total registrados", Total.ToString(), Colors.Blue.Darken2, Colors.White);
                            AddSummaryBox(row, "Masculino", MaleTotal.ToString(), Colors.Green.Darken2, Colors.White);
                            AddSummaryBox(row, "Femenino", FemaleTotal.ToString(), Colors.Orange.Lighten1, Colors.Black);
                            AddSummaryBox(row, "% Asistencia", $"{AttendancePercentage:0.0}%", Colors.Cyan.Darken2, Colors.White);
                            AddSummaryBox(row, "Ausentes", TotalAbsent.ToString(), Colors.Red.Darken2, Colors.White);
                        });

                        content.Item().PaddingTop(15).Text("Listado de estudiantes")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(2.4f);
                                columns.ConstantColumn(45);
                                columns.RelativeColumn(1);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(65);
                                columns.RelativeColumn(1.4f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("N°");
                                header.Cell().Element(HeaderCell).Text("Carnet");
                                header.Cell().Element(HeaderCell).Text("Estudiante");
                                header.Cell().Element(HeaderCell).Text("Género");
                                header.Cell().Element(HeaderCell).Text("Estado");
                                header.Cell().Element(HeaderCell).Text("Hora");
                                header.Cell().Element(HeaderCell).Text("Justificada");
                                header.Cell().Element(HeaderCell).Text("Observación");
                            });

                            var index = 1;

                            foreach (var item in Attendances)
                            {
                                var fullName = $"{item.Student.FirstName} {item.Student.LastName}";
                                var carnet = GetStudentCarnet(item.Student);

                                table.Cell().Element(BodyCell).Text(index.ToString());
                                table.Cell().Element(BodyCell).Text(carnet);
                                table.Cell().Element(BodyCell).Text(fullName);
                                table.Cell().Element(BodyCell).Text(item.Student.Gender);
                                table.Cell().Element(BodyCell).Text(item.AttendanceStatus.AttendanceStatusName);
                                table.Cell().Element(BodyCell).Text(item.RegisterTime.ToString("HH:mm"));
                                table.Cell().Element(BodyCell).Text(item.IsJustified ? "Sí" : "No");
                                table.Cell().Element(BodyCell).Text(item.Observation ?? "");

                                index++;
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            var fileName = $"asistencia_clase_{id}_{DateTime.Now:yyyyMMddHHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        private async Task<bool> LoadPageDataAsync(int id)
        {
            ClassInfo = await _context.TblClasses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Include(c => c.Classroom)
                    .ThenInclude(cr => cr.Building)
                        .ThenInclude(b => b.Area)
                .Include(c => c.ClassStatus)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (ClassInfo == null)
                return false;

            AttendanceUrl = $"{Request.Scheme}://{Request.Host}/Attendance/Register?token={ClassInfo.TokenLink}";

            Attendances = await _context.TblAttendances
                .Include(a => a.Student)
                    .ThenInclude(s => s.TblDocumentStudents)
                        .ThenInclude(d => d.DocumentType)
                .Include(a => a.AttendanceStatus)
                .Where(a => a.ClassId == id && a.IsActive)
                .OrderBy(a => a.Student.LastName)
                .ThenBy(a => a.Student.FirstName)
                .ToListAsync();

            Total = Attendances.Count;
            MaleTotal = Attendances.Count(a => a.Student.Gender == "M");
            FemaleTotal = Attendances.Count(a => a.Student.Gender == "F");

            TotalEnrolled = await _context.TblClassEnrollments
                .CountAsync(e => e.ClassId == id && e.IsActive);

            TotalPresent = await _context.TblAttendances
                .CountAsync(a => a.ClassId == id && a.IsActive && a.AttendanceStatusId != 3);

            var presentStudentIds = await _context.TblAttendances
                .Where(a => a.ClassId == id && a.IsActive)
                .Select(a => a.StudentId)
                .ToListAsync();

            AbsentStudents = await _context.TblClassEnrollments
                .Include(e => e.Student)
                .Where(e => e.ClassId == id &&
                            e.IsActive &&
                            !presentStudentIds.Contains(e.StudentId))
                .Select(e => e.Student)
                .ToListAsync();

            QuickSummaryText = BuildQuickSummary();

            return true;
        }

        private async Task MarkAbsentStudentsAsync(int id)
        {
            var enrolledStudents = await _context.TblClassEnrollments
                .Where(e => e.ClassId == id && e.IsActive)
                .Select(e => e.StudentId)
                .ToListAsync();

            var registeredStudents = await _context.TblAttendances
                .Where(a => a.ClassId == id && a.IsActive)
                .Select(a => a.StudentId)
                .ToListAsync();

            var absentStudents = enrolledStudents.Except(registeredStudents);

            foreach (var studentId in absentStudents)
            {
                var alreadyExists = await _context.TblAttendances
                    .AnyAsync(a => a.ClassId == id && a.StudentId == studentId);

                if (!alreadyExists)
                {
                    _context.TblAttendances.Add(new TblAttendance
                    {
                        ClassId = id,
                        StudentId = studentId,
                        AttendanceStatusId = 3,
                        RegisterMethodId = 1,
                        IsJustified = false,
                        RegisterTime = DateTime.Now,
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        private string GetStudentCarnet(CatStudent student)
        {
            return student.TblDocumentStudents
                .FirstOrDefault(d =>
                    d.IsActive &&
                    d.DocumentType.DocumentType.ToLower().Contains("carnet"))?.Document
                ?? "Sin carnet";
        }

        private string BuildQuickSummary()
        {
            if (ClassInfo == null)
                return "";

            return
                $@"Asistencia
                - Fecha: {ClassInfo.ClassDate:dd-MM-yyyy}
                - Docente: {ClassInfo.Teacher.FirstName} {ClassInfo.Teacher.LastName}
                - Aula: {ClassInfo.Classroom.ClassroomName}
                - Grupo: {ClassInfo.Group.GroupName}
                - Turno: {ClassInfo.Group.Shift.ShiftName}
                - Asignatura: {ClassInfo.Subject.SubjectName}
                - Masculino: {MaleTotal}
                - Femenino: {FemaleTotal}
                - Total: {Total}";
        }

        private static QuestPDF.Infrastructure.IContainer HeaderCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken3)
                .Padding(4)
                .DefaultTextStyle(x => x.FontColor(Colors.White).Bold().FontSize(8));
        }

        private static QuestPDF.Infrastructure.IContainer BodyCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(4)
                .DefaultTextStyle(x => x.FontSize(8));
        }

      

        private static void AddInfoRow(TableDescriptor table, string label1, string value1, string label2, string value2)
        {
            table.Cell().Element(InfoLabelCell).Text(label1);
            table.Cell().Element(InfoValueCell).Text(value1);
            table.Cell().Element(InfoLabelCell).Text(label2);
            table.Cell().Element(InfoValueCell).Text(value2);
        }

        private static QuestPDF.Infrastructure.IContainer InfoLabelCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(5)
                .DefaultTextStyle(x => x.Bold().FontSize(8));
        }

        private static QuestPDF.Infrastructure.IContainer InfoValueCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(5)
                .DefaultTextStyle(x => x.FontSize(8));
        }

        private static void AddSummaryBox(
            RowDescriptor row,
            string label,
            string value,
            string background,
            string foreground)
        {
            row.RelativeItem()
                .PaddingRight(6)
                .Background(background)
                .Padding(8)
                .Column(col =>
                {
                    col.Item().Text(label).FontColor(foreground).FontSize(8);
                    col.Item().Text(value).FontColor(foreground).Bold().FontSize(13);
                });
        }

        private static string EscapeCsv(string value)
        {
            value ??= "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return $"\"{value.Replace("\"", "\"\"")}\"";

            return value;
        }

        private byte[]? GetLogoBytes()
        {
            var logoPath = Path.Combine(
                _environment.WebRootPath,
                "images",
                "log_UNHSJM_Texto.jpg"
            );

            if (!System.IO.File.Exists(logoPath))
                return null;

            return System.IO.File.ReadAllBytes(logoPath);
        }
    }
}