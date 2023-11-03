using LogicAPI.Models;

namespace LogicAPI.DataService;
public class DictionaryDataService
{
  private Context _context;

  public DictionaryDataService(Context context){
    _context = context;
  }

  public bool CanConnect(){
    return _context.Database.CanConnect();
  }
  public List<SerializedDictionary> Get(){
    var tmp = _context.Dictionaries.ToList();
    return tmp;
  }
  public SerializedDictionary Insert(SerializedDictionary dictionary){
    _context.Dictionaries.Add(dictionary);
    _context.SaveChanges();
    return dictionary;
  }
  public SerializedDictionary? Update(SerializedDictionary dictionary){
    var dicDbo = _context.Dictionaries.Find(dictionary.Id);
    if(dicDbo == null) return null;

    dicDbo.Name = dictionary.Name;
    dicDbo.Value = dictionary.Value;
    dicDbo.Children = dictionary.Children;
    _context.SaveChanges();
    return dictionary;
  }
  public bool Delete(long id){
    var dicDbo = _context.Dictionaries.Find(id);
    if(dicDbo==null) return false;

    _context.Dictionaries.Remove(dicDbo);
    _context.SaveChanges();
    return true;
  }
}