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
      trees.Add(ProcessJsonString(tree, (_,_)=>{}, (_,_)=>{}));
    });

    return trees;
  }
  public DictionaryRel? Insert(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();

    long validId = dRel.Id >= 0? dRel.Id:0;
    bool done = false;
    DictionaryNoRel? toUpdate = null;

    jsonString_trees.ForEach( tree => {
      bool update = false;
      var bigFather = ProcessJsonString(
        tree,
        (father, _) => {
          if(father.Id >= validId)
            validId = father.Id+1;
          if(father.Id == dRel.FatherId) update = true;
        },
        (_,_)=>{}
      );
      if(update) toUpdate = bigFather;
    });
    
    // dictionary have a father
    if(dRel.FatherId == null){
      _dictionaryDS.Insert(new Dictionary(validId, dRel.Name, dRel.Value, "[]"));
      done = true;
    }
    else if(toUpdate != null){
      var bigFatherProcess = ProcessDictionaryNoRel(toUpdate,
        (father) => {
          if(father.Id == dRel.FatherId)
          father.Childs.Add(new DictionaryNoRel(validId, dRel.Name, dRel.Value, new List<DictionaryNoRel>()));
        },
        (_)=>{}
      );
      _dictionaryDS.Update(bigFatherProcess);
      done = true;
    }
    dRel.Id = validId;
    return done? dRel: null;
  }

  public DictionaryNoRel? Update(DictionaryRel dRel){
    bool nodeFound = false;

    var jsonString_trees = _dictionaryDS.Get();
    DictionaryNoRel? toInsert = null;
    DictionaryNoRel? lastBigFather = null;
    DictionaryNoRel? curBigFather = null;
    long? lastFatherId = -1;
    long? curFatherId = -1;

    jsonString_trees.ForEach( tree => {
      bool isCurBigFather = false;
      bool isLastBigFather = false;
      var bigFather = ProcessJsonString(
        tree,
        (node, fatherId) => {
          if(node.Id == dRel.Id)
          {
            nodeFound = true;
            node.Name = dRel.Name; node.Value = dRel.Value;
            if(fatherId == dRel.FatherId) isCurBigFather = true;
            // if father changed, need to update last father
            if(fatherId != dRel.FatherId) {
              lastFatherId = fatherId;
              isLastBigFather = true;
            }
          }
          // search curFather
          if(node.Id == dRel.FatherId) {
            isCurBigFather = true;
            curFatherId = dRel.FatherId;
          }
        },
        (node, fatherId)=>{
          if(node.Id == dRel.Id){
            toInsert = node;
          };
        }
      );
      if(lastFatherId > -1 && isLastBigFather) lastBigFather = bigFather;
      if(isCurBigFather) curBigFather = bigFather;
    });
    
    // updates
    if(curBigFather != null && lastBigFather == null){
      var bigFatherProcess = ProcessDictionaryNoRel(curBigFather, (_)=>{}, (_)=>{});
      _dictionaryDS.Update(bigFatherProcess);
    }
    // change father
    if(curBigFather != null && lastBigFather != null && toInsert != null){
    Console.WriteLine($"- {lastFatherId} - {curFatherId}");
    Console.WriteLine($"- {curBigFather.Id} - {lastBigFather.Id}");
      // changes within the same bigfather
      if(curBigFather.Id == lastBigFather.Id){
        var bigFatherProcess = ProcessDictionaryNoRel(curBigFather, (father) => {
          if (father.Id == lastFatherId){
            father.Childs.RemoveAll(item => item.Id == dRel.Id);
          }
          if (father.Id == curFatherId){
            father.Childs.Add(toInsert);
          }
        }, (_) => {});
        _dictionaryDS.Update(bigFatherProcess);
      }
      // changes occurs in different bigfathers 
      else {
        var curFatherProcess = ProcessDictionaryNoRel(lastBigFather, (father) => {
          if(father.Id == curFatherId) father.Childs.Add(toInsert);
        }, (_) => {});
        _dictionaryDS.Update(curFatherProcess);
        if(lastBigFather.Id == dRel.Id){
          _dictionaryDS.Delete(dRel.Id);
        }
        else{
          var lastFatherProcess = ProcessDictionaryNoRel(lastBigFather, (father) => {
            if(father.Id == lastFatherId) father.Childs.RemoveAll(item => item.Id == dRel.Id);
          }, (_) => {});
          _dictionaryDS.Update(lastFatherProcess);
        }
      }
    }

    return toInsert;
  }

  // ---
  private DictionaryNoRel ProcessJsonString(Dictionary node, Action<DictionaryNoRel, long?> action1, Action<DictionaryNoRel, long?> action2, long? fatherId = null){
    var process_father = new DictionaryNoRel(node.Id, node.Name, node.Value, new List<DictionaryNoRel>());

    var childs = new List<Dictionary>();
    var tmp_childs = JsonSerializer.Deserialize<List<Dictionary>>(node.Childs);

    if(tmp_childs == null) return process_father;
    
    action1(process_father, fatherId);

    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child, action1, action2, node.Id);
      process_father.Childs.Add(child_process);
    });

    action2(process_father, fatherId);
    return process_father;
  }

  private Dictionary ProcessDictionaryNoRel(DictionaryNoRel dNoRel, Action<DictionaryNoRel> action1, Action<Dictionary> action2){
    List<Dictionary> childs = new List<Dictionary>();
    action1(dNoRel);    

    dNoRel.Childs.ForEach( child =>{
      var child_process = ProcessDictionaryNoRel(child, action1, action2);
      childs.Add(child_process);
    }); 
    var father = new Dictionary(dNoRel.Id, dNoRel.Name, dNoRel.Value, ConvertToJsonString(childs));
    action2(father);
    return father;
  }
  private string ConvertToJsonString(List<Dictionary> childs){
    return JsonSerializer.Serialize(childs);
  }
}