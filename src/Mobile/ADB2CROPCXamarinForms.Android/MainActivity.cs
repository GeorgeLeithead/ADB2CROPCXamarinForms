namespace ADB2CROPCXamarinForms.Droid
{
	using Android.App;
	using Android.Content.PM;
	using Android.Runtime;
	using Android.OS;
	using Plugin.CurrentActivity;
	using Android.Content;
	using Microsoft.Identity.Client;
	using Xamarin.Forms;
	using ADB2CROPCXamarinForms.Interfaces;

	[Activity(Label = "ADB2CROPCXamarinForms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
			DependencyService.Register<IParentWindowLocatorService, AndroidParentWindowLocatorService>();

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			Forms.Init(this, savedInstanceState);
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
		}
	}
}