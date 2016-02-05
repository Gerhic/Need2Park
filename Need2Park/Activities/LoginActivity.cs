using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Preferences;

namespace Need2Park
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	class LoginActivity : Activity
	{
		LoginView contentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			ActionBar.Hide ();

			contentView = new LoginView (this);
			SetContentView (contentView);
		}


		public void CacheUser (UserInfo user)
		{
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this); 
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString (UserInfo.NAME, user.Name);
			editor.PutString (UserInfo.SESSIONID, user.SessionId);
			// TODO add the rest
			editor.Apply();
		}
	}
}

