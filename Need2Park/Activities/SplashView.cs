using System;
using Android.App;
using Android.OS;

namespace Need2Park
{
	class SplashView : UIView
	{
//		UIImageView backgroundImage;
		UIImageView logo;

		public SplashView (SplashActivity splashActivity) : base (splashActivity)
		{
//			backgroundImage = new UIImageView (splashActivity);
//			backgroundImage.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();
//			backgroundImage.ImageResource = Resource.Drawable.landing_backg;
//			backgroundImage.SetScaleType (Android.Widget.ImageView.ScaleType.CenterCrop);

			logo = new UIImageView (splashActivity);
			logo.ImageResource = Resource.Drawable.esilehe_logo;
			logo.Frame = new Frame (
				Sizes.LoginHorizontalPadding,
				DeviceInfo.ScreenHeight / 5,
				DeviceInfo.ScreenWidth - 2 * Sizes.LoginHorizontalPadding,
				DeviceInfo.ScreenHeight / 3
			);
			logo.SetScaleType (Android.Widget.ImageView.ScaleType.CenterInside);

			AddViews (
//				backgroundImage,
				logo
			);
		}
	}

}

