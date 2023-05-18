using Newtonsoft.Json;
using MicroserviceOne.DataService;

namespace MicroserviceOne.BusinessService;
public class DictionaryBusinessService
{
  private DictionaryDataService _dictionaryDataService;

  public DictionaryBusinessService(DictionaryDataService dictionaryDataService){
    _dictionaryDataService = dictionaryDataService;
  }

  // MAIN ------
  public List<Dictionary> GetDictionaries(){
    List<DictionaryNoRel> dictionariesNoRel = _dictionaryDataService.GetDictionaries();
    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesNoRel, dictionaries, null);
    return dictionaries;
  }
  public Dictionary InsertDictionary(Dictionary newDictionary){
    List<DictionaryNoRel> dictionariesNoRel = _dictionaryDataService.GetDictionaries();
    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesNoRel, dictionaries, null);
    long newId = getNewId(dictionaries);
    newDictionary.Id = newId;

    var bigFather = getBigFather(dictionaries, newDictionary);
    if(bigFather.Id == newDictionary.Id){
      _dictionaryDataService.InsertDictionary(
        new DictionaryNoRel(newDictionary.Id, newDictionary.Name, newDictionary.Value, null)
      );
    } else {
      dictionaries.Add(newDictionary);
      var bigFatherNoRel = toNoRelational(bigFather, dictionaries);
      _dictionaryDataService.UpdateDictionary(bigFatherNoRel);
    }
    return newDictionary;
  } 
  public bool UpdateDictionary(Dictionary dictionary){
    List<DictionaryNoRel> dictionariesNoRel = _dictionaryDataService.GetDictionaries();
    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesNoRel, dictionaries, null);

    bool updated = false;

    var oriChildIndex = dictionaries.FindIndex(rel => rel.Id == dictionary.Id);
    if(oriChildIndex < 0) return updated;
    
    if(dictionaries[oriChildIndex].Name == dictionary.Name || dictionaries[oriChildIndex].Value == dictionary.Value)
      updated = changeContent(dictionary, oriChildIndex, dictionaries);
    if(dictionaries[oriChildIndex].FatherId != dictionary.FatherId)
      updated = changeFather(dictionary, oriChildIndex, dictionaries);

    return updated;
  }

  // EXTRA ------
  private Dictionary getBigFather(List<Dictionary> relList, Dictionary child){
    var father = new Dictionary(child);
    while(father.FatherId != null){
      var posibBigFather = relList.Find(dic => dic.Id == father.FatherId);
      if(posibBigFather != null){
        father = posibBigFather;
      }
      else
        father.FatherId = null;
    }
    return father;
  }
  // ---
  private void ConvertToDictionaries(List<DictionaryNoRel> dictionariesAlfa, List<Dictionary> dictionaries, long? fatherId){
    dictionariesAlfa.ForEach(curDictionary => {
      dictionaries.Add(new Dictionary(curDictionary.Id, curDictionary.Name, curDictionary.Value, fatherId));
      var childs = GetChildList(curDictionary);
      ConvertToDictionaries(childs, dictionaries, curDictionary.Id);
    });
  }
  private List<DictionaryNoRel> GetChildList(DictionaryNoRel father){
    var empty = new List<DictionaryNoRel>();
    if(father.Childs == null) return empty;
    var childs = JsonConvert.DeserializeObject<List<DictionaryNoRel>>(father.Childs);
    return childs == null? empty : childs;
  }
  private long getNewId(List<Dictionary> dictionaries){
    var maxDicId = dictionaries.MaxBy(dic => dic.Id);
    return maxDicId == null ? 0 : maxDicId.Id+1;
  }
  // ------
  private bool changeContent(Dictionary child, int oriChildIndex, List<Dictionary> relList){
    relList[oriChildIndex].Name = child.Name;
    relList[oriChildIndex].Value = child.Value;
    var bigFather = getBigFather(relList, relList[oriChildIndex]);
    var bigFatherNoRel = toNoRelational(bigFather, relList);
    _dictionaryDataService.UpdateDictionary(bigFatherNoRel);
    return true;
  }
  private bool changeFather(Dictionary child, int oriChildIndex, List<Dictionary> relList){
    var lastBigFather = getBigFather(relList, relList[oriChildIndex]);
    var newBigFather = getBigFather(relList, child);
    if(newBigFather.Id == child.Id && child.FatherId != null) return false;

    // case 1: last == new
    // case 2: last != new
    relList[oriChildIndex] = child;
    if(lastBigFather.Id == newBigFather.Id){
      var bigFatherNoRel = toNoRelational(lastBigFather, relList);
      _dictionaryDataService.UpdateDictionary(bigFatherNoRel);
    } else{
      var newBigFatherNoRel = toNoRelational(newBigFather, relList);

      // child's new father is himself, insert new row
      if(newBigFather.Id == child.Id)
        _dictionaryDataService.InsertDictionary(newBigFatherNoRel);
      else
        _dictionaryDataService.UpdateDictionary(newBigFatherNoRel);

      // child's last father is himself and is already added to newFather
      if(lastBigFather.Id == child.Id){
        _dictionaryDataService.DeleteDictionary(lastBigFather.Id);
      } else {
        var lastBigFatherNoRel = toNoRelational(lastBigFather, relList);
        _dictionaryDataService.UpdateDictionary(lastBigFatherNoRel);
      }
    }
    return true;
  }
  private DictionaryNoRel toNoRelational(Dictionary father, List<Dictionary> relList){
    var list = new List<Dictionary>(relList);
    var childs = toNoRelationalChilds(father, list);
    return new DictionaryNoRel(father.Id, father.Name, father.Value, childs);
  }
  private string toNoRelationalChilds(Dictionary bigFather, List<Dictionary> relList){
    var childsRel = relList.FindAll(rel => rel.FatherId == bigFather.Id);
    relList.RemoveAll(rel => rel.FatherId == bigFather.Id);
    List<DictionaryNoRel> childsNoRel = new List<DictionaryNoRel>();
    childsRel.ForEach(child => {
      var strChilds = toNoRelationalChilds(child, relList);
      childsNoRel.Add(new DictionaryNoRel(child.Id, child.Name, child.Value, strChilds));
    });
    return JsonConvert.SerializeObject(childsNoRel);
  }
  // ------
}
