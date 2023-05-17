
using Microsoft.EntityFrameworkCore;

namespace MicroserviceOne.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options):base(options){

        }
        public DbSet<Student> Students {get;set;}
        public DbSet<StudentCourse> StudentCourses {get;set;}
        public DbSet<DictionaryAlfa> Dictionaries {get;set;}
    }
}