using System.Text.Json;
using System.Text.RegularExpressions;

namespace Konnect.API.Utilities
{
  public static class StringHelper
  {
    public static string ToCamelCase(string input)
    {
      return JsonNamingPolicy.CamelCase.ConvertName(input);
    }
  }
}
