using Newtonsoft.Json;

namespace MicroserviceOne
{
  public class ZTree
  {
    public ZNode Root {get; set;}

    public ZTree(ZNode root){
      Root = root;
    }
    public ZTree(DictionaryNoRel noRel){
      Root = ChildsFromJsonString(noRel);
    }

    // --- Methods ---
    public bool addNode(ZNode node, long fatherId){
      bool added = false;
      processTree(Root, null, father => father.Id == fatherId, (father, _) => {father.addChild(node); added = true;});
      return added;
    }
    public ZNode? getNode(long id){
      ZNode? found = null;
      processTree(Root, null, node => node.Id == id, (node,_) => found = node);
      return found; 
    }
    public void editNode(ZNode toUpdate){
      processTree(Root, null, node => node.Id == toUpdate.Id, (node, _) => {
        node.Name = toUpdate.Name;
        node.Value = toUpdate.Value;
      });
    }
    public void removeNode(long id){
      processTree(Root, null, father => father.Childs.FindIndex(node => node.Id == id)>=0, (father, _) => father.removeChild(id));
    }
    public List<Dictionary> toRelational(){
      return childsToList(Root, null);
    }
    private List<Dictionary> childsToList(ZNode node, long? fatherId){
      List<Dictionary> relList = new List<Dictionary>();
      relList.Add(new Dictionary(node.Id, node.Name, node.Value, fatherId));
      
      node.Childs.ForEach(child => {
        List<Dictionary> childList = childsToList(child, node.Id);
        relList.AddRange(childList);
      });
      
      return relList;
    }
    public DictionaryNoRel toNoRelational(){
      return ChildsToJsonString(Root);
    }
    private DictionaryNoRel ChildsToJsonString(ZNode node){
      List<DictionaryNoRel> noRelChilds = new List<DictionaryNoRel>();
      node.Childs.ForEach(child => {
        DictionaryNoRel noRel =  ChildsToJsonString(child);
        noRelChilds.Add(noRel);
      });
      DictionaryNoRel nodeNoRel = new DictionaryNoRel(node.Id, node.Name, node.Value, "");
      nodeNoRel.Childs = JsonConvert.SerializeObject(noRelChilds);
      return nodeNoRel;
    }

    // --- Process Tree ---
    private ZNode ChildsFromJsonString(DictionaryNoRel noRel){
      List<ZNode> ZChilds = new List<ZNode>();
      List<DictionaryNoRel>? childs = JsonConvert.DeserializeObject<List<DictionaryNoRel>>(noRel.Childs);
      
      childs?.ForEach(child => {
        ZNode node = ChildsFromJsonString(child);
        ZChilds.Add(node);
      });

      return new ZNode(noRel.Id, noRel.Name, noRel.Value, ZChilds);
    }

    private void processTree(ZNode root, long? fatherId, Func<ZNode, bool> condition, Action<ZNode, long?> action){
      if(condition(root)){
        action(root, fatherId);
        return;
      }

      root.Childs.ForEach(child => {
        processTree(child, root.Id, condition, action);
      });
    }
  }
}