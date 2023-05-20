
using Microsoft.AspNetCore.Mvc;
using MicroserviceOne.BusinessService;

namespace MicroserviceOne.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DictionaryController: ControllerBase
{
  private DictionaryBusinessService _dictionaryBusinessService;
  public DictionaryController(DictionaryBusinessService dictionaryBusinessService){
    _dictionaryBusinessService = dictionaryBusinessService;
  }

  [HttpGet]
  public List<Dictionary> GetDictionaries(){
    return _dictionaryBusinessService.GetDictionaries();
  }
  [HttpPost]
  public Dictionary? InsertDictionary(Dictionary dictionary){
    return _dictionaryBusinessService.InsertDictionary(dictionary);
  }
  [HttpGet("{id:int}")]
  public Dictionary? GetDictionary(int id){
    return _dictionaryBusinessService.GetDictionary(id);
  }
  [HttpPut("{id:int}")]
  public bool UpdateDictionary(int id, Dictionary dictionary){
    dictionary.Id = id;
    return _dictionaryBusinessService.UpdateDictionary(dictionary);
  }
  [HttpDelete("{id:int}")]
  public bool DeleteDictionary(int id){
    return _dictionaryBusinessService.DeleteDictionary(id);
  }
}