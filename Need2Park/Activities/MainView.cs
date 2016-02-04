using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	class MainView : UIView
	{
		HorizontalMenu horizontalMenu;
		MapView mapView;
		MapView mapView2;

		public MainView (Activity activity) : base (activity)
		{
			horizontalMenu = new HorizontalMenu (activity);

			mapView = new MapView (activity);
			mapView.BackgroundColor = Color.Orange;

			mapView2 = new MapView (activity);
			mapView2.BackgroundColor = Color.AliceBlue;

			AddViews (
				horizontalMenu,
				mapView,
				mapView2
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.TrueScreenHeight);

			horizontalMenu.OnRandomEvent += SwitchMap;
		}

		public override void LayoutSubviews ()
		{
			horizontalMenu.Frame = new Frame (Frame.W, Sizes.HorizontalMenuHeight);
			mapView.Frame = new Frame (0, Sizes.HorizontalMenuHeight, Frame.W, Frame.H - Sizes.HorizontalMenuHeight);
			mapView2.Frame = new Frame (0, Sizes.HorizontalMenuHeight, Frame.W, Frame.H - Sizes.HorizontalMenuHeight);
			mapView2.TranslationX = Frame.W;
		}

		bool isMap1Open = true;
		void SwitchMap (object sender, System.EventArgs e)
		{
			int transition1 = 0;
			int transition2 = Frame.W;

			if (isMap1Open) {
				transition1 = -Frame.W;
				transition2 = 0;
			}
			mapView.Animate ().TranslationX (transition1);
			mapView2.Animate ().TranslationX (transition2);
			isMap1Open = !isMap1Open;
		}
	}

}
