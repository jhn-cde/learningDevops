using LogicAPI.DataService;
using System.Text.Json;

namespace LogicAPI.BusinessService;
public class DictionaryBusinessService
{
  private DictionaryDataService _dictionaryDS;
  public DictionaryBusinessService(DictionaryDataService dictionaryDS){
    _dictionaryDS = dictionaryDS;
  }

  public string CanConnect(){
    if(_dictionaryDS.CanConnect())
      return "API is working!";
    else
      return "Connection to database failed!...";
  }
  public IEnumerable<DictionaryNoRel> Get(){
    var jsonString_trees = _dictionaryDS.Get();
    var trees = new List<DictionaryNoRel>();
    jsonString_trees.ForEach(tree => {
      trees.Add(ProcessJsonString(tree, (_,_) => {}));
    });

    return trees;
  }
  public DictionaryNoRel? Insert(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();

    long validId = dRel.Id;
    DictionaryNoRel? toInsert = null;
    DictionaryNoRel? toUpdate = null;

    jsonString_trees.ForEach( tree => {
      var bigFather = ProcessJsonString(
        tree,
        (father, _) => {
          if(father.Id == validId) validId++;
          if(father.Id == dRel.FatherId)
            toInsert = new DictionaryNoRel(dRel.Id, dRel.Name, dRel.Value, new List<DictionaryNoRel>());
        }
      );
      if(toInsert != null) {
        toUpdate = bigFather;
      };
    });
    
    // dictionary have a father
    if(toInsert != null && toUpdate != null){
      toInsert.Id = validId;
      var bigFatherProcess = ProcessDictionaryNoRel(toUpdate,
        (father) => {
          if(father.Id == dRel.FatherId)
          father.Childs.Add(toInsert);
        }
      );
      _dictionaryDS.Update(bigFatherProcess);
    }

    // dictionary doesnt have father
    if(toInsert == null && dRel.FatherId == null){
      var to_insert = new Dictionary(validId, dRel.Name, dRel.Value, ConvertToJsonString(new List<Dictionary>()));
      var tmp = _dictionaryDS.Insert(to_insert);
      toInsert = ProcessJsonString(tmp, (_,_)=>{});
    }
    return toInsert;
  }

  public DictionaryNoRel Update(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();
    DictionaryNoRel? toInsert = null;
    DictionaryNoRel? lastBigFather = null;
    DictionaryNoRel? newBigFather = null;
    long? lastFatherId = -1;
    long? curFatherId = -1;

    jsonString_trees.ForEach( tree => {
      bool isCurBigFather = false;
      var bigFather = ProcessJsonString(
        tree,
        (node, fatherId) => {
          if(node.Id == dRel.Id)
          {
            node.Name = dRel.Name; node.Value = dRel.Value;
            // default
            if(fatherId == dRel.FatherId) isCurBigFather = true;
            // if father changed, need to update last father
            if(fatherId != dRel.FatherId) lastFatherId = fatherId; 
          }
          // search curFather
          if(node.Id == dRel.FatherId) isCurBigFather = true;
        }
      );
      if(lastFatherId != null) lastBigFather = bigFather;
      if(isCurBigFather) newBigFather = bigFather;
    });
    
    toInsert = new DictionaryNoRel(dRel.Id, dRel.Name, dRel.Value, new List<DictionaryNoRel>());
    /*
    if(lastBigFather != null && lastBigFather.Id != newBigFather.Id){
      var bigFatherProcess = ProcessDictionaryNoRel(lastBigFather,
        (father) => {
          if(father.Id == dRel.FatherId)
            father.Childs.Add(toInsert);
        }
      );
      _dictionaryDS.Update(bigFatherProcess);
    }

    // dictionary doesnt have father
    if(toInsert == null && dRel.FatherId == null){
      var to_insert = new Dictionary(dRel.Id, dRel.Name, dRel.Value, ConvertToJsonString(new List<Dictionary>()));
      var tmp = _dictionaryDS.Insert(to_insert);
      toInsert = ProcessJsonString(tmp, (_,_)=>{});
    }*/

    return new DictionaryNoRel();
  }

  // ---
  private DictionaryNoRel ProcessJsonString(Dictionary node, Action<DictionaryNoRel, long?> action, long? fatherId = null){
    var process_father = new DictionaryNoRel(node.Id, node.Name, node.Value, new List<DictionaryNoRel>());

    var childs = new List<Dictionary>();
    var tmp_childs = JsonSerializer.Deserialize<List<Dictionary>>(node.Childs);

    if(tmp_childs == null) return process_father;
    
    action(process_father, fatherId);

    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child, action, node.Id);
      process_father.Childs.Add(child_process);
    });

    return process_father;
  }

  private Dictionary ProcessDictionaryNoRel(DictionaryNoRel dNoRel, Action<DictionaryNoRel> action){
    List<Dictionary> childs = new List<Dictionary>();
    action(dNoRel);    

    dNoRel.Childs.ForEach( child =>{
      var child_process = ProcessDictionaryNoRel(child, action);
      childs.Add(child_process);
    }); 
    return new Dictionary(dNoRel.Id, dNoRel.Name, dNoRel.Value, ConvertToJsonString(childs));
  }
  private string ConvertToJsonString(List<Dictionary> childs){
    return JsonSerializer.Serialize(childs);
  }
}