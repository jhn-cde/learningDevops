using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceOne
{
  public class ZNode
  {
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public List<ZNode> Childs {get; set;}

    public ZNode(){
      Id = 0;
      Name = "";
      Value = "";
      Childs = new List<ZNode>();
    }
    public ZNode(ZNode node){
      Id = node.Id;
      Name = node.Name;
      Value = node.Value;
      Childs = node.Childs;
    }
    public ZNode(long id, string name, string value, List<ZNode> childs){
      Id = id;
      Name = name;
      Value = value;
      Childs = childs;
    }

    public void addChild(ZNode node){
      Childs.Add(node);
    }
    public void removeChild(long id){
      Childs.RemoveAll(child => child.Id == id);
    }
  }
}