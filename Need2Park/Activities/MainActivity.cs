using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace Need2Park
{
	[Activity (Label = "Need2Park", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		IMenuItem menuitem;
		const int menuitemId = 0;

		MenuPopupView menuPopup;
		MainView contentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
//			Xamarin.Insights.Initialize (XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);

			DeviceInfo.Measure(this);

			contentView = new MainView (this);
			SetContentView (contentView);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			menuitem = menu.Add (Menu.None, menuitemId, Menu.None, "Open menu");
//			menuitem.SetIcon (Resource.Drawable.icon_normal_filters);
			menuitem.SetShowAsAction (ShowAsAction.Always);

			if (menuPopup == null) {
				InitMenuPopup();
			}
			return true;
		}

		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			if (item.ItemId == menuitemId) {
				ShowMenu ();
				return true;
			}
			return base.OnMenuItemSelected (featureId, item);
		}

		void ShowMenu ()
		{
			menuPopup.ShowAnimated ();
		}

		void InitMenuPopup ()
		{
			menuPopup = new MenuPopupView (this, WindowManager);
		}
	}
}
