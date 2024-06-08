namespace Konnect.API.Utilities
{
	public class RandomUtilitiy<T>
	{
		public static T Random(IList<T>source)
		{
			Random r = new Random();
			int x = r.Next(source.Count());
			return source[x];
		}
	}
}
