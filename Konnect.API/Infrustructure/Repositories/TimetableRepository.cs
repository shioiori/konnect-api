using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses.DTOs;

namespace Konnect.API.Infrustructure.Repositories
{
	public interface ITimetableRepository
	{
		bool DeleteEvent(int id);
		bool UpdateEvent(int id, UpdateEventRequest dto);
	}

	public class TimetableRepository : ITimetableRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly EFContext _dbContext;

		public TimetableRepository(EFContext dbContext,
		UserManager<User> userManager,
		RoleManager<Role> roleManager)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		private Event GetEvent(int id)
		{
			return _dbContext.Events.FirstOrDefault(x => x.Id == id);
		}

		public bool UpdateEvent(int id, UpdateEventRequest dto)
		{
			var ev = GetEvent(id);
			if (ev == default)
			{
				return false;
			}
			CustomMapper.Mapper.Map<UpdateEventRequest, Event>(dto, ev);
			_dbContext.SaveChanges();
			return true;
		}

		public bool DeleteEvent(int id)
		{
			var ev = GetEvent(id);
			if (ev == default) return false;
			_dbContext.Events.Remove(ev);
			_dbContext.SaveChanges();
			return true;

		}
	}
}
