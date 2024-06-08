using UTCClassSupport.API.Requests.Pagination;

namespace Konnect.API.Infrustructure.Interfaces
{
	public interface IMapperSupport<TSource, TDestination>
	{
		TDestination Mapper(TSource source);
		IEnumerable<TDestination> Mapper (IEnumerable<TSource> source);
	}

	public interface IPaginationSupport<T>
	{
		IEnumerable<T> Pagination(PaginationData paginationData);
	}
}
