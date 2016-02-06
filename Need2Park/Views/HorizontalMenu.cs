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
		public EventHandler OnLabelClick;
		HorizontalMenuLabel mapLabel;
		HorizontalMenuLabel myListLabel;

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
					activeLabel.BackgroundColor = Color.Transparent;
				}
				activeLabel = value;
				activeLabel.BackgroundColor = CustomColors.LightColor;
			}
		}

		public HorizontalMenu (Activity activity) : base (activity)
		{
			BackgroundColor = CustomColors.LightColorDim;
			mapLabel = new HorizontalMenuLabel (activity);
			mapLabel.Text = "Map";
			mapLabel.Click += HandleLabelClick;

			myListLabel = new HorizontalMenuLabel (activity);
			myListLabel.Text = "My places";
			myListLabel.Click += HandleLabelClick;

			AddViews (
				mapLabel,
				myListLabel
			);
			ActiveLabel = mapLabel;
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
		}

		void HandleLabelClick (object sender, EventArgs e)
		{
			if (sender == ActiveLabel) {
				return;
			}

			HorizontalMenuLabel label = sender as HorizontalMenuLabel;
			if (label != null) {
				ActiveLabel = label;
			}

			if (OnLabelClick != null) {
				OnLabelClick (new object (), null);
			}
		}
	}


}
