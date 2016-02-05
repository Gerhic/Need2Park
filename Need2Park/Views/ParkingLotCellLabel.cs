using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;

namespace Need2Park
{
	class ParkingLotCellLabel : UILabel
	{
		public ParkingLotCellLabel (Activity activity) : base (activity)
		{
			Gravity = GravityFlags.CenterVertical;
			TextColor = Color.Black;
			Ellipsize = Android.Text.TextUtils.TruncateAt.End;
			SetSingleLine ();
		}
	}





}
