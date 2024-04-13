﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UploadNewsToBulletin
{
  public class UploadNewsToBulletinHandler : IRequestHandler<UploadNewsToBulletinCommand, UploadNewsToBulletinResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public UploadNewsToBulletinHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<UploadNewsToBulletinResponse> Handle(UploadNewsToBulletinCommand request, CancellationToken cancellationToken)
    {
      var news = new Bulletin()
      {
        Content = request.Content,
        GroupId = request.GroupId,
        CreatedDate = DateTime.UtcNow,
        CreatedBy = request.UserName
      };
      if (request.RoleName == GroupRole.User.ToString())
      {
        news.Approved = (int)ApproveProcess.OnHold;
        // need send notify to manager (send mail/tel/notify in web)
      }
      if (request.RoleName == GroupRole.Manager.ToString())
      {
        news.Approved = (int)ApproveProcess.Accept;
        // need send notify to everyone in group
      }
      _dbContext.Bulletins.Add(news);
      await _dbContext.SaveChangesAsync();
      return new UploadNewsToBulletinResponse()
      {
        Success = true,
        Message = request.RoleName == GroupRole.Manager.ToString() 
                  ? "Your news is posted" 
                  : "Your news will be posted when group manager accepts"
      };
    }
  }
}