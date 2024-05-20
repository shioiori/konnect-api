using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Konnect.ChatHub.Helper
{
  public static class JsonHelper
  {
    public static string SerializeObjectCamelCase(object obj)
    {
      return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      });
    }
  }
}
