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

		bool isRequestInProgress;

		readonly Activity activity;

		public ParkingLotView (Activity activity, ParkingLotInfo info) : base (activity)
		{
			this.activity = activity;
			parkingLotInfo = info;

			BackgroundColor = Color.LightCyan;

			nameLabel = new ParkingLotCellLabel (activity);
			nameLabel.Text = parkingLotInfo.Name;

			locationLabel = new ParkingLotCellLabel (activity);
			locationLabel.Text = parkingLotInfo.Location;

			descriptionLabel = new ParkingLotCellLabel (activity);
			descriptionLabel.Text = parkingLotInfo.Provider;

			startParkingButton = new ParkingLotCellLabel (activity);

			if (LoginState.ActiveUser != null && LoginState.ActiveUser.ParkingLotInUse == info) {
				startParkingButton.Text = "End parking session";
			} else {
				if (LoginState.ActiveUser.ParkingLotInUse == null) {
					startParkingButton.Text = "Start a parking session";
				} else {
					startParkingButton.Text = "Allready parking at: " + LoginState.ActiveUser.ParkingLotInUse.Name;
				}
			}

			startParkingButton.BackgroundColor = Color.LightGreen;

			startParkingButton.Gravity = GravityFlags.Center;
			startParkingButton.Click += HandleStartParkingClick;

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

		void HandleStartParkingClick (object sender, System.EventArgs e)
		{
			if (!isRequestInProgress) {
				if (LoginState.ActiveUser == null) {
					Intent loginIntent = new Intent (activity, typeof(LoginActivity));
					loginIntent.AddFlags (ActivityFlags.ClearTop);
					activity.StartActivity (loginIntent);
				} else {
					if (LoginState.ActiveUser.ParkingLotInUse == null) {
						isRequestInProgress = true;
						// TODO check if succeeded
						Networking.SendStartParkingRequest (parkingLotInfo.PublicId);
						LoginState.ActiveUser.ParkingLotInUse = parkingLotInfo;
						startParkingButton.Text = "End parking session";
					} else {
						if (LoginState.ActiveUser.ParkingLotInUse == parkingLotInfo) {
							isRequestInProgress = true;
							Networking.SendStopParkingRequest (parkingLotInfo.PublicId);
							LoginState.ActiveUser.ParkingLotInUse = null;
							startParkingButton.Text = "Start a parking session";
						}
					}
					isRequestInProgress = false;
				}
			}
		}
	}
}