using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;
using Android.Net;

namespace Need2Park
{
	class ParkingLotView : UIView
	{
		ParkingLotInfo parkingLotInfo;

		ParkingLotLabel nameLabel;
//		ParkingLotLabel locationLabel;
		ParkingLotLabel freeSpotsLabel;

		ParkingLotLabel startParkingButton;

		bool isRequestInProgress;

		readonly Activity activity;

		UIImageView backgroundImage;

		UIView infoContainer;
		UIView separator;

		UIView navigateButtonContainer;
		ParkingLotLabel navigateButton;

		public ParkingLotView (Activity activity, ParkingLotInfo info) : base (activity)
		{
			this.activity = activity;
			parkingLotInfo = info;

			backgroundImage = new UIImageView (activity);
			backgroundImage.ImageResource = Resource.Drawable.app_parking_background;
			backgroundImage.SetScaleType (Android.Widget.ImageView.ScaleType.CenterCrop);
			backgroundImage.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			infoContainer = new UIView (activity);
			infoContainer.SetRoundBordersWithColor (
				CustomColors.LightColor, 
				Sizes.LoginInputHeight / 3,
				Sizes.LoginSeparatorSize
			);

			nameLabel = new ParkingLotLabel (activity);
			nameLabel.Text = parkingLotInfo.Name;
			nameLabel.TextSize = Sizes.GetRealSize (10);

//			locationLabel = new ParkingLotLabel (activity);
//			locationLabel.Text = parkingLotInfo.Location;
//			locationLabel.TextSize = Sizes.GetRealSize (8);

			freeSpotsLabel = new ParkingLotLabel (activity);
			freeSpotsLabel.TextSize = Sizes.GetRealSize (8);
			UpdateFreeSpots ();

			startParkingButton = new ParkingLotLabel (activity);
			startParkingButton.TextSize = Sizes.GetRealSize (9);
			startParkingButton.TextColor = CustomColors.DarkColor;
			if (LoginState.ActiveUser != null && LoginState.ActiveUser.ParkingLotInUse == info) {
				startParkingButton.Text = Strings.EndParking;
			} else {
				if (LoginState.ActiveUser.ParkingLotInUse == null) {
					startParkingButton.Text = Strings.StartParking;
				} else {
					startParkingButton.Text = "Parking at: " + LoginState.ActiveUser.ParkingLotInUse.Name;
				}
			}

			int radius = (int)(Sizes.ParkingViewLabelHeight * 0.6f);
			startParkingButton.SetCornerRadiusWithColor (CustomColors.LightColor, 
				new float[] {
					0, 0,
					0, 0, 
					radius, radius, 
					radius, radius
				}
			);

			startParkingButton.Click += HandleStartParkingClick;

			separator = new UIView (activity);
			separator.BackgroundColor = CustomColors.LightColor;

			infoContainer.AddViews (
				nameLabel,
				separator,
//				locationLabel,
				freeSpotsLabel
			);


			navigateButton = new ParkingLotLabel (activity);
			navigateButton.Text = Strings.Navigate;
			navigateButton.TextSize = Sizes.GetRealSize (10);
			navigateButton.Click += HandleNavigateClick;

			navigateButtonContainer = new UIView (activity);
			
			navigateButtonContainer.SetRoundBordersWithColor (
				CustomColors.LightColor, 
				Sizes.LoginInputHeight / 3,
				Sizes.LoginSeparatorSize
			);
			navigateButtonContainer.AddView (navigateButton);

			AddViews (
				backgroundImage,
				infoContainer,
				startParkingButton,
				navigateButtonContainer
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.TrueScreenHeight);
		}

		public override void LayoutSubviews ()
		{
			int containerHeight = Sizes.ParkingViewTitleHeight + Sizes.LoginSeparatorSize + 2 * Sizes.ParkingViewLabelHeight;

			navigateButtonContainer.Frame = new Frame (
				Sizes.ParkingViewHorizontalPadding + Sizes.ParkingViewButtonPadding,
				Frame.H - Sizes.ParkingViewVerticalPadding,
				Frame.W - 2 * Sizes.ParkingViewHorizontalPadding - 2 * Sizes.ParkingViewButtonPadding,
				Sizes.ParkingViewButtonHeight
			);
			navigateButton.Frame = new Frame (
				Sizes.ParkingViewButtonPadding,
				0,
				navigateButtonContainer.Frame.W - 2 * Sizes.ParkingViewButtonPadding,
				navigateButtonContainer.Frame.H
			);

			infoContainer.Frame = new Frame (
				Sizes.LoginHorizontalPadding,
				(Frame.H - containerHeight - Sizes.ParkingViewButtonHeight) / 3,
				Frame.W - 2 * Sizes.LoginHorizontalPadding,
				containerHeight
			);

			nameLabel.Frame = new Frame (
				Sizes.ParkingViewButtonPadding,
				0,
				infoContainer.Frame.W - 2 * Sizes.ParkingViewButtonPadding,
				Sizes.ParkingViewTitleHeight
			);

			separator.Frame = new Frame (
				Sizes.LoginInputPadding,
				nameLabel.Frame.Bottom,
				infoContainer.Frame.W - 2 * Sizes.LoginInputPadding,
				Sizes.LoginSeparatorSize
			);

//			locationLabel.Frame = new Frame (
//				Sizes.ParkingViewHorizontalPadding,
//				separator.Frame.Bottom,
//				infoContainer.Frame.W - 2 * Sizes.ParkingViewHorizontalPadding,
//				Sizes.ParkingViewLabelHeight
//			);

			freeSpotsLabel.Frame = new Frame (
				Sizes.ParkingViewButtonPadding,
				separator.Frame.Bottom,
				infoContainer.Frame.W - 2 * Sizes.ParkingViewButtonPadding,
				2 * Sizes.ParkingViewLabelHeight
			);

			startParkingButton.Frame = new Frame (
				infoContainer.Frame.X + Sizes.ParkingViewButtonPadding,
				infoContainer.Frame.Bottom,
				infoContainer.Frame.W - 2 * Sizes.ParkingViewButtonPadding,
				Sizes.ParkingViewButtonHeight
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
						startParkingButton.Text = Strings.EndParking;
						if (parkingLotInfo.Spots > 0) {
							parkingLotInfo.Spots -= 1;
						}
						UpdateFreeSpots ();

						// dialog
//						DialogUtils.CreateDialog (activity, "Success!", null);

					} else {
						if (LoginState.ActiveUser.ParkingLotInUse == parkingLotInfo) {
							isRequestInProgress = true;
							Networking.SendStopParkingRequest (parkingLotInfo.PublicId);
							LoginState.ActiveUser.ParkingLotInUse = null;
							startParkingButton.Text = Strings.StartParking;
							parkingLotInfo.Spots += 1;
							UpdateFreeSpots ();
						}
					}
					isRequestInProgress = false;
				}
			}
		}

		void UpdateFreeSpots ()
		{
			freeSpotsLabel.Text = Strings.FreeSlots + parkingLotInfo.Spots;
		}

		void HandleNavigateClick (object sender, System.EventArgs e)
		{
			if (parkingLotInfo != null && !string.IsNullOrWhiteSpace (parkingLotInfo.Location)) {
				var tokens = parkingLotInfo.Location.Split (new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);

				if (tokens.Length >= 2) {
					string lat = tokens [0];
					string lon = tokens [1];

					OpenMapWithLatAndLon(lat, lon);
				}
			}
		}

		void OpenMapWithLatAndLon (string lat, string lon)
		{
			string uri = string.Format ("http://maps.google.com/maps?daddr={0},{1}", lat, lon);
			Intent intent = new Intent(Intent.ActionView, Uri.Parse (uri));
			activity.StartActivity (intent);

		}
	}
}