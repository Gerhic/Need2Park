using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	public class MapView : UIView
	{
		UIImageView mapImage;
		public MapView (Activity activity) : base (activity)
		{
			mapImage = new UIImageView (activity);
			mapImage.ImageResource = Resource.Drawable.map;
			mapImage.SetScaleType (ImageView.ScaleType.CenterCrop);



			AddViews (
				mapImage
			);
		}

		public override void LayoutSubviews ()
		{
			mapImage.Frame = Frame.Bounds;
		}
	}


}
