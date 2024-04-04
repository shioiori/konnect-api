using MediatR;
using Microsoft.AspNetCore.Mvc;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
    [ApiController]
  public class ImportController : ControllerBase
  {
    private readonly IMediator _mediator;
    public ImportController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost]
    public async Task<ImportUserToDatabaseResponse> ImportUser(ImportUserDTO dto)
    {
      var import = CustomMapper.Mapper.Map<ImportUserToDatabaseCommand>(dto);
      return await _mediator.Send(import);
    }

    [HttpPost]
    public async Task<ImportTimetableResponse> ImportTimetable(string username, string groupId)
    {
      return await _mediator.Send(new ImportTimetableCommand()
      {
        UserName = username,
        GroupId = groupId
      });
    }
  }
}
