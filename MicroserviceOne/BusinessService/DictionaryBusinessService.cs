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
  public Dictionary InsertDictionary(Dictionary dictionary){
    if(dictionary.FatherId==null){
      _dictionaryDataService.InsertDictionary(new DictionaryAlfa(-1, dictionary.Name, dictionary.Value, null));
    } else {
      List<DictionaryAlfa> dictionariesAlfa = _dictionaryDataService.GetDictionaries();
      List<Dictionary> dictionaries = new List<Dictionary>();
      ConvertToDictionaries(dictionariesAlfa, dictionaries, null);
    }
    return dictionary;
  } 

  // private functions
  // 1: Buscar padre de nuevo diccionario en diccionarios.
  // 2: Obtener padre de padres y guardar profundidad(camino)
  // 3: obtener
  private void convertToDictionariesAlfa(){
    
  }
  private void ConvertToDictionaries(List<DictionaryAlfa> dictionariesAlfa, List<Dictionary> dictionaries, long? fatherId){
    dictionariesAlfa.ForEach(dAlfa => {
      dictionaries.Add(new Dictionary(dAlfa.Id, dAlfa.Name, dAlfa.Value, fatherId));

      if(dAlfa.Childs != null)
      {
        var childs = JsonConvert.DeserializeObject<List<DictionaryAlfa>>(dAlfa.Childs);
        ConvertToDictionaries(childs, dictionaries, dAlfa.Id);
      }
    });
  }
}

public class MyData
{
     public List<DictionaryAlfa> data { get; set; }
}