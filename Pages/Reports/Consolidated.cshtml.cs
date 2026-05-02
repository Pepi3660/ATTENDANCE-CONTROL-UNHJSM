using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace AttendanceControl.Web.Pages.Reports
{
    [Authorize(Policy = "TeacherOrAdmin")]
    public class ConsolidatedModel : PageModel
    {
        private readonly AttendanceDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ConsolidatedModel(AttendanceDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty(SupportsGet = true)]
        public DateOnly? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CareerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ShiftId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SubjectId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TeacherId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? StudentId { get; set; }

        public bool IsAdminUser => User.IsInRole("ADMIN");

        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Shifts { get; set; } = new();
        public List<SelectListItem> Groups { get; set; } = new();
        public List<SelectListItem> Subjects { get; set; } = new();
        public List<SelectListItem> Teachers { get; set; } = new();
        public List<SelectListItem> Students { get; set; } = new();

        public List<ConsolidatedRow> Results { get; set; } = new();

        public int TotalStudents { get; set; }
        public int TotalMeetings { get; set; }
        public int TotalPresent { get; set; }
        public int TotalLate { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalJustified { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetConsolidatedDataAsync();
            LoadSummary();

            return Page();
        }

        public async Task<IActionResult> OnGetExportCsvAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetConsolidatedDataAsync();
            LoadSummary();

            var csv = new StringBuilder();

            csv.AppendLine("Reporte Consolidado de Asistencia");
            csv.AppendLine($"Generado,{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Fecha inicial,{StartDate?.ToString("dd/MM/yyyy") ?? "Todas"}");
            csv.AppendLine($"Fecha final,{EndDate?.ToString("dd/MM/yyyy") ?? "Todas"}");
            csv.AppendLine($"Carrera,{GetSelectedText(Careers, CareerId) ?? "Todas"}");
            csv.AppendLine($"Turno,{GetSelectedText(Shifts, ShiftId) ?? "Todos"}");
            csv.AppendLine($"Grupo,{GetSelectedText(Groups, GroupId) ?? "Todos"}");
            csv.AppendLine($"Asignatura,{GetSelectedText(Subjects, SubjectId) ?? "Todas"}");
            csv.AppendLine($"Docente,{GetSelectedText(Teachers, TeacherId) ?? (IsAdminUser ? "Todos" : "Docente actual")}");
            csv.AppendLine($"Estudiante,{GetSelectedText(Students, StudentId) ?? "Todos"}");
            csv.AppendLine();

            csv.AppendLine("Carnet,Estudiante,Genero,Carrera,Grupo,Turno,Total Encuentros,Presentes,Tardes,Ausentes,Justificadas,Porcentaje Asistencia");

            foreach (var row in Results)
            {
                csv.AppendLine(
                    $"{EscapeCsv(row.Carnet)}," +
                    $"{EscapeCsv(row.StudentName)}," +
                    $"{EscapeCsv(row.Gender)}," +
                    $"{EscapeCsv(row.CareerName)}," +
                    $"{EscapeCsv(row.GroupName)}," +
                    $"{EscapeCsv(row.ShiftName)}," +
                    $"{row.TotalMeetings}," +
                    $"{row.PresentCount}," +
                    $"{row.LateCount}," +
                    $"{row.AbsentCount}," +
                    $"{row.JustifiedCount}," +
                    $"{row.AttendancePercentage:0.0}%"
                );
            }

            csv.AppendLine();
            csv.AppendLine("Resumen");
            csv.AppendLine($"Estudiantes,{TotalStudents}");
            csv.AppendLine($"Encuentros,{TotalMeetings}");
            csv.AppendLine($"Presentes,{TotalPresent}");
            csv.AppendLine($"Tardes,{TotalLate}");
            csv.AppendLine($"Ausentes,{TotalAbsent}");
            csv.AppendLine($"Justificadas,{TotalJustified}");

            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();

            var fileName = $"reporte_consolidado_{DateTime.Now:yyyyMMddHHmm}.csv";

            return File(bytes, "text/csv", fileName);
        }

        public async Task<IActionResult> OnGetExportPdfAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetConsolidatedDataAsync();
            LoadSummary();

            QuestPDF.Settings.License = LicenseType.Community;

            var logoBytes = GetLogoBytes();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(26);
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
                                col.Item().AlignCenter().Text("Reporte Consolidado de Asistencia")
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
                        content.Item().Text("Datos generales del reporte")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(6).Element(container =>
                        {
                            container
                                .Background(Colors.Grey.Lighten4)
                                .Border(0.5f)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(10)
                                .Column(col =>
                                {
                                    col.Item().Text($"Período: {(StartDate?.ToString("dd/MM/yyyy") ?? "Todas")} - {(EndDate?.ToString("dd/MM/yyyy") ?? "Todas")}")
                                        .Bold()
                                        .FontSize(9);

                                    col.Item().Text($"Carrera: {GetSelectedText(Careers, CareerId) ?? "Todas"}")
                                        .FontSize(8);

                                    col.Item().Text($"Grupo: {GetSelectedText(Groups, GroupId) ?? "Todos"}")
                                        .FontSize(8);

                                    col.Item().Text($"Turno: {GetSelectedText(Shifts, ShiftId) ?? "Todos"}")
                                        .FontSize(8);

                                    col.Item().Text($"Docente: {GetSelectedText(Teachers, TeacherId) ?? (IsAdminUser ? "Todos" : "Docente actual")}")
                                        .FontSize(8);

                                    col.Item().Text($"Asignatura: {GetSelectedText(Subjects, SubjectId) ?? "Todas"}")
                                        .FontSize(8);

                                    col.Item().Text($"Estudiante: {GetSelectedText(Students, StudentId) ?? "Todos"}")
                                        .FontSize(8);
                                });
                        });

                        content.Item().PaddingTop(12).Text("Resumen")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Row(row =>
                        {
                            AddSummaryBox(row, "Estudiantes", TotalStudents.ToString(), Colors.Blue.Darken2, Colors.White);
                            AddSummaryBox(row, "Encuentros", TotalMeetings.ToString(), Colors.Cyan.Darken2, Colors.White);
                            AddSummaryBox(row, "Presentes", TotalPresent.ToString(), Colors.Green.Darken2, Colors.White);
                            AddSummaryBox(row, "Tardes", TotalLate.ToString(), Colors.Orange.Lighten1, Colors.Black);
                            AddSummaryBox(row, "Ausentes", TotalAbsent.ToString(), Colors.Red.Darken2, Colors.White);
                            AddSummaryBox(row, "Justificadas", TotalJustified.ToString(), Colors.Purple.Darken2, Colors.White);
                        });

                        content.Item().PaddingTop(15).Text("Listado consolidado por estudiante")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(2.2f);
                                columns.ConstantColumn(45);

                                if (!CareerId.HasValue)
                                    columns.RelativeColumn(1.4f);

                                if (!GroupId.HasValue)
                                    columns.RelativeColumn(1.2f);

                                if (!ShiftId.HasValue)
                                    columns.RelativeColumn(1.1f);

                                columns.ConstantColumn(55);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(45);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(55);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("N°");
                                header.Cell().Element(HeaderCell).Text("Carnet");
                                header.Cell().Element(HeaderCell).Text("Estudiante");
                                header.Cell().Element(HeaderCell).Text("Género");

                                if (!CareerId.HasValue)
                                    header.Cell().Element(HeaderCell).Text("Carrera");

                                if (!GroupId.HasValue)
                                    header.Cell().Element(HeaderCell).Text("Grupo");

                                if (!ShiftId.HasValue)
                                    header.Cell().Element(HeaderCell).Text("Turno");

                                header.Cell().Element(HeaderCell).Text("Encuentros");
                                header.Cell().Element(HeaderCell).Text("Presentes");
                                header.Cell().Element(HeaderCell).Text("Tardes");
                                header.Cell().Element(HeaderCell).Text("Ausentes");
                                header.Cell().Element(HeaderCell).Text("Justif.");
                                header.Cell().Element(HeaderCell).Text("%");
                            });

                            var index = 1;

                            foreach (var item in Results)
                            {
                                table.Cell().Element(BodyCell).Text(index.ToString());
                                table.Cell().Element(BodyCell).Text(item.Carnet);
                                table.Cell().Element(BodyCell).Text(item.StudentName);
                                table.Cell().Element(BodyCell).Text(item.Gender);

                                if (!CareerId.HasValue)
                                    table.Cell().Element(BodyCell).Text(item.CareerName);

                                if (!GroupId.HasValue)
                                    table.Cell().Element(BodyCell).Text(item.GroupName);

                                if (!ShiftId.HasValue)
                                    table.Cell().Element(BodyCell).Text(item.ShiftName);

                                table.Cell().Element(BodyCell).Text(item.TotalMeetings.ToString());
                                table.Cell().Element(BodyCell).Text(item.PresentCount.ToString());
                                table.Cell().Element(BodyCell).Text(item.LateCount.ToString());
                                table.Cell().Element(BodyCell).Text(item.AbsentCount.ToString());
                                table.Cell().Element(BodyCell).Text(item.JustifiedCount.ToString());
                                table.Cell().Element(BodyCell).Text($"{item.AttendancePercentage:0.0}%");

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

            var fileName = $"reporte_consolidado_{DateTime.Now:yyyyMMddHHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        public async Task<JsonResult> OnGetFilterOptionsAsync(int? careerId, int? shiftId, int? groupId, int? teacherId, int? subjectId)
        {
            var classesQuery = _context.TblClasses
                .Include(c => c.Group)
                    .ThenInclude(g => g.Career)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Where(c => c.IsActive || c.ClassStatusId == 2)
                .AsQueryable();

            if (!IsAdminUser)
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();

                if (currentTeacherId == null)
                {
                    return new JsonResult(new
                    {
                        groups = Array.Empty<object>(),
                        shifts = Array.Empty<object>(),
                        subjects = Array.Empty<object>(),
                        students = Array.Empty<object>()
                    });
                }

                classesQuery = classesQuery.Where(c => c.TeacherId == currentTeacherId.Value);
            }
            else if (teacherId.HasValue)
            {
                classesQuery = classesQuery.Where(c => c.TeacherId == teacherId.Value);
            }

            if (careerId.HasValue)
                classesQuery = classesQuery.Where(c => c.Group.CareerId == careerId.Value);

            if (shiftId.HasValue)
                classesQuery = classesQuery.Where(c => c.Group.ShiftId == shiftId.Value);

            if (groupId.HasValue)
                classesQuery = classesQuery.Where(c => c.GroupId == groupId.Value);

            if (subjectId.HasValue)
                classesQuery = classesQuery.Where(c => c.SubjectId == subjectId.Value);

            var classIds = await classesQuery
                .Select(c => c.ClassId)
                .ToListAsync();

            var groups = await classesQuery
                .Select(c => new
                {
                    value = c.GroupId.ToString(),
                    text = c.Group.GroupName
                    //text = c.Group.Career.CareerName + " - " + c.Group.GroupName + " - " + c.Group.Shift.ShiftName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var shifts = await classesQuery
                .Select(c => new
                {
                    value = c.Group.ShiftId.ToString(),
                    text = c.Group.Shift.ShiftName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var subjects = await classesQuery
                .Select(c => new
                {
                    value = c.SubjectId.ToString(),
                    text = c.Subject.SubjectName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var students = await _context.TblClassEnrollments
                .Include(e => e.Student)
                .Where(e =>
                    classIds.Contains(e.ClassId) &&
                    e.IsActive &&
                    e.Student.IsActive)
                .Select(e => new
                {
                    value = e.StudentId.ToString(),
                    text = e.Student.FirstName + " " + e.Student.LastName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            return new JsonResult(new
            {
                groups,
                shifts,
                subjects,
                students
            });
        }

        private async Task<List<ConsolidatedRow>> GetConsolidatedDataAsync()
        {
            var classQuery = _context.TblClasses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Career)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Where(c => c.IsActive || c.ClassStatusId == 2)
                .AsQueryable();

            if (!IsAdminUser)
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();

                if (currentTeacherId == null)
                    return new List<ConsolidatedRow>();

                classQuery = classQuery.Where(c => c.TeacherId == currentTeacherId.Value);
                TeacherId = null;
            }
            else if (TeacherId.HasValue)
            {
                classQuery = classQuery.Where(c => c.TeacherId == TeacherId.Value);
            }

            if (StartDate.HasValue)
                classQuery = classQuery.Where(c => c.ClassDate >= StartDate.Value);

            if (EndDate.HasValue)
                classQuery = classQuery.Where(c => c.ClassDate <= EndDate.Value);

            if (CareerId.HasValue)
                classQuery = classQuery.Where(c => c.Group.CareerId == CareerId.Value);

            if (ShiftId.HasValue)
                classQuery = classQuery.Where(c => c.Group.ShiftId == ShiftId.Value);

            if (GroupId.HasValue)
                classQuery = classQuery.Where(c => c.GroupId == GroupId.Value);

            if (SubjectId.HasValue)
                classQuery = classQuery.Where(c => c.SubjectId == SubjectId.Value);

            var classes = await classQuery
                .OrderBy(c => c.ClassDate)
                .ThenBy(c => c.StartTime)
                .ToListAsync();

            if (!classes.Any())
                return new List<ConsolidatedRow>();

            var classIds = classes.Select(c => c.ClassId).ToList();

            var enrollments = await _context.TblClassEnrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s.TblDocumentStudents)
                        .ThenInclude(d => d.DocumentType)
                .Where(e =>
                    classIds.Contains(e.ClassId) &&
                    e.IsActive &&
                    e.Student.IsActive)
                .ToListAsync();

            if (StudentId.HasValue)
            {
                enrollments = enrollments
                    .Where(e => e.StudentId == StudentId.Value)
                    .ToList();
            }

            var attendances = await _context.TblAttendances
                .Where(a =>
                    classIds.Contains(a.ClassId) &&
                    a.IsActive)
                .ToListAsync();

            var rows = new List<ConsolidatedRow>();

            var groupedStudents = enrollments
                .GroupBy(e => e.StudentId)
                .OrderBy(g => g.First().Student.LastName)
                .ThenBy(g => g.First().Student.FirstName);

            foreach (var studentGroup in groupedStudents)
            {
                var student = studentGroup.First().Student;

                var studentClassIds = studentGroup
                    .Select(e => e.ClassId)
                    .Distinct()
                    .ToList();

                var studentClasses = classes
                    .Where(c => studentClassIds.Contains(c.ClassId))
                    .ToList();

                if (!studentClasses.Any())
                    continue;

                var studentAttendances = attendances
                    .Where(a =>
                        a.StudentId == student.StudentId &&
                        studentClassIds.Contains(a.ClassId))
                    .ToList();

                var presentCount = studentAttendances.Count(a => a.AttendanceStatusId == 1);
                var lateCount = studentAttendances.Count(a => a.AttendanceStatusId == 2);
                var explicitAbsentCount = studentAttendances.Count(a => a.AttendanceStatusId == 3);

                var classesWithAttendance = studentAttendances
                    .Select(a => a.ClassId)
                    .Distinct()
                    .Count();

                var missingAttendanceCount = studentClasses.Count - classesWithAttendance;

                var absentCount = explicitAbsentCount + missingAttendanceCount;
                var justifiedCount = studentAttendances.Count(a => a.IsJustified);

                var firstClass = studentClasses.First();

                rows.Add(new ConsolidatedRow
                {
                    Carnet = GetStudentCarnet(student),
                    StudentName = $"{student.FirstName} {student.LastName}",
                    Gender = student.Gender,
                    CareerName = firstClass.Group.Career.CareerName,
                    GroupName = firstClass.Group.GroupName,
                    ShiftName = firstClass.Group.Shift.ShiftName,
                    TotalMeetings = studentClasses.Count,
                    PresentCount = presentCount,
                    LateCount = lateCount,
                    AbsentCount = absentCount,
                    JustifiedCount = justifiedCount
                });
            }

            return rows;
        }

        private void LoadSummary()
        {
            TotalStudents = Results.Count;
            TotalMeetings = Results.Sum(x => x.TotalMeetings);
            TotalPresent = Results.Sum(x => x.PresentCount);
            TotalLate = Results.Sum(x => x.LateCount);
            TotalAbsent = Results.Sum(x => x.AbsentCount);
            TotalJustified = Results.Sum(x => x.JustifiedCount);
        }

        private async Task LoadCatalogsAsync()
        {
            Careers = await _context.CatCareers
                .Where(c => c.IsActive)
                .OrderBy(c => c.CareerName)
                .Select(c => new SelectListItem
                {
                    Value = c.CareerId.ToString(),
                    Text = c.CareerName
                })
                .ToListAsync();

            Shifts = await _context.CatShifts
                .Where(s => s.IsActive)
                .OrderBy(s => s.ShiftName)
                .Select(s => new SelectListItem
                {
                    Value = s.ShiftId.ToString(),
                    Text = s.ShiftName
                })
                .ToListAsync();

            Groups = await _context.CatGroups
                .Include(g => g.Career)
                .Include(g => g.Shift)
                .Where(g => g.IsActive)
                .OrderBy(g => g.Career.CareerName)
                .ThenBy(g => g.GroupName)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupName
                    //Text = g.Career.CareerName + " - " + g.GroupName + " - " + g.Shift.ShiftName
                })
                .ToListAsync();

            Subjects = await _context.CatSubjects
                .Where(s => s.IsActive)
                .OrderBy(s => s.SubjectName)
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.SubjectName
                })
                .ToListAsync();

            Teachers = IsAdminUser
                ? await _context.CatTeachers
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.FirstName)
                    .Select(t => new SelectListItem
                    {
                        Value = t.TeacherId.ToString(),
                        Text = t.FirstName + " " + t.LastName
                    })
                    .ToListAsync()
                : new List<SelectListItem>();

            Students = await _context.CatStudents
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new SelectListItem
                {
                    Value = s.StudentId.ToString(),
                    Text = s.FirstName + " " + s.LastName
                })
                .ToListAsync();
        }

        private async Task<int?> GetCurrentTeacherIdAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
                return null;

            return await _context.CatUsers
                .Where(u => u.IdUser == userId && u.IsActive)
                .Select(u => (int?)u.TeacherId)
                .FirstOrDefaultAsync();
        }

        private static string GetStudentCarnet(CatStudent student)
        {
            return student.TblDocumentStudents
                .FirstOrDefault(d =>
                    d.IsActive &&
                    d.DocumentType.DocumentType.ToLower().Contains("carnet"))?.Document
                ?? "Sin carnet";
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

        private static string? GetSelectedText(List<SelectListItem> items, int? selectedValue)
        {
            if (!selectedValue.HasValue)
                return null;

            return items
                .FirstOrDefault(x => x.Value == selectedValue.Value.ToString())
                ?.Text;
        }

        private static void AddSummaryBox(
            RowDescriptor row,
            string label,
            string value,
            string background,
            string foreground)
        {
            row.RelativeItem()
                .PaddingRight(5)
                .Background(background)
                .Padding(7)
                .Column(col =>
                {
                    col.Item().Text(label).FontColor(foreground).FontSize(8);
                    col.Item().Text(value).FontColor(foreground).Bold().FontSize(12);
                });
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

        private static QuestPDF.Infrastructure.IContainer HeaderCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken3)
                .Padding(4)
                .DefaultTextStyle(x => x.FontColor(Colors.White).Bold().FontSize(7));
        }

        private static QuestPDF.Infrastructure.IContainer BodyCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(4)
                .DefaultTextStyle(x => x.FontSize(7));
        }

        private static string EscapeCsv(string value)
        {
            value ??= "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return $"\"{value.Replace("\"", "\"\"")}\"";

            return value;
        }

        public class ConsolidatedRow
        {
            public string Carnet { get; set; } = "";
            public string StudentName { get; set; } = "";
            public string Gender { get; set; } = "";
            public string CareerName { get; set; } = "";
            public string GroupName { get; set; } = "";
            public string ShiftName { get; set; } = "";
            public int TotalMeetings { get; set; }
            public int PresentCount { get; set; }
            public int LateCount { get; set; }
            public int AbsentCount { get; set; }
            public int JustifiedCount { get; set; }

            public double AttendancePercentage =>
                TotalMeetings == 0
                    ? 0
                    : (double)(PresentCount + LateCount) / TotalMeetings * 100;
        }
    }
}