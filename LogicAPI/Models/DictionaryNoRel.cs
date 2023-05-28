using System.ComponentModel.DataAnnotations;

namespace LogicAPI
{
  public class DictionaryNoRel
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public List<DictionaryNoRel> Childs {get; set;}
    
    public DictionaryNoRel(){
      Id = -1;
      Name = "";
      Value = "";
      Childs = new List<DictionaryNoRel>();
    }
    public DictionaryNoRel(long id, string name, string value, List<DictionaryNoRel> childs){
      Id = id;
      Name = name;
      Value = value;
      Childs = childs;
    }
    public DictionaryNoRel(DictionaryNoRel dictionaryNoRel){
      Id = dictionaryNoRel.Id;
      Name = dictionaryNoRel.Name;
      Value = dictionaryNoRel.Value;
      Childs = dictionaryNoRel.Childs;
    }
  }
}