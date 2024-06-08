using Konnect.API.Infrustructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Mapper;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses.DTOs;

namespace Konnect.API.Infrustructure.Repositories
{
	public interface IBulletinRepository
	{
		void DeletePost(string id);
		Bulletin GetPost(string id);
		void UpdatePost(string id, PostDTO model);
	}

	public class BulletinRepository : IMapperSupport<Bulletin, PostDTO>, IBulletinRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly EFContext _dbContext;

		public BulletinRepository(
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IConfiguration configuration,
			EFContext dbContext)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_dbContext = dbContext;
		}

		public IEnumerable<PostDTO> Mapper(IEnumerable<Bulletin> source)
		{
			return CustomMapper.Mapper.Map<IEnumerable<PostDTO>>(source);
		}

		public PostDTO Mapper(Bulletin source, PostDTO? destination)
		{
			if (destination == default)
			{
				return CustomMapper.Mapper.Map<PostDTO>(source);
			}
			return CustomMapper.Mapper.Map<Bulletin, PostDTO>(source, destination);
		}

		public Bulletin MapperReverse(PostDTO source, Bulletin? destination)
		{
			if (destination == default)
			{
				return CustomMapper.Mapper.Map<Bulletin>(source);
			}
			return CustomMapper.Mapper.Map<PostDTO, Bulletin>(source, destination);
		}

		public IEnumerable<Bulletin> MapperReverse(IEnumerable<PostDTO> source)
		{
			return CustomMapper.Mapper.Map<IEnumerable<Bulletin>>(source);
		}

		public Bulletin GetPost(string id)
		{
			return _dbContext.Bulletins.FirstOrDefault(b => b.Id == id);
		}

		public void UpdatePost(string id, PostDTO model)
		{
			var post = GetPost(id);
			model.Id = id;
			post = MapperReverse(model, post);
			_dbContext.SaveChanges();
		}

		public void DeletePost(string id)
		{
			var post = GetPost(id);
			_dbContext.Bulletins.Remove(post);
			_dbContext.SaveChanges();
		}
	}
}
