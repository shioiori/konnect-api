﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTCClassSupport.API.Application.ImportExcel;
using UTCClassSupport.API.Application.ImportTimetable;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Controllers
{
    [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer")]
  [Route("import")]  
  public class ImportController : BaseController
  {
    private readonly IMediator _mediator;
    public ImportController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("user")]
    public async Task<ImportUserToDatabaseResponse> ImportUser([FromBody]ImportUserDTO dto)
    {
      var import = CustomMapper.Mapper.Map<ImportUserToDatabaseCommand>(dto);
      return await _mediator.Send(import);
    }

    [HttpPost("timetable")]
    public async Task<ImportTimetableResponse> ImportTimetable([FromBody]ImportTimetableDTO dto)
    {
      var data = ReadJWTToken();
      var command = new ImportTimetableCommand();
      CustomMapper.Mapper.Map<BaseRequest, ImportTimetableCommand>(data, command);
      CustomMapper.Mapper.Map<ImportTimetableDTO, ImportTimetableCommand>(dto, command);
      return await _mediator.Send(command);
    }
  }
}
