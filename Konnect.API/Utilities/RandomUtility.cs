namespace UTCClassSupport.API.Utilities
{
  public class RandomUtility
  {
    public const string LowerCaseAlphabet = "abcdefghijklmnopqrstuvwyxz";
    public const string UpperCaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string FullCaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static Random rng = new Random();
    public static string GenerateString(int size = 8)
    {
      return GenerateString(size, FullCaseAlphabet); 
    }
    public static string GenerateUpperCaseString(int size)
    {
      return GenerateString(size, UpperCaseAlphabet);
    }

    public static string GenerateLowerCaseString(int size)
    {
      return GenerateString(size, LowerCaseAlphabet);
    }

    public static string GenerateString(int size, string alphabet)
    {
      char[] chars = new char[size];
      for (int i = 0; i < size; i++)
      {
        chars[i] = alphabet[rng.Next(alphabet.Length)];
      }
      return new string(chars);
    }
  }
}
