using Konnect.API.Infrustructure.Repositories;
using MediatR;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Responses;

namespace Konnect.API.Application.AddGroup
{
  public class AddGroupHandler : IRequestHandler<AddGroupCommand, Response>
  {
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    public AddGroupHandler(IGroupRepository groupRepository, IUserRepository userRepository)
    {
      _groupRepository = groupRepository;
      _userRepository = userRepository;
    }
    public Task<Response> Handle(AddGroupCommand request, CancellationToken cancellationToken)
    {
      _groupRepository.AddGroup(request.Group, request.UserId);
      return Task.FromResult(new Response()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Thêm nhóm thành công"
      });
    }
  }
}
