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
		ParkingLotView contentView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			ActionBar.Hide ();

			if (PublicInfo.ActiveParkingLotInfo != null) {
				contentView = new ParkingLotView (this, PublicInfo.ActiveParkingLotInfo);
				SetContentView (contentView);
			} else {
				Finish ();
			}
		}
	}

}