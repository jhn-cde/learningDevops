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
      trees.Add(ProcessJsonString(tree));
    });

    return trees;
  }
  public DictionaryNoRel? Insert(DictionaryRel dRel){
    var jsonString_trees = _dictionaryDS.Get();
    // dictionary doesnt have father
    if(dRel.FatherId == null){
      var to_insert = new Dictionary(dRel.Id, dRel.Name, dRel.Value, ConvertToJsonString(new List<Dictionary>()));
      var inserted = _dictionaryDS.Insert(to_insert);
      return ProcessJsonString(inserted);
    }
    // dictionary has fatherId
    else {
      Dictionary? inserted = null;
      int i = 0;
      while(i < jsonString_trees.Count() && inserted == null){
        var tree = jsonString_trees[i];
        var bigFather = ProcessJsonString(
          tree,
          father => father.Id == dRel.FatherId,
          (father, childs) => {
            inserted = new Dictionary(dRel.Id, dRel.Name, dRel.Value, ConvertToJsonString(new List<Dictionary>())); 
            childs.Add(inserted);
          }
        );
        if(inserted != null) Console.WriteLine($"{bigFather.Id}: {inserted.Id} - {inserted.Childs}");//_dictionaryDS.Update(bigFather);
        i++;
      }

      return inserted == null? null: new DictionaryNoRel(dRel.Id, dRel.Name, dRel.Value, new List<DictionaryNoRel>());
    }
  }

  // ---
  private DictionaryNoRel ProcessJsonString(Dictionary father){
    var process_father = new DictionaryNoRel(father.Id, father.Name, father.Value, new List<DictionaryNoRel>());
    var tmp_childs = JsonSerializer.Deserialize<List<Dictionary>>(father.Childs);

    if(tmp_childs == null) return process_father;
    
    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child);
      process_father.Childs.Add(child_process);
    });

    return process_father;
  }
  private Dictionary ProcessJsonString(Dictionary father, Func<Dictionary, bool> condition, Action<Dictionary, List<Dictionary>> action){
    
    var childs = new List<Dictionary>();
    var tmp_childs = JsonSerializer.Deserialize<List<Dictionary>>(father.Childs);

    if(tmp_childs == null) return father;
    
    if(condition(father)){
      action(father, tmp_childs);
      return father;
    }

    tmp_childs.ForEach(child => {
      var child_process = ProcessJsonString(child, condition, action);
      childs.Add(child_process);
    });

    father.Childs = ConvertToJsonString(childs);
    return father;
  }


  private Dictionary ProcessDictionaryNoRel(DictionaryNoRel dNoRel){
    List<Dictionary> childs = new List<Dictionary>();
    dNoRel.Childs.ForEach( child =>{
      var child_process = ProcessDictionaryNoRel(child);
      childs.Add(child_process);
    }); 
    return new Dictionary(dNoRel.Id, dNoRel.Name, dNoRel.Value, ConvertToJsonString(childs));
  }
  private string ConvertToJsonString(List<Dictionary> childs){
    return JsonSerializer.Serialize(childs);
  }
}