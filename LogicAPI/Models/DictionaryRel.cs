using System.ComponentModel.DataAnnotations;

namespace LogicAPI
{
  public class DictionaryRel
  {
    [Key]
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public long? FatherId {get; set;}
    
    public DictionaryRel(){
      Id = -1;
      Name = "";
      Value = "";
      FatherId = null;
    }
    public DictionaryRel(long id, string name, string value, long? fatherId){
      Id = id;
      Name = name;
      Value = value;
      FatherId = fatherId;
    }
    public DictionaryRel(DictionaryRel dictionaryRel){
      Id = dictionaryRel.Id;
      Name = dictionaryRel.Name;
      Value = dictionaryRel.Value;
      FatherId = dictionaryRel.FatherId;
    }
  }
}