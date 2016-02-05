using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;

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
			base.OnCreate (savedInstanceState);

			DeviceInfo.Measure(this);

			GetActiveUserFromSharedPreferences ();

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

		protected override void OnResume ()
		{
			base.OnResume ();
			if (menuPopup != null) {
				menuPopup.UpdateView ();
			}
		}

		public async void GetActiveUserFromSharedPreferences ()
		{
			if (LoginState.ActiveUser == null) {
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this); 
				var name = prefs.GetString (UserInfo.NAME, string.Empty);
				var sessionId = prefs.GetString (UserInfo.SESSIONID, string.Empty);

				if (sessionId != string.Empty) {
					LoginState.ActiveUser = new UserInfo {
						Name = name,
						SessionId = sessionId
					};
				}

				List<ParkingLotInfo> parkingLotInfoList = await Networking.SendParkingLotsRequest ();
				if (parkingLotInfoList != null) {
					LoginState.ActiveUser.AccessInfo.AddRange (parkingLotInfoList);
					contentView.UpdateViews ();
				} else {
					// TODO handle failing in life
				}
			}
		}

		public void ClearUserCache ()
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this); 
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString (UserInfo.NAME, null);
			editor.PutString (UserInfo.SESSIONID, null);
			// TODO add the rest
			editor.Apply();
		}

		public void HandeLogout ()
		{
			contentView.UpdateViews ();
		}
	}
}