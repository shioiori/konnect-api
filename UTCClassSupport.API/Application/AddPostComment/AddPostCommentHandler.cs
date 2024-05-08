﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Application.AddPostComment
{
  public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, AddPostCommentResponse>
  {
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public AddPostCommentHandler(EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public Task<AddPostCommentResponse> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
    {
      var comment = new Comment()
      {
        PostId = request.PostId,
        Content = request.Content,
        CreatedDate = DateTime.UtcNow,
        CreatedBy = request.UserName,
      };
      _dbContext.Comments.Add(comment);
      _dbContext.SaveChanges();
      var cmt = CustomMapper.Mapper.Map<CommentDTO>(comment);
      cmt.DisplayName = request.DisplayName;
      return Task.FromResult(new AddPostCommentResponse()
      {
        Success = true,
        Message = "Add user success",
        Comment = cmt
      });
    }
  }
}
