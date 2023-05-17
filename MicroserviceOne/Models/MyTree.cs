using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroserviceOne
{
  public class MyTree
  {
    public MyNode Father {get; set;}

    public MyTree(MyNode father){
      Father = father;
    }

    public void AddChild(MyNode child, long fatherId){
      
    }
  }
}