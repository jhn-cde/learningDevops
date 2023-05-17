using MicroserviceOne.Models;

namespace MicroserviceOne.DataService;
public class DictionaryDataService
{
  private Context _context;

  public DictionaryDataService(Context context){
    _context = context;
  }

  public List<DictionaryAlfa> GetDictionaries(){
    return _context.Dictionaries.ToList();
  } 
  public DictionaryAlfa InsertDictionary(DictionaryAlfa dictionary){
    _context.Dictionaries.Add(dictionary);
    _context.SaveChanges();
    return dictionary;
  }
  public DictionaryAlfa? UpdateDictionary(DictionaryAlfa dictionary){
    Console.WriteLine($"{dictionary.Id}: {dictionary.Childs}");
    var dicDbo = _context.Dictionaries.Find(dictionary.Id);
    if(dicDbo == null) return null;

    dicDbo.Name = dictionary.Name;
    dicDbo.Value = dictionary.Value;
    dicDbo.Childs = dictionary.Childs;
    _context.SaveChanges();
    return dictionary;
  }
}