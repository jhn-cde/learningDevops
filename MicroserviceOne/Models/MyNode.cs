using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceOne
{
  public class MyNode
  {
    public long Id {get; set;}
    public string Name {get; set;}
    public string Value {get; set;}
    public LinkedList<MyNode>? Children {get; set;}

    public MyNode(long id, string name, string value){
      Id = id; Name = name;
      Value = value;
      Children = new LinkedList<MyNode>();
    }

    public void AddChild(MyNode child){
      Children.AddFirst(child);
    }
    public LinkedList<MyNode> getChildren(){
      return Children;
    }
  }
}