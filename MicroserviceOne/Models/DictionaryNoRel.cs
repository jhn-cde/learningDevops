using System.ComponentModel.DataAnnotations;

namespace MicroserviceOne
{
  public class DictionaryNoRel
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public string Childs {get; set;}
    
    public DictionaryNoRel(){
      Id = -1;
      Name = "";
      Value = "";
      Childs = "";
    }
    public DictionaryNoRel(string name, string value, string childs){
      Name = name;
      Value = value;
      Childs = childs;
    }
    public DictionaryNoRel(long id, string name, string value, string childs){
      Id = id;
      Name = name;
      Value = value;
      Childs = childs;
    }
    public DictionaryNoRel(DictionaryNoRel noRel){
      Id = noRel.Id;
      Name = noRel.Name;
      Value = noRel.Value;
      Childs = noRel.Childs;
    }
  }
}