using System.ComponentModel.DataAnnotations;

namespace LogicAPI
{
  public class Dictionary
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public List<Dictionary> Children {get; set;}
    
    public Dictionary(){
      Id = -1;
      Name = "";
      Value = "";
      Children = new List<Dictionary>();
    }
    public Dictionary(long id, string name, string value, List<Dictionary> childs){
      Id = id;
      Name = name;
      Value = value;
      Children = childs;
    }
    public Dictionary(Dictionary dictionaryNoRel){
      Id = dictionaryNoRel.Id;
      Name = dictionaryNoRel.Name;
      Value = dictionaryNoRel.Value;
      Children = dictionaryNoRel.Children;
    }
  }
}