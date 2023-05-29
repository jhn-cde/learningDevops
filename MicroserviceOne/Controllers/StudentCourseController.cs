
using Microsoft.AspNetCore.Mvc;
using MicroserviceOne.BusinessService;

namespace MicroserviceOne.Controllers;
[ApiController]
[Route("schoolapi/[controller]")]
public class StudentCourseController: ControllerBase
{
  private StudentCourseBusinessService _studentCourseBusinessService;
  public StudentCourseController(StudentCourseBusinessService studentCourseBusinessService){
    _studentCourseBusinessService = studentCourseBusinessService;
  }

  [HttpGet]
  public List<StudentCourse> GetStudentCourses(){
    return _studentCourseBusinessService.GetStudentCourses();
  }

  [HttpPost]
  public StudentCourse InsertStudentCourse(StudentCourse studentCourse){
      return _studentCourseBusinessService.InsertStudentCourse(studentCourse);
  }
}