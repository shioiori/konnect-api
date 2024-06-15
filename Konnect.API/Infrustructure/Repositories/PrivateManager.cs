using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System.Xml;
using UTCClassSupport.API.Infrustructure.Data;

namespace Konnect.API.Infrustructure.Repositories
{
	public static class PrivateManager
	{
		private static Dictionary<string, string> _keys;
		public static Dictionary<string, string> Keys
		{
			get
			{
				if (_keys == null)
				{

					_keys = new Dictionary<string, string>();
					ReadXml();
				}
				return _keys;
			}
		}
		private static void ReadXml()
		{
			var path = String.Format(AppDomain.CurrentDomain.BaseDirectory + "\\PrivateKey.xml");
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(path);

			XmlNodeList appSettings = xmlDoc.SelectNodes("/configuration/appSettings/add");

			foreach (XmlNode node in appSettings)
			{
				string key = node.Attributes["key"].Value;
				string value = node.Attributes["value"].Value;
				_keys.Add(JsonNamingPolicy.CamelCase.ConvertName(key), value);
			}
		}

		public class PrivateKey
		{
			public const string BaseApiUrl = "BaseApiUrl";
			public const string BaseChatApiUrl = "BaseChatApiUrl";
			public const string GoogleApiKey = "GoogleApiKey";
			public const string GoogleClientId = "GoogleClientId";
			public const string GoogleDiscoveryDoc = "GoogleDiscoveryDoc";
			public const string GoogleScopes = "GoogleScopes";
			public const string JWTTokenName = "JWTTokenName";
			public const string ImgurClientID = "ImgurClientID";
			public const string ImgurClientSecret = "ImgurClientSecret";
		}
	}
}
