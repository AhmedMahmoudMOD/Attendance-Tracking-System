using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
	public interface IAdminRepo
	{
		public bool CheckEmailUniqueness(string email, int id);
		public bool CheckEmailUniquenessForNewUsers(string email);
		public Task EditAdminData(Admin admin);
		public void uploadImg(string ImgName, int id);
		public User AdminData(int? id);
		public List<Student> GetStudents();
		public Student GetStudentById(int? id);
		public int DeleteStudent(int? Id);
		public void ImportDataFromExcel(string filePath);
		public void AssignRoleToUser(int userId, int roleId);
		public void UpdateStudentData(Student student);
		public List<Track> GetAllTracks();
		public List<ITIProgram> GetAllPrograms();
		public List<Employee> GetAllEmployees();
		public Employee GetEmployeeById(int? id);
		public int DeleteEmployee(int? Id);
		public void UpdateEmployeeData(Employee employee);
		public int GetRoleId(String RoleType);
		public void AddEmployee(Employee emp, string userImageFileName);
		public void UpdateUserRole(int userId, int roleId);
		public List<Intake> GetIntakes();
		public int DeleteIntake(int? id);
		public Intake GetIntakeById(int? id);
		public int updateIntake(Intake intake);
		public void AddIntake(Intake intake);
		public List<ITIProgram> GetITIPrograms();
		public void AddTracksToIntake(int intakeId, List<int> TracksId);
	}
}
