using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Maps;
using Android.Widget;
using Android.Views;
using Android.Runtime;

namespace Need2Park
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	class InfoActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			ActionBar.Hide ();

			SetContentView (new TermsView (this));
		}
	}

	class TermsView : UIView
	{
		UILabel infoLabel;
		UILabel aboutUsLabel;

		public TermsView (InfoActivity termsActivity) : base (termsActivity)
		{
			BackgroundColor = CustomColors.LightBackground;

			infoLabel = new UILabel (termsActivity);
			infoLabel.Text = Strings.AboutUsText;
			infoLabel.TextColor = CustomColors.DarkColor;
			infoLabel.TextSize = Sizes.GetRealSize (8);

			aboutUsLabel = new UILabel (termsActivity);
			aboutUsLabel.Text = "About Us";
			aboutUsLabel.TextColor = CustomColors.DarkColor;
			aboutUsLabel.TextSize = Sizes.GetRealSize (12);
			aboutUsLabel.Measure (0, 0);
			aboutUsLabel.Gravity = GravityFlags.CenterVertical;
			AddViews (
				infoLabel,
				aboutUsLabel
			);

			Frame = new Frame (
				DeviceInfo.ScreenWidth,
				DeviceInfo.ScreenHeight - DeviceInfo.StatusBarHeight
			);
		}

		public override void LayoutSubviews ()
		{
			infoLabel.Frame = new Frame (
				Sizes.LoginHorizontalPadding,
				Sizes.GetRealSize (100),
				Frame.W - 2 * Sizes.LoginHorizontalPadding,
				Frame.H - Sizes.GetRealSize (100) - Sizes.ActionBarPadding
			);

			aboutUsLabel.Frame = new Frame (
				Sizes.LoginHorizontalPadding,
				infoLabel.Frame.Y - Sizes.GetRealSize (10) - aboutUsLabel.MeasuredHeight, 
				Frame.W - 2 * Sizes.LoginHorizontalPadding,
				aboutUsLabel.MeasuredHeight
			);
		}
	}
}

