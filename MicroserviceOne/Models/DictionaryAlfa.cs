using System.ComponentModel.DataAnnotations;

namespace MicroserviceOne
{
  public class DictionaryAlfa
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public string? Childs {get; set;}
    
    public DictionaryAlfa(long id, string name, string value, string? childs){
      Id = id;
      Name = name;
      Value = value;
      Childs = childs;
    }
  }
}