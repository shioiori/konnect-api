using Konnect.API.Infrustructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.ChangeNewsState
{
  public class ChangePostStateHandler : IRequestHandler<ChangePostStateCommand, ChangePostStateResponse>
  {
    private readonly IPostRepository _postRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly NotificationManager _notificationManager;
    public ChangePostStateHandler(IPostRepository postRepository, NotificationManager notificationManager, IGroupRepository groupRepository)
    {
      _postRepository = postRepository;
      _notificationManager = notificationManager;
      _groupRepository = groupRepository;
    }

    public async Task<ChangePostStateResponse> Handle(ChangePostStateCommand request, CancellationToken cancellationToken)
    {
      var post = _postRepository.GetPost(request.PostId);
      int oldState = post.Approved;
      if (post == null)
      {
        return new ChangePostStateResponse()
        {
          Success = false,
          Type = ResponseType.Error,
          Message = "Không tìm thấy tin"
        };
      }
      _postRepository.ChangePostState(request.PostId, request.State);
      if (oldState != (int)request.State)
      {
        if (request.State == ApproveState.Accept)
        {
          var group = _groupRepository.GetGroup(post.GroupId);
          await _notificationManager.NotifyPostAcceptAsync(post, request, request.Message);
          _notificationManager.NotifyNewPost(post, group, request);
        }
        else if (request.State == ApproveState.Reject)
        {
          await _notificationManager.NotifyPostRejectAsync(post, request, request.Message);
        }
        else
        {
          await _notificationManager.NotifyPostPendingAsync(post, request);
        }
      }
      return new ChangePostStateResponse()
      {
        Success = true,
        Type = ResponseType.Success,
        Message = "Cập nhật thành công"
      };
    }
  }
}
