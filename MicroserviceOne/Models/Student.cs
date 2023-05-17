using System.ComponentModel.DataAnnotations;

namespace MicroserviceOne
{
  public class Student
  {
    [Key]
    public long Id {get; set;}
    public string FirstName {get; set;}
    public string LastName {get; set;}
    public ICollection<StudentCourse>? StudentCourses {get; set;}
  }
}