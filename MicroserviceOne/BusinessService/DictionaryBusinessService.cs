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
    List<DictionaryAlfa> dictionariesAlfa = _dictionaryDataService.GetDictionaries();

    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesAlfa, dictionaries, null);
    return dictionaries;
  }
  public Dictionary InsertDictionary(Dictionary newDictionary){
    List<DictionaryAlfa> dictionariesAlfa = _dictionaryDataService.GetDictionaries();
    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesAlfa, dictionaries, null);
    long newId = getNewId(dictionaries);
    newDictionary.Id = newId;

    // search for big father in db
    var bigFatherRoute = getBigFatherRoute(dictionaries, newDictionary);

    // if bigFatherAlfa is null, add bigFather to new row
    if(bigFatherRoute.Count() == 0)
      _dictionaryDataService.InsertDictionary(new DictionaryAlfa(newDictionary.Id, newDictionary.Name, newDictionary.Value, null));
    else{
      int posRoute = bigFatherRoute.Count()-1;
      long bigFatherId = bigFatherRoute[posRoute];
      var bigFatherAlfa = dictionariesAlfa.Find(dic => dic.Id == bigFatherId);
      if(bigFatherAlfa != null){
        var func = (List<DictionaryAlfa> childs, Dictionary child, DictionaryAlfa father) => {
          childs.Add(new DictionaryAlfa(child.Id, child.Name, child.Value, null));
        };
        var bigFatherAlfaMod = addDictionaryToBigAlfa(bigFatherAlfa, bigFatherRoute, newDictionary, posRoute-1, func);
        _dictionaryDataService.UpdateDictionary(bigFatherAlfaMod);
      }else{
        Console.WriteLine("ERROR!");
      }
    }
    return newDictionary;
  } 
  public bool UpdateDictionary(Dictionary dictionary){
    List<DictionaryAlfa> dictionariesAlfa = _dictionaryDataService.GetDictionaries();
    List<Dictionary> dictionaries = new List<Dictionary>();
    ConvertToDictionaries(dictionariesAlfa, dictionaries, null);

    int toupIndex = dictionaries.FindIndex(dic => dic.Id == dictionary.Id);
    if(toupIndex < 0) return false;

    var bigFatherRoute = getBigFatherRoute(dictionaries, dictionaries[toupIndex]);
    bigFatherRoute.Insert(0, dictionary.Id);

    int posRoute = bigFatherRoute.Count()-1;
    long bigFatherId = bigFatherRoute[posRoute];
    var bigFatherAlfa = dictionariesAlfa.Find(dic => dic.Id == bigFatherId);

    var func = (List<DictionaryAlfa> list, Dictionary toUpd, DictionaryAlfa toUpdAlfa) => {
      toUpdAlfa.Name = toUpd.Name;
      toUpdAlfa.Value = toUpd.Value;
    };
    var bigFatherAlfaMod = addDictionaryToBigAlfa(bigFatherAlfa, bigFatherRoute, dictionary, posRoute-1, func);
    _dictionaryDataService.UpdateDictionary(bigFatherAlfaMod);

    return false;
  }

  // private functions
  private List<long> getBigFatherRoute(List<Dictionary> dictionaries, Dictionary child){
    var father = child;
    List<long> route = new List<long>();
    while(father.FatherId != null){
      var posibFather = dictionaries.Find(dic => dic.Id == father.FatherId);
      if(posibFather != null){
        father = posibFather;
        route.Add(father.Id);
      }
      else
        father.FatherId = null;
    }
    return route;
  }
  //
  private DictionaryAlfa addDictionaryToBigAlfa(DictionaryAlfa bigFather, List<long> route, Dictionary newChild, int posRoute, Action<List<DictionaryAlfa>, Dictionary, DictionaryAlfa> func){
    //
    var childs = GetChilds(bigFather);
    if (posRoute < 0)
      func(childs, newChild, bigFather);
      
    else{
      int fatherIndex = childs.FindIndex(dic => dic.Id == route[posRoute]);
      childs[fatherIndex] = addDictionaryToBigAlfa(childs[fatherIndex], route, newChild, posRoute-1, func);
    }
    //
    string strChilds = SetChilds(childs); 
    bigFather.Childs = strChilds;
    return bigFather;
  }
  // ---
  private void ConvertToDictionaries(List<DictionaryAlfa> dictionariesAlfa, List<Dictionary> dictionaries, long? fatherId){
    dictionariesAlfa.ForEach(curDictionary => {
      dictionaries.Add(new Dictionary(curDictionary.Id, curDictionary.Name, curDictionary.Value, fatherId));
      var childs = GetChilds(curDictionary);
      ConvertToDictionaries(childs, dictionaries, curDictionary.Id);
    });
  }
  private List<DictionaryAlfa> GetChilds(DictionaryAlfa father){
    var empty = new List<DictionaryAlfa>();
    if(father.Childs == null) return empty;
    var childs = JsonConvert.DeserializeObject<List<DictionaryAlfa>>(father.Childs);
    return childs == null? empty : childs;
  }
  private string SetChilds(List<DictionaryAlfa> childs){
    var strChilds = JsonConvert.SerializeObject(childs);
    return strChilds;
  }
  private long getNewId(List<Dictionary> dictionaries){
    var maxDicId = dictionaries.MaxBy(dic => dic.Id);
    return maxDicId == null ? 0 : maxDicId.Id+1;
  }
}

public class MyData
{
     public List<DictionaryAlfa> data { get; set; }
}