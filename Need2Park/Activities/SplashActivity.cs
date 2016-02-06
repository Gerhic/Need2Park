using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Need2Park
{
	[Activity (Theme = "@style/Theme.Splash", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true)]
	public class SplashActivity : Activity
	{
		SplashView contentView;

		protected override async void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			DeviceInfo.Measure(this);

			contentView = new SplashView (this);
			SetContentView (contentView);

			await GetActiveUserFromSharedPreferences ();

			StartActivity (new Intent (this, typeof(MainActivity)));

			Finish ();
		}

		public async Task GetActiveUserFromSharedPreferences ()
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
				} else {
					// TODO handle failing in life
				}
			}
		}
	}
}

