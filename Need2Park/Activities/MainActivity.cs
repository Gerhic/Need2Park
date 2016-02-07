using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;
using Android.Graphics;
using Android.Locations;
using System.Linq;
using System;

namespace Need2Park
{
	[Activity (Theme = "@style/Theme.MainTheme", Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity, ILocationListener
	{
		const int menuitemId = 0;

		MenuPopupView menuPopup;
		MainView contentView;

		UIView actionBarContainer;
		UIImageView logoImage;
		UIView menuImageContainer;
		UIImageView menuImage;


		public Location CurrentLocation { get; set; }

		LocationManager locMgr;


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			logoImage = new UIImageView (this);
			logoImage.ImageResource = Resource.Drawable.esilehe_logo;
			logoImage.SetScaleType (ImageView.ScaleType.CenterInside);
			logoImage.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			menuImageContainer = new UIView (this);
			menuImageContainer.Frame = new Frame (
				0,
				0,
				DeviceInfo.NavigationBarHeight + Sizes.ActionBarButtonSize,
				DeviceInfo.NavigationBarHeight
			);

			menuImage = new UIImageView (this);
			menuImage.ImageResource = Resource.Drawable.burger;
			menuImage.SetScaleType (ImageView.ScaleType.CenterInside);
			menuImage.Frame = new Frame (
				0,
				(menuImageContainer.Frame.H - Sizes.ActionBarButtonSize) / 2,
				Sizes.ActionBarButtonSize,
				Sizes.ActionBarButtonSize
			);

			menuImageContainer.AddView (menuImage);

			actionBarContainer = new UIView (this);
			actionBarContainer.Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.NavigationBarHeight);
			actionBarContainer.AddViews (
				logoImage,
				menuImageContainer
			);

			ActionBar.SetDisplayShowCustomEnabled (true);

			this.ActionBar.SetCustomView (
				actionBarContainer, 
				new ActionBar.LayoutParams (DeviceInfo.ScreenWidth, DeviceInfo.NavigationBarHeight)
			);




			locMgr = GetSystemService (Context.LocationService) as LocationManager;




			contentView = new MainView (this);
			SetContentView (contentView);

			menuPopup = new MenuPopupView (this, WindowManager);
			menuPopup.UpdateView ();

			menuImageContainer.Click += (object sender, System.EventArgs e) => ShowMenu ();
		}

		void ShowMenu ()
		{
			menuPopup.ShowAnimated ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			string Provider = LocationManager.GpsProvider;

			if(locMgr.IsProviderEnabled(Provider))
			{
				Criteria locationCriteria = new Criteria();

				locationCriteria.Accuracy = Accuracy.Coarse;
				locationCriteria.PowerRequirement = Power.Medium;

				var locationProvider = locMgr.GetBestProvider(locationCriteria, true);

				if (locationProvider != null)
				{
					locMgr.RequestLocationUpdates (locationProvider, 2000, 1, this);
				}
				else
				{
					Console.WriteLine ("No location providers available");
				}
			}
			else
			{
				Console.WriteLine (Provider + " is not available. Does the device have location services enabled?");
			}


			if (menuPopup != null) {
				menuPopup.UpdateView ();
			}
			if (contentView != null) {
				contentView.UpdateViews ();
			}

			if (contentView != null && contentView.mapView != null) {
				contentView.mapView.AddMarkers ();
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
			if (contentView != null && contentView.mapView != null) {
				contentView.mapView.AddMarkers ();
			}
		}

		public override void OnBackPressed ()
		{
			if (menuPopup.isOpen) {
				menuPopup.HideMenu ();
			} else {
				base.OnBackPressed ();
			}
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			locMgr.RemoveUpdates (this);
		}

		public void OnLocationChanged (Location location)
		{
			CurrentLocation = location;
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
		}
	}
}