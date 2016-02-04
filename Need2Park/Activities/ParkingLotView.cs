using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;

namespace Need2Park
{
	class ParkingLotView : UIView
	{
		ParkingLotInfo parkingLotInfo;

		ParkingLotCellLabel nameLabel;
		ParkingLotCellLabel locationLabel;
		ParkingLotCellLabel descriptionLabel;

		ParkingLotCellLabel startParkingButton;

		public ParkingLotView (Activity activity, ParkingLotInfo info) : base (activity)
		{
			parkingLotInfo = info;

			BackgroundColor = Color.LightCyan;

			nameLabel = new ParkingLotCellLabel (activity);
			nameLabel.Text = parkingLotInfo.Name;

			locationLabel = new ParkingLotCellLabel (activity);
			locationLabel.Text = parkingLotInfo.Location;

			descriptionLabel = new ParkingLotCellLabel (activity);
			descriptionLabel.Text = parkingLotInfo.Description;

			startParkingButton = new ParkingLotCellLabel (activity);
			startParkingButton.Text = "Start parking!";
			startParkingButton.BackgroundColor = Color.SeaGreen;

			startParkingButton.Gravity = GravityFlags.Center;

			AddViews (
				nameLabel,
				locationLabel,
				descriptionLabel,
				startParkingButton
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.TrueScreenHeight);
		}

		public override void LayoutSubviews ()
		{
			int y = Sizes.ParkingViewVerticalPadding;
			nameLabel.Frame = new Frame (
				Sizes.ParkingViewHorizontalPadding,
				y,
				Frame.W - 2 * Sizes.ParkingViewHorizontalPadding,
				Sizes.ParkingViewLabelHeight
			);

			y += nameLabel.Frame.H + Sizes.ParkingViewVerticalPadding;

			locationLabel.Frame = new Frame (
				Sizes.ParkingViewHorizontalPadding,
				y,
				Frame.W - 2 * Sizes.ParkingViewHorizontalPadding,
				Sizes.ParkingViewLabelHeight
			);

			y += locationLabel.Frame.H + Sizes.ParkingViewVerticalPadding;

			descriptionLabel.Frame = new Frame (
				Sizes.ParkingViewHorizontalPadding,
				y,
				Frame.W - 2 * Sizes.ParkingViewHorizontalPadding,
				Sizes.ParkingViewLabelHeight
			);

			startParkingButton.Frame = new Frame (
				Sizes.ParkingViewHorizontalPadding,
				Frame.Bottom - Sizes.ParkingViewLabelHeight - Sizes.ParkingViewVerticalPadding,
				Frame.W - 2 * Sizes.ParkingViewHorizontalPadding,
				Sizes.ParkingViewLabelHeight
			);
		}

	}
}