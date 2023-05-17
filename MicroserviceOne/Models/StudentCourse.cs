using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceOne
{
  public class StudentCourse
  {
    [Key]
    public long Id {get; set;}
    public string CourseName {get; set;}
    public long StudentId {get; set;}
    [ForeignKey(nameof(StudentId))]
    public Student? Student {get; set;}
  }
}