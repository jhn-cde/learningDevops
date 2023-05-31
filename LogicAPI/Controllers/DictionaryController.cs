using Microsoft.AspNetCore.Mvc;
using LogicAPI.BusinessService;

namespace LogicAPI.Controllers;

[ApiController]
[Route("logicapi/[controller]")]
public class DictionaryController : ControllerBase
{

    private DictionaryBusinessService _dictionaryBS;
    public DictionaryController(DictionaryBusinessService dictionaryBS)
    {
      _dictionaryBS = dictionaryBS;
    }

    [HttpGet]
    public string CanConnect()
    {
        return _dictionaryBS.CanConnect();
    }
    [HttpGet("GetAll")]
    public IEnumerable<DictionaryNoRel> Get()
    {
        return _dictionaryBS.Get();
    }
    [HttpPost("Insert")]
    public DictionaryRel? Insert(DictionaryRel dictionaryRel)
    {
        return _dictionaryBS.Insert(dictionaryRel);
    }
    [HttpPost("Update/{id}")]
    public DictionaryNoRel? Update(long id, DictionaryRel dictionaryRel)
    {
        dictionaryRel.Id = id;
        return _dictionaryBS.Update(dictionaryRel);
    }
}
