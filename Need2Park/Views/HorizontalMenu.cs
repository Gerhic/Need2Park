using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Graphics;

namespace Need2Park
{
	class HorizontalMenu : UIView
	{
		public EventHandler OnRandomEvent;
		HorizontalMenuLabel mapLabel;
		HorizontalMenuLabel myListLabel;
		UIView hilightBar;

		const int labelsCount = 2;

		HorizontalMenuLabel activeLabel;
		HorizontalMenuLabel ActiveLabel { 
			get {
				if (activeLabel == null) {
					ActiveLabel = mapLabel;
				}
				return activeLabel;
			} 
			set {
				if (activeLabel != null) {
					activeLabel.SetBold (false);
				}
				activeLabel = value;
				activeLabel.SetBold (true);

			}
		}

		public HorizontalMenu (Activity activity) : base (activity)
		{
			BackgroundColor = Color.LimeGreen;
			mapLabel = new HorizontalMenuLabel (activity);
			mapLabel.Text = "Map";
			mapLabel.Click += HandleLabelClick;

			myListLabel = new HorizontalMenuLabel (activity);
			myListLabel.Text = "Bookmarks";
			myListLabel.Click += HandleLabelClick;

			hilightBar = new UIView (activity);
			hilightBar.BackgroundColor = Color.White;

			AddViews (
				mapLabel,
				myListLabel,
				hilightBar
			);
		}

		public override void LayoutSubviews ()
		{
			int x = 0;
			int labelWidth = Frame.W / labelsCount;
			mapLabel.Frame = new Frame (
				x,
				0,
				labelWidth,
				Frame.H
			);
			x += labelWidth;

			myListLabel.Frame = new Frame (
				x,
				0,
				Frame.W / labelsCount,
				Frame.H
			);

			hilightBar.Frame = new Frame (
				ActiveLabel.Frame.X,
				ActiveLabel.Frame.Bottom - Sizes.HilightBarHeight,
				ActiveLabel.Frame.W,
				Sizes.HilightBarHeight
			);
		}

		void HandleLabelClick (object sender, EventArgs e)
		{
			if (sender == activeLabel) {
				return;
			}

			HorizontalMenuLabel label = sender as HorizontalMenuLabel;
			if (label != null) {
				hilightBar.Animate ().TranslationX (label.Frame.X);
				ActiveLabel = label;
			}

			if (OnRandomEvent != null) {
				OnRandomEvent (new object (), null);
			}
		}
	}


}
