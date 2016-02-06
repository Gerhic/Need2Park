using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;

namespace Need2Park
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class ParkingLotActivity : Activity
	{
		ParkingLotInfo parkingLotInfo;
		ParkingLotView contentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (PublicInfo.ActiveParkingLotInfo != null) {
				parkingLotInfo = PublicInfo.ActiveParkingLotInfo;
				Title = parkingLotInfo.Name;
				contentView = new ParkingLotView (this, parkingLotInfo);
				SetContentView (contentView);
			}
		}
	}

}