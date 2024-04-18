using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using System.Net;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace Attendance_Tracking_System.Repositories
{
	public class AdminRepo : IAdminRepo
	{
		private readonly ITISysContext context;
		public AdminRepo(ITISysContext _context)
		{
			this.context = _context;
		}
		public User AdminData(int? id)
		{
			User user = context.User.FirstOrDefault(a => a.Id == id.Value);
			return user;
		}
		public async Task EditAdminData(Admin admin)
		{
			var user = await context.admin.FirstOrDefaultAsync(a => a.Id == admin.Id);
			if (user != null)
			{
				user.Name = admin.Name;
				user.Email = admin.Email;
				user.PhoneNumber = admin.PhoneNumber;
				user.Age = admin.Age;
				user.Password = admin.Password;
				user.UserImage = admin.UserImage;
				await context.SaveChangesAsync();
			}
		}
		public void uploadImg(string ImgName, int id)
		{
			//var res = context.admin.FirstOrDefault(a => a.Id == id);User
			var res = context.User.FirstOrDefault(a => a.Id == id);
			res.UserImage = ImgName;
			context.SaveChanges();
		}
		public bool CheckEmailUniqueness(string email, int id)
		{
			return !context.User.Any(a => a.Email == email && a.Id != id);
		}
		public bool CheckEmailUniquenessForNewUsers(string email)
		{
			return !context.User.Any(a => a.Email == email);
		}
		public List<Student> GetStudents()
		{
			return context.Student.Where(a => a.IsDeleted == false).ToList();
		}
		public Student GetStudentById(int? id)
		{
			return context.Student.FirstOrDefault(std => std.Id == id);
		}
		public int DeleteStudent(int? Id)
		{
			var res = context.Student.FirstOrDefault(s => s.Id == Id && !s.IsDeleted);
			if (res != null)
			{
				res.IsDeleted = true;
				context.SaveChanges();
				return 1;
			}
			return 0;
		}
		public void ImportDataFromExcel(string filePath)
		{
			using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

				int rowCount = worksheet.Dimension.End.Row;
				int columnCount = worksheet.Dimension.Columns;
				for (int row = 2; row <= rowCount; row++)
				{
					Student entity = new Student();
					entity.Name = worksheet.Cells[row, 1].Value.ToString() ?? "";
					entity.Email = worksheet.Cells[row, 2].Value.ToString() ?? "";
					entity.Password = worksheet.Cells[row, 3].Value.ToString() ?? "";
					entity.Age = int.Parse(worksheet.Cells[row, 4].Value.ToString() ?? "");
					entity.PhoneNumber = worksheet.Cells[row, 5].Value.ToString() ?? "";
					entity.University = worksheet.Cells[row, 6].Value.ToString() ?? "";
					entity.Faculty = worksheet.Cells[row, 7].Value.ToString() ?? "";
					entity.GraduationYear = int.Parse(worksheet.Cells[row, 8].Value.ToString() ?? "");
					entity.Specialization = worksheet.Cells[row, 9].Value.ToString() ?? "";
					entity.ProgramID = int.Parse(worksheet.Cells[row, 10].Value.ToString() ?? "");
					entity.TrackID = int.Parse(worksheet.Cells[row, 11].Value.ToString() ?? "");
					entity.IntakeNo = int.Parse(worksheet.Cells[row, 12].Value.ToString() ?? "");
					context.Student.Add(entity);
					AssignRoleToUser(entity.Id, 1);
				}
				context.SaveChanges();
			}
		}
		public void AssignRoleToUser(int userId, int roleId)
		{
			var user = context.User.FirstOrDefault(u => u.Id == userId);
			var role = context.roles.FirstOrDefault(r => r.Id == roleId);
			if (user != null && role != null)
			{
				user.role.Add(role);
				context.SaveChanges();
			}
		}
		public void UpdateStudentData(Student student)
		{
			var res = context.Student.FirstOrDefault(s => s.Id == student.Id);
			if (res != null)
			{
				res.Name = student.Name;

				res.TrackID = student.TrackID;
				var tId = student.TrackID;
				var track = context.Track.FirstOrDefault(a => a.Id == tId);
				var progId = track?.ProgramID;
				res.ProgramID = progId;
				res.Email = student.Email;
				res.Faculty = student.Faculty;
				res.Age = student.Age;
				res.GraduationYear = student.GraduationYear;
				res.Password = student.Password;
				res.PhoneNumber = student.PhoneNumber;
				res.RegisterationStatus = student.RegisterationStatus;
				var intake = context.Intake.FirstOrDefault(i => i.ProgramID == progId);
				res.IntakeNo = intake.No;
				res.University = student.University;
				//res.UserImage = student.UserImage;
				res.AttendanceDegrees = student.AttendanceDegrees;
				res.RegisterationStatus = student.RegisterationStatus;
				context.SaveChanges();
			}
		}
		public List<Track> GetAllTracks()
		{
			return context.Track.Where(t => t.Status == true).ToList();
		}
		public List<ITIProgram> GetAllPrograms()
		{
			return context.Program.Where(p => p.IsDeleted == false).ToList();
		}
		public List<Employee> GetAllEmployees()
		{
			return context.Employee.Where(a => a.IsDeleted == false).ToList();

		}
		public Employee GetEmployeeById(int? id)
		{
			return context.Employee.FirstOrDefault(emp => emp.Id == id);
		}
		public int DeleteEmployee(int? Id)
		{
			var res = context.Employee.FirstOrDefault(s => s.Id == Id && !s.IsDeleted);
			if (res != null)
			{
				res.IsDeleted = true;
				context.SaveChanges();
				return 1;
			}
			return 0;
		}
		public void UpdateEmployeeData(Employee employee)
		{
			var res = context.Employee.FirstOrDefault(s => s.Id == employee.Id);
			if (res != null)
			{
				res.Name = employee.Name;
				res.Email = employee.Email;
				res.Age = employee.Age;
				res.Password = employee.Password;
				res.PhoneNumber = employee.PhoneNumber;
				//res.UserImage = employee.UserImage;
				res.Salary = employee.Salary;
				res.Type = employee.Type;
				context.SaveChanges();
			}
		}
		public int GetRoleId(String RoleType)
		{
			if (RoleType != null)
			{
				var res = context.roles.FirstOrDefault(a => a.RoleType == RoleType);
				return res.Id;
			}
			return 0;
		}
		public void AddEmployee(Employee emp, string userImageFileName)
		{
			if (emp != null)
			{
			//	emp.UserImage = userImageFileName;
				context.Employee.Add(emp);
				context.SaveChanges();
			}
		}
		public void UpdateUserRole(int userId, int roleId)
		{
			var user = context.User.FirstOrDefault(u => u.Id == userId);
			var role = context.roles.FirstOrDefault(r => r.Id == roleId);

			if (user != null && role != null)
			{
				foreach (var existingRole in user.role.ToList())
				{
					user.role.Remove(existingRole);
				}
				user.role.Add(role);

				context.SaveChanges();
			}
		}
		public List<Intake> GetIntakes()
		{
			return context.Intake.Where(a => a.IsDeleted == false).ToList();
		}
		public int DeleteIntake(int? id)
		{
			var intake = context.Intake.FirstOrDefault(a => a.No == id);
			if (id != null)
			{
				intake.IsDeleted = true;
				context.SaveChanges();
				return 1;
			}
			return 0;
		}
		public Intake GetIntakeById(int? id)
		{
			var intake = context.Intake.FirstOrDefault(i => i.No == id);
			return intake;
		}
		public int updateIntake(Intake intake)
		{
			var i = context.Intake.FirstOrDefault(i => i.No == intake.No);
			if (i != null)
			{
				i.Name = intake.Name;
				i.StartDate = intake.StartDate;
				i.EndDate = intake.EndDate;
				context.SaveChanges();
				return 1;
			}
			return -1;
		}
		public void AddIntake(Intake intake)
		{
			if (intake != null)
			{
				context.Intake.Add(intake);
				context.SaveChanges();
			}
		}
		public void AddTracksToIntake(int intakeId, List<int> TracksId)
		{
			var intake = context.Intake.FirstOrDefault(i => i.No == intakeId);
			if (intake != null)
			{
				foreach (var trackId in TracksId)
				{
					var track = context.Track.FirstOrDefault(t => t.Id == trackId);
					intake.Tracks.Add(track);
					context.SaveChanges();
				}
			}
		}
		public List<ITIProgram> GetITIPrograms()
		{
			return context.Program.Where(p => p.IsDeleted == false).ToList();
		}

	}
}
