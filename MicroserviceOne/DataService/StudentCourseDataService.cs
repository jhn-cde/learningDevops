using MicroserviceOne.Models;

namespace MicroserviceOne.DataService;
public class StudentCourseDataService
{
  private Context _context;

  public StudentCourseDataService(Context context){
    _context = context;
  }

  public List<StudentCourse> GetStudentCourses(){
    return _context.StudentCourses.ToList();
  } 
  public StudentCourse InsertStudentCourse(StudentCourse studentCourse){
    _context.StudentCourses.Add(studentCourse);
    _context.SaveChanges();
    return studentCourse;
  } 
}