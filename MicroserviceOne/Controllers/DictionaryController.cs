
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
  public Dictionary InsertDictionaries(Dictionary dictionary){
    return _dictionaryBusinessService.InsertDictionary(dictionary);
  }
}