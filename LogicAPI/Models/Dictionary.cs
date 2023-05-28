using System.ComponentModel.DataAnnotations;

namespace LogicAPI
{
  public class Dictionary
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public string Childs {get; set;}
    
    public Dictionary(){
      Id = -1;
      Name = "";
      Value = "";
      Childs = "";
    }
    public Dictionary(string name, string value, string childs){
      Name = name;
      Value = value;
      Childs = childs;
    }
    public Dictionary(long id, string name, string value, string childs){
      Id = id;
      Name = name;
      Value = value;
      Childs = childs;
    }
    public Dictionary(Dictionary noRel){
      Id = noRel.Id;
      Name = noRel.Name;
      Value = noRel.Value;
      Childs = noRel.Childs;
    }
  }
}