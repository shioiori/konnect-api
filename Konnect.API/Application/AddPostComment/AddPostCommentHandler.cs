using Konnect.API.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.AddPostComment
{
	public class AddPostCommentHandler : IRequestHandler<AddPostCommentCommand, AddPostCommentResponse>
	{
		private readonly EFContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly NotificationManager _notificationManager;
		private readonly IUserRepository _userRepository;
		public AddPostCommentHandler(EFContext dbContext,
		  UserManager<User> userManager,
			RoleManager<Role> roleManager,
			NotificationManager notificationManager,
			IUserRepository userRepository)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_notificationManager = notificationManager;
			_userRepository = userRepository;
		}

		public async Task<AddPostCommentResponse> Handle(AddPostCommentCommand request, CancellationToken cancellationToken)
		{
			var userNames = StringHelper.ExtractStringsFollowingAtSymbol(request.Content);
			Dictionary<string, UserDTO> userDic = new Dictionary<string, UserDTO>();
			if (userNames.Count > 0)
			{
				foreach (var userName in userNames)
				{
					var user = await _userRepository.GetUserAsync(userName);
					if (user == null) continue;
					request.Content = request.Content.Replace("@" + userName + " ", "<a class='cmt-mention'>@" + user.DisplayName + "</a> ");
					userDic[userName] = user;
				}

			}
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
			cmt.User = CustomMapper.Mapper.Map<UserDTO>(await _userManager.FindByNameAsync(comment.CreatedBy));

			//notification
			var post = _dbContext.Bulletins.Find(request.PostId);
			_notificationManager.NotifyNewCommentAsync(post, request);
			if (userDic.Count > 0)
			{
				foreach (var userName in userNames)
				{
					_notificationManager.NotifyMention(comment, userDic[userName]);
				}
			}
			return new AddPostCommentResponse()
			{
				Success = true,
				Message = "Add user success",
				Comment = cmt
			};
		}
	}
}
