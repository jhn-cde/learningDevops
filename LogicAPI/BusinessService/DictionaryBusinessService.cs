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
  public IEnumerable<Dictionary> Get(){
    var jsonString_trees = _dictionaryDS.Get();
    var trees = new List<Dictionary>();
    jsonString_trees.ForEach(tree => {
      trees.Add(ProcessJsonString(tree, (_,_)=>{}, (_,_)=>{}));
    });

    return trees;
  }
  public Dictionary? GetId(long id){
    var jsonString_trees = _dictionaryDS.Get();
    Dictionary? found = null;
    int i = 0;
    while(found == null &&  i < jsonString_trees.Count()) {
      var tree = jsonString_trees[i];
      ProcessJsonString(tree, (_,_)=>{}, (node,_)=>{
        if(node.Id == id) found = node;
      });
      i++;
    }
    return found;
  }
  public DictionaryRel? Insert(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();

    long validId = dRel.Id >= 0? dRel.Id:0;
    bool done = false;
    Dictionary? toUpdate = null;

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
      _dictionaryDS.Insert(new SerializedDictionary(validId, dRel.Name, dRel.Value, "[]"));
      done = true;
    }
    else if(toUpdate != null){
      var bigFatherProcess = ProcessDictionaryNoRel(toUpdate,
        (father) => {
          if(father.Id == dRel.FatherId)
          father.Children.Add(new Dictionary(validId, dRel.Name, dRel.Value, new List<Dictionary>()));
        },
        (_)=>{}
      );
      _dictionaryDS.Update(bigFatherProcess);
      done = true;
    }
    dRel.Id = validId;
    return done? dRel: null;
  }

  public Dictionary? Update(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();
    Dictionary? toInsert = null;
    Dictionary? lastBigFather = null;
    Dictionary? curBigFather = null;
    long? lastFatherId = -1;
    long? curFatherId = -1;

    jsonString_trees.ForEach( tree => {
      bool isLastBigFather = false; bool isCurBigFather = false; 
      var bigFather = ProcessJsonString(
        tree,
        (_,_)=>{},
        (node, fatherId)=>{
          if(node.Id == dRel.Id){
            node.Name = dRel.Name; node.Value = dRel.Value;
            if((node.Name != dRel.Name || node.Value != dRel.Value) && fatherId == dRel.FatherId){
              isCurBigFather = true;
            }
            if(fatherId != dRel.FatherId){
              isLastBigFather = true;
              lastFatherId = fatherId;
              toInsert = node;
            }
          }
          if(node.Id == dRel.FatherId){
            isCurBigFather = true;
            curFatherId = node.Id;
          }
        }
      );
      if(isLastBigFather) lastBigFather = bigFather;
      if(isCurBigFather) curBigFather = bigFather;
    });
    
    // updates
    if(curBigFather != null && lastBigFather == null){
      var bigFatherProcess = ProcessDictionaryNoRel(curBigFather, (_)=>{}, (_)=>{});
      _dictionaryDS.Update(bigFatherProcess);
    }
    // change father
    if(lastBigFather != null && toInsert != null){
      // 1. child to new bigfather
      if(dRel.FatherId == null){
        Console.WriteLine("Case 1");
        var bigFatherProcess = ProcessDictionaryNoRel(lastBigFather, (father) => {
          if (father.Id == lastFatherId)
            father.Children.RemoveAll(item => item.Id == dRel.Id);
        }, (_) => {});
        _dictionaryDS.Update(bigFatherProcess);
        _dictionaryDS.Insert(ProcessDictionaryNoRel(toInsert,(_)=>{},(_)=>{}));
      }
      // 2. change childs big father
      else if(curBigFather != null && curBigFather.Id != lastBigFather.Id && lastFatherId != null){
        Console.WriteLine("Case 2");
        var lastBigFatherProcess = ProcessDictionaryNoRel(lastBigFather, (father)=> {
          if(father.Id == lastFatherId)
            father.Children.RemoveAll(item => item.Id == dRel.Id);
        },(_)=>{});
        _dictionaryDS.Update(lastBigFatherProcess);
        var curBigFatherProcess = ProcessDictionaryNoRel(curBigFather, (father)=> {
          if(father.Id == curFatherId)
            father.Children.Add(toInsert);
        },(_)=>{});
        _dictionaryDS.Update(curBigFatherProcess);
      }
      // 3. transform bigfather to child of other bigfather
      else if(lastBigFather.Id == dRel.Id && curBigFather != null && curBigFather.Id != lastBigFather.Id){
        Console.WriteLine("Case 3");
        _dictionaryDS.Delete(dRel.Id);
        var curBigFatherProcess = ProcessDictionaryNoRel(curBigFather, (father)=> {
          if(father.Id == curFatherId){
            father.Children.Add(toInsert);
          }
        },(_)=>{});
        _dictionaryDS.Update(curBigFatherProcess);
      }
      // 4. Change childs father but not bigfather (same tree)
      else if(curBigFather != null && curBigFather.Id == lastBigFather.Id){
        Console.WriteLine("Case 4");
        var curBigFatherProcess = ProcessDictionaryNoRel(curBigFather, (father)=> {
          if(father.Id == lastFatherId)
            father.Children.RemoveAll(item => item.Id == dRel.Id);
          if(father.Id == curFatherId)
            father.Children.Add(toInsert);
        },(_)=>{});
        _dictionaryDS.Update(curBigFatherProcess);
      }
    }

    return toInsert;
  }

  public bool Delete(long id){
    var jsonString_trees = _dictionaryDS.Get();
    bool done = false;
    long? fatherId = null;
    Dictionary? bigFather = null;

    jsonString_trees.ForEach( tree => {
      bool isCurTree = false;
      var bFather = ProcessJsonString(
        tree,
        (_,_) => { },
        (node, fId)=>{
          if(node.Id == id){
            fatherId = fId;
            isCurTree = true;
          }
        }
      );
      if(isCurTree) bigFather = bFather; 
    });
    if(bigFather != null){
      // 1. delete bigfather
      if(fatherId == null && id == bigFather.Id) {
        _dictionaryDS.Delete(id);
        done = true;
      }
      // 2. delete child
      if(fatherId != null){
        var bigFatherProcess = ProcessDictionaryNoRel(bigFather, (father)=> {
          if(father.Id == fatherId)
            father.Children.RemoveAll(item => item.Id == id);
        },(_)=>{});
        _dictionaryDS.Update(bigFatherProcess);
        done = true;
      }
    }
    return done;
  }

  // ---
  private Dictionary ProcessJsonString(SerializedDictionary node, Action<Dictionary, long?> action1, Action<Dictionary, long?> action2, long? fatherId = null){
    var process_father = new Dictionary(node.Id, node.Name, node.Value, new List<Dictionary>());

    var childs = new List<SerializedDictionary>();
    var tmp_childs = JsonSerializer.Deserialize<List<SerializedDictionary>>(node.Children);

    if(tmp_childs == null) return process_father;
    
    action1(process_father, fatherId);

    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child, action1, action2, node.Id);
      process_father.Children.Add(child_process);
    });

    action2(process_father, fatherId);
    return process_father;
  }

  private SerializedDictionary ProcessDictionaryNoRel(Dictionary dNoRel, Action<Dictionary> action1, Action<SerializedDictionary> action2){
    List<SerializedDictionary> childs = new List<SerializedDictionary>();
    action1(dNoRel);    

    dNoRel.Children.ForEach( child =>{
      var child_process = ProcessDictionaryNoRel(child, action1, action2);
      childs.Add(child_process);
    }); 
    var father = new SerializedDictionary(dNoRel.Id, dNoRel.Name, dNoRel.Value, ConvertToJsonString(childs));
    action2(father);
    return father;
  }
  private string ConvertToJsonString(List<SerializedDictionary> childs){
    return JsonSerializer.Serialize(childs);
  }
}