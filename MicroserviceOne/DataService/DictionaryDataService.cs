using MicroserviceOne.Models;

namespace MicroserviceOne.DataService;
public class DictionaryDataService
{
  private Context _context;

  public DictionaryDataService(Context context){
    _context = context;
  }

  public List<DictionaryNoRel> GetDictionaries(){
    return _context.Dictionaries.ToList();
  } 
  public DictionaryNoRel InsertDictionary(DictionaryNoRel dictionary){
    _context.Dictionaries.Add(dictionary);
    _context.SaveChanges();
    return dictionary;
  }
  public DictionaryNoRel? UpdateDictionary(DictionaryNoRel dictionary){
    var dicDbo = _context.Dictionaries.Find(dictionary.Id);
    if(dicDbo == null) return null;

    dicDbo.Name = dictionary.Name;
    dicDbo.Value = dictionary.Value;
    dicDbo.Childs = dictionary.Childs;
    _context.SaveChanges();
    return dictionary;
  }
  public bool DeleteDictionary(long id){
    var dicDbo = _context.Dictionaries.Find(id);
    if(dicDbo==null) return false;

    _context.Dictionaries.Remove(dicDbo);
    _context.SaveChanges();
    return true;
  }
}