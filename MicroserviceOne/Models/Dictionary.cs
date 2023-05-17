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

    public Dictionary(long id, string name, string value, long? fatherId ){
      Id = id; Name = name;
      Value = value;
      FatherId = fatherId;
    }
  }
}