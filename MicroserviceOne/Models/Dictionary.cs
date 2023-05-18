using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceOne
{
  public class Dictionary
  {
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public long? FatherId {get; set;}
    public Dictionary(){
      Id = -1; Name = "";
      Value = "";
      FatherId = null;
    }
    public Dictionary(long id, string name, string value, long? fatherId ){
      Id = id; Name = name;
      Value = value;
      FatherId = fatherId;
    }
    public Dictionary(Dictionary _ori ){
      Id = _ori.Id; Name = _ori.Name;
      Value = _ori.Value;
      FatherId = _ori.FatherId;
    }
  }
}