using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;
using Android.Graphics;

namespace Need2Park
{
	[Activity (Theme = "@style/Theme.MainTheme", Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		const int menuitemId = 0;

		MenuPopupView menuPopup;
		MainView contentView;

		UIView actionBarContainer;
		UIImageView logoImage;
		UIView menuImageContainer;
		UIImageView menuImage;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			logoImage = new UIImageView (this);
			logoImage.ImageResource = Resource.Drawable.esilehe_logo;
			logoImage.SetScaleType (ImageView.ScaleType.CenterInside);
			logoImage.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			menuImageContainer = new UIView (this);
			menuImageContainer.Frame = new Frame (
				DeviceInfo.ScreenWidth - DeviceInfo.NavigationBarHeight - Sizes.MenuButtonSize,
				0,
				DeviceInfo.NavigationBarHeight + Sizes.MenuButtonSize,
				DeviceInfo.NavigationBarHeight
			);



			menuImage = new UIImageView (this);
			menuImage.ImageResource = Resource.Drawable.burger;
			menuImage.SetScaleType (ImageView.ScaleType.CenterInside);
			menuImage.Frame = new Frame (
				Sizes.MenuButtonSize,
				(menuImageContainer.Frame.H - Sizes.MenuButtonSize) / 2,
				Sizes.MenuButtonSize,
				Sizes.MenuButtonSize
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
			if (menuPopup != null) {
				menuPopup.UpdateView ();
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