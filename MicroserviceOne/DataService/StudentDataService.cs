using MicroserviceOne.Models;

namespace MicroserviceOne.DataService;
public class StudentDataService
{
  private Context _context;

  public StudentDataService(Context context){
    _context = context;
  }

  public List<Student> GetStudents(){
    return _context.Students.ToList();
  } 
  public Student InsertStudent(Student student){
    _context.Students.Add(student);
    _context.SaveChanges();
    return student;
  } 
}