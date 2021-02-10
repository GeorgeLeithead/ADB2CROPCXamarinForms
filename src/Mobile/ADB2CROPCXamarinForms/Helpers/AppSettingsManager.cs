namespace ADB2CROPCXamarinForms.Helpers
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using Newtonsoft.Json.Linq;
	using Xamarin.Essentials;

	public class AppSettingsManager
	{
#if DEBUG
		private static readonly string DefaultChatIP = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost"; // NOTE: 10.0.2.2 is the Android equivalent of localhost
#else
		private static readonly string DefaultChatIP = AppSettingsManager.Settings["ApiEndpoint"];
#endif
		
		private const string FileName = "AppSettings.json";

		// constants needed to locate and access the App Settings file
		private const string Namespace = "ADB2CROPCXamarinForms";

		// stores the instance of the singleton
		private static AppSettingsManager _instance;

		// variable to store your appsettings in memory for quick and easy access
		private readonly JObject secrets;

		// Creates the instance of the singleton
		private AppSettingsManager()
		{
			try
			{
				Assembly assembly = typeof(AppSettingsManager).GetTypeInfo().Assembly;
				Stream stream = assembly.GetManifestResourceStream($"{Namespace}.{FileName}");
				using (StreamReader reader = new StreamReader(stream))
				{
					string json = reader.ReadToEnd();
					secrets = JObject.Parse(json);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Unable to load secrets file: {ex.Message}");
			}
		}

		/// <summary>Gets or sets the hello service address.</summary>
		public static string HelloServiceIP
		{
			get => Preferences.Get(nameof(HelloServiceIP), DefaultChatIP);
			set => Preferences.Set(nameof(HelloServiceIP), value);
		}

		public static AppSettingsManager Settings
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AppSettingsManager();
				}

				return _instance;
			}
		}

		/// <summary>Gets a value indicating whether to use HTTPS.</summary>
		public static bool UseHttps => DefaultChatIP != "localhost" && DefaultChatIP != "10.0.2.2";

		// Used to retrieved setting values
		public string this[string name]
		{
			get
			{
				try
				{
					string[] path = name.Split(':');

					JToken node = secrets[path[0]];
					for (int index = 1; index < path.Length; index++)
					{
						node = node[path[index]];
					}

					return node.ToString();
				}
				catch (Exception)
				{
					Debug.WriteLine($"Unable to retrieve secret '{name}'");
					return string.Empty;
				}
			}
		}
	}
}