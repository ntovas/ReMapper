using ReMap.Classes;

namespace ReMap.Extensions
{
	public static class ReMapperExtensions
	{
		public static MapBuilder<TSource, TResult> AddBuilder<TSource, TResult>(
			this ReMapper reMapper)
		{
			return new MapBuilder<TSource, TResult>(reMapper);
		}
	}
}
