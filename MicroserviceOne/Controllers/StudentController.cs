
using Microsoft.AspNetCore.Mvc;
using MicroserviceOne.BusinessService;

namespace MicroserviceOne.Controllers;
[ApiController]
[Route("api/[controller]")]
public class StudentController: ControllerBase
{
  private StudentBusinessService _studentBusinessService;
  public StudentController(StudentBusinessService studentBusinessService){
    _studentBusinessService = studentBusinessService;
  }

  [HttpGet]
  public List<Student> GetStudents(){
    return _studentBusinessService.GetStudents();
  }

  [HttpPost]
  public Student InsertStudent(Student student){
      return _studentBusinessService.InsertStudent(student);
  }
}