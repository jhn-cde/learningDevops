using MicroserviceOne.Models;
using MicroserviceOne.DataService;

namespace MicroserviceOne.BusinessService;
public class StudentCourseBusinessService
{
  private StudentCourseDataService _studentCourseDataService;

  public StudentCourseBusinessService(StudentCourseDataService studentCourseDataService){
    _studentCourseDataService = studentCourseDataService;
  }

  public List<StudentCourse> GetStudentCourses(){
    return _studentCourseDataService.GetStudentCourses();
  }
  public StudentCourse InsertStudentCourse(StudentCourse studentCourse){
    return _studentCourseDataService.InsertStudentCourse(studentCourse);
  } 
}