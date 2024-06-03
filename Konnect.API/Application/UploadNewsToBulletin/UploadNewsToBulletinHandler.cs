using Konnect.API.Infrustructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
  public class UploadNewsToBulletinHandler : IRequestHandler<UploadNewsToBulletinCommand, UploadNewsToBulletinResponse>
  {
    private readonly IPostRepository _postRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly NotificationManager _notificationManager;
    private readonly IMailHandler _mailHandler;
    public UploadNewsToBulletinHandler(IPostRepository postRepository, 
      NotificationManager notificationManager, 
      IGroupRepository groupRepository,
      IUserRepository userRepository,
      IMailHandler mailHandler)
    {
      _postRepository = postRepository;
      _notificationManager = notificationManager;
      _groupRepository = groupRepository;
      _userRepository = userRepository;
      _mailHandler = mailHandler;
    }
    public async Task<UploadNewsToBulletinResponse> Handle(UploadNewsToBulletinCommand request, CancellationToken cancellationToken)
    {
      try
      {
        var post = new Bulletin()
        {
          Content = request.Content,
          GroupId = request.GroupId,
          CreatedDate = DateTime.UtcNow,
          CreatedBy = request.UserName
        };
        _postRepository.AddPost(post);
        var group = _groupRepository.GetGroup(request.GroupId);
        if (request.RoleName == GroupRole.User.ToString())
        {
          _postRepository.ChangePostState(post.Id, ApproveState.Pending);
          await _notificationManager.NotifyPostPendingAsync(post, request);
        }
        if (request.RoleName == GroupRole.Manager.ToString())
        {
          _postRepository.ChangePostState(post.Id, ApproveState.Accept);
          var users = _userRepository.GetUsers(request.GroupId);
          foreach (var user in users)
          {
            // send mail to everyone in group
            _mailHandler.Send(new MailContent()
            {
              To = user.Email,
              Subject = "Thông báo mới: " + request.Content.Substring(0, 32) + "...",
              Body = request.Content
            });
          }
        }
        _notificationManager.NotifyNewPost(post, group, request);
        return new UploadNewsToBulletinResponse()
        {
          Success = true,
          Type = ResponseType.Success,
          Message = request.RoleName == GroupRole.Manager.ToString()
                    ? "Đăng tin thành công"
                    : "Tin của bạn sẽ được đăng sau khi quản lý chấp nhận"
        };
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
  }
}
