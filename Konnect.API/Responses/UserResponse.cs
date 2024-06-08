using UTCClassSupport.API.Requests.Pagination;
using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
	public class GetUserResponse : Response
	{
		public UserDTO User { get; set; }
	}

	public class GetUsersResponse : Response
	{
		public int Total { get; set; }
		public PaginationData Pagination { get; set; }
		public List<UserDTO> Users { get; set; }
	}

	public class AddUserResponse : Response
	{
		public UserDTO User { get; set; }
	}

	public class UpdateUserResponse : Response { public UserDTO User { get; set; } }
	public class DeleteUserResponse : Response
	{
	}
	public class ChangeRoleResponse : Response { }

	public class ChangePasswordResponse : Response { }
}
