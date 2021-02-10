namespace ADB2CROPCXamarinForms.Droid
{
	using ADB2CROPCXamarinForms.Interfaces;
	using Plugin.CurrentActivity;

	internal class AndroidParentWindowLocatorService : IParentWindowLocatorService
	{
		public object GetCurrentParentWindow()
		{
			return CrossCurrentActivity.Current.Activity;
		}
	}
}