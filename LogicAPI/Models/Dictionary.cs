using System.ComponentModel.DataAnnotations;

namespace LogicAPI
{
  public class SerializedDictionary
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public string Children {get; set;}
    
    public SerializedDictionary(){
      Id = -1;
      Name = "";
      Value = "";
      Children = "";
    }
    public SerializedDictionary(string name, string value, string childs){
      Name = name;
      Value = value;
      Children = childs;
    }
    public SerializedDictionary(long id, string name, string value, string childs){
      Id = id;
      Name = name;
      Value = value;
      Children = childs;
    }
    public SerializedDictionary(SerializedDictionary noRel){
      Id = noRel.Id;
      Name = noRel.Name;
      Value = noRel.Value;
      Children = noRel.Children;
    }
  }
}