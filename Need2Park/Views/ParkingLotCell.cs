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
		ParkingLotCellLabel locationLabel;

		public ParkingLotInfo info {
			get;
			set;
		}

		public ParkingLotCell (Activity activity) : base (activity)
		{
			BackgroundColor = Color.LightBlue;

			nameLabel = new ParkingLotCellLabel (activity);
			locationLabel = new ParkingLotCellLabel (activity);

			AddViews (
				nameLabel, 
				locationLabel
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, Sizes.ParkingLotCellHeight);
		}

		public void UpdateInfo (ParkingLotInfo parkingLotInfo)
		{
			info = parkingLotInfo;

			nameLabel.Text = parkingLotInfo.Name;
			locationLabel.Text = parkingLotInfo.Location;

			LayoutSubviews ();
		}

		public override void LayoutSubviews ()
		{
			nameLabel.Frame = new Frame (
				Sizes.ParkingLotCellLabelPadding,
				0,
				Frame.W - 2 * Sizes.ParkingLotCellLabelPadding,
				Frame.H / 2
			);

			locationLabel.Frame = new Frame (
				Sizes.ParkingLotCellLabelPadding,
				Frame.H / 2,
				Frame.W - 2 * Sizes.ParkingLotCellLabelPadding,
				Frame.H / 2
			);
		}
	}




}
