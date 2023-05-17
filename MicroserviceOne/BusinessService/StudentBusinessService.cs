using MicroserviceOne.Models;
using MicroserviceOne.DataService;

namespace MicroserviceOne.BusinessService;
public class StudentBusinessService
{
  private StudentDataService _studentDataService;

  public StudentBusinessService(StudentDataService studentDataService){
    _studentDataService = studentDataService;
  }

  public List<Student> GetStudents(){
    return _studentDataService.GetStudents();
  }
  public Student InsertStudent(Student student){
    return _studentDataService.InsertStudent(student);
  } 
}