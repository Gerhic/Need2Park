using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	public class MapView : UIView
	{
		UILabel label;
		public MapView (Activity activity) : base (activity)
		{
			label = new UILabel (activity);
			label.Gravity = GravityFlags.Center;
			label.TextColor = Color.Black;
			label.Text = "Map";
			AddView (label);
		}

		public override void LayoutSubviews ()
		{
			label.Frame = Frame.Bounds;
		}
	}


}
