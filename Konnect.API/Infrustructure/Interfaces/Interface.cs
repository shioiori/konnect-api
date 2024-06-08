using UTCClassSupport.API.Requests.Pagination;

namespace Konnect.API.Infrustructure.Interfaces
{
	public interface IMapperSupport<TSource, TDestination>
	{
		TDestination Mapper(TSource source, TDestination? destination);
		IEnumerable<TDestination> Mapper (IEnumerable<TSource> source);
		TSource MapperReverse(TDestination source, TSource? destination);
		IEnumerable<TSource> MapperReverse(IEnumerable<TDestination> source);

	}

	public interface IPaginationSupport<T>
	{
		IEnumerable<T> Pagination(PaginationData paginationData);
	}
}
