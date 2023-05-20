using Newtonsoft.Json;
using MicroserviceOne.DataService;

namespace MicroserviceOne.BusinessService;
public class DictionaryBusinessService
{
  private DictionaryDataService _dictionaryDataService;
  public DictionaryBusinessService(DictionaryDataService dictionaryDataService){
    _dictionaryDataService = dictionaryDataService;
  }

  public List<Dictionary> GetDictionaries(){
    List<ZTree> treeList = getTreesList();
    return getDictionaryFromTrees(treeList);
  }
  public Dictionary? GetDictionary(int id){
    List<ZTree> treesList = getTreesList();
    List<Dictionary> relList = getDictionaryFromTrees(treesList);
    return relList.Find(dic => dic.Id == id);
  }
  public Dictionary? InsertDictionary(Dictionary newDictionary){
    List<ZTree> treesList = getTreesList();
    List<Dictionary> relList = getDictionaryFromTrees(treesList);
    newDictionary.Id = getNewId(relList);
    
    if(newDictionary.FatherId == null){
      _dictionaryDataService.InsertDictionary(
        new DictionaryNoRel(newDictionary.Id, newDictionary.Name, newDictionary.Value, "")
      );
    }else{
      List<ZTree> treesToUpdate = new List<ZTree>();
      treesList.ForEach(tree => {
        var fatherId = newDictionary.FatherId??-1;
        bool added = tree.addNode(
          new ZNode(newDictionary.Id, newDictionary.Name, newDictionary.Value, new List<ZNode>()), 
          fatherId);
        if(added) treesToUpdate.Add(tree);
      });

      treesToUpdate.ForEach(tree => {
        var noRel = tree.toNoRelational();
        _dictionaryDataService.UpdateDictionary(noRel);
      });
    }

    return newDictionary;
  }
  public bool UpdateDictionary(Dictionary dictionary){
    List<ZTree> treesList = getTreesList();

    ZNode? original = null;
    ZTree? lastTree = null;
    ZTree? newTree = null;
    treesList.ForEach(tree => {
      original = tree.getNode(dictionary.Id);
      if(original != null)
        lastTree = tree;
    });
    var tmp = original!=null?original.Id:1;
    
    if(original != null){
      var fatherId = dictionary.FatherId??-1;
      if(original.Name != dictionary.Name || original.Value != dictionary.Value){
        original.Name = dictionary.Name; original.Value = dictionary.Value;
        lastTree?.editNode(original);
      }
      treesList.ForEach(tree => {
        if(tree.getNode(fatherId) != null)
          newTree = tree;
      });
      long tmpp = newTree!=null?newTree.Root.Id: -1;
      
      if(newTree != null && lastTree != null){
        original = lastTree.getNode(dictionary.Id);
        var oriId = original!=null?original.Id:-1;
        lastTree.removeNode(oriId);
        if(original!=null)
          newTree.addNode(original, fatherId);
      }
      if(lastTree!=null){
        var noRel = lastTree.toNoRelational();
        _dictionaryDataService.UpdateDictionary(noRel);
      }
      if(newTree != null && lastTree != null&& newTree.Root.Id != lastTree.Root.Id){
        var noRel = newTree.toNoRelational();
        _dictionaryDataService.UpdateDictionary(noRel);
      }
    }
    if(original == null) return false;

    return true;
  }

  public bool DeleteDictionary(long id){
    List<ZTree> treesList = getTreesList();
    ZTree? treeToUpdate = null;
    treesList.ForEach(tree => {
      if(tree.getNode(id) != null)
        treeToUpdate = tree;
    });
    if(treeToUpdate != null){
      treeToUpdate.removeNode(id);
      var noRel = treeToUpdate.toNoRelational();
      _dictionaryDataService.UpdateDictionary(noRel);
    } else return false;
    return true;
  }

  // ------ --- --- --- --- -------
  // ------ private methods -------
  // ------ --- --- --- --- -------

  private List<ZTree> getTreesList(){
    List<DictionaryNoRel> noRelList = _dictionaryDataService.GetDictionaries();
    
    List<ZTree> treeList = new List<ZTree>();
    noRelList.ForEach(noRel => treeList.Add(new ZTree(noRel)));
    return treeList;
  }
  private List<Dictionary> getDictionaryFromTrees(List<ZTree> treesList){
    List<Dictionary> relList = new List<Dictionary>();
    treesList.ForEach(tree => {
      var tmp = tree.toRelational();
      relList.AddRange(tmp);
    });
    return relList;
  }
  private long getNewId(List<Dictionary> dictionaries){
    var maxDicId = dictionaries.MaxBy(dic => dic.Id);
    return maxDicId == null ? 0 : maxDicId.Id+1;
  }
}
