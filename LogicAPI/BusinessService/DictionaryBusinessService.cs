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
      trees.Add(ProcessJsonString(tree, (_)=>false, (_) => {}));
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
        father => {
          // get a validId
          if(father.Id == validId) validId++;
          return father.Id == dRel.FatherId;
        },
        (father) => {
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
        father => {
          return father.Id == dRel.FatherId;
        },
        (father) => {
          father.Childs.Add(toInsert);
        }
      );
      _dictionaryDS.Update(bigFatherProcess);
    }

    // dictionary doesnt have father
    if(toInsert == null && dRel.FatherId == null){
      var to_insert = new Dictionary(validId, dRel.Name, dRel.Value, ConvertToJsonString(new List<Dictionary>()));
      var tmp = _dictionaryDS.Insert(to_insert);
      toInsert = ProcessJsonString(tmp, (_)=>false, (_)=>{});
    }
    return toInsert;
  }

  // ---
  private DictionaryNoRel ProcessJsonString(Dictionary father, Func<Dictionary, bool> condition, Action<Dictionary> action){
    var process_father = new DictionaryNoRel(father.Id, father.Name, father.Value, new List<DictionaryNoRel>());

    var childs = new List<Dictionary>();
    var tmp_childs = JsonSerializer.Deserialize<List<Dictionary>>(father.Childs);

    if(tmp_childs == null) return process_father;
    
    if(condition(father)) action(father);

    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child, condition, action);
      process_father.Childs.Add(child_process);
    });

    father.Childs = ConvertToJsonString(childs);
    return process_father;
  }

  private Dictionary ProcessDictionaryNoRel(DictionaryNoRel dNoRel, Func<DictionaryNoRel, bool> condition, Action<DictionaryNoRel> action){
    List<Dictionary> childs = new List<Dictionary>();
    if(condition(dNoRel)) action(dNoRel);    

    dNoRel.Childs.ForEach( child =>{
      var child_process = ProcessDictionaryNoRel(child, condition, action);
      childs.Add(child_process);
    }); 
    return new Dictionary(dNoRel.Id, dNoRel.Name, dNoRel.Value, ConvertToJsonString(childs));
  }
  private string ConvertToJsonString(List<Dictionary> childs){
    return JsonSerializer.Serialize(childs);
  }
}