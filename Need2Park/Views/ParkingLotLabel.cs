using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;

namespace Need2Park
{
	class ParkingLotLabel : UILabel
	{
		public ParkingLotLabel (Activity activity) : base (activity)
		{
			Gravity = GravityFlags.Center;
			TextColor = CustomColors.LightColor;
			Ellipsize = Android.Text.TextUtils.TruncateAt.End;
			SetSingleLine ();
		}
	}

}