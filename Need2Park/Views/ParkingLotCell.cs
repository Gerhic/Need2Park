using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;

namespace Need2Park
{
	public class ParkingLotCell : UITableCell
	{
		ParkingLotCellLabel nameLabel;
		ParkingLotCellLabel freeSlotsLabel;
		UIImageView arrowImage;

		public ParkingLotInfo info {
			get;
			set;
		}

		public ParkingLotCell (Activity activity) : base (activity)
		{
			nameLabel = new ParkingLotCellLabel (activity);
			nameLabel.TextSize = Sizes.GetRealSize (9);
			nameLabel.Gravity = GravityFlags.Bottom;

			freeSlotsLabel = new ParkingLotCellLabel (activity);
			freeSlotsLabel.TextSize = Sizes.GetRealSize (7);

			arrowImage = new UIImageView (activity);
			arrowImage.SetScaleType (ImageView.ScaleType.CenterInside);
			arrowImage.ImageResource = Resource.Drawable.nool;

			AddViews (
				nameLabel, 
				freeSlotsLabel,
				arrowImage
			);

			Frame = new Frame (DeviceInfo.ScreenWidth - 2 * Sizes.ListViewPadding, Sizes.ParkingLotCellHeight);
		}

		public void UpdateInfo (ParkingLotInfo parkingLotInfo)
		{
			info = parkingLotInfo;

			nameLabel.Text = parkingLotInfo.Name;
			freeSlotsLabel.Text = "Free slots: " + parkingLotInfo.Spots;

			LayoutSubviews ();
		}

		public override void LayoutSubviews ()
		{
			arrowImage.Frame = new Frame (
				Frame.W - Sizes.ParkingLotCellArrowSize - Sizes.ParkingLotCellArrowPadding,
				(Frame.H - Sizes.ParkingLotCellArrowSize) / 2,
				Sizes.ParkingLotCellArrowSize,
				Sizes.ParkingLotCellArrowSize
			);

			nameLabel.Frame = new Frame (
				Sizes.ParkingLotCellLabelPadding,
				0,
				arrowImage.Frame.X - Sizes.ParkingLotCellLabelPadding - Sizes.ParkingLotCellArrowPadding,
				Frame.H / 2
			);

			freeSlotsLabel.Frame = new Frame (
				Sizes.ParkingLotCellLabelPadding,
				Frame.H / 2,
				arrowImage.Frame.X - Sizes.ParkingLotCellLabelPadding - Sizes.ParkingLotCellArrowPadding,
				Frame.H / 2
			);
		}
	}




}
