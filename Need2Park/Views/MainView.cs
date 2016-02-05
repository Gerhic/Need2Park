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
		MyPlacesView myPlacesView;
		UIView contentContainer;

		public MainView (Activity activity) : base (activity)
		{
			horizontalMenu = new HorizontalMenu (activity);

			contentContainer = new UIView (activity);

			mapView = new MapView (activity);
			mapView.BackgroundColor = Color.Orange;
			mapView.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			myPlacesView = new MyPlacesView (activity);
			myPlacesView.SetBackgroundColor (Color.AliceBlue);
			myPlacesView.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			myPlacesView.TranslationX = DeviceInfo.ScreenWidth;

			contentContainer.AddViews (
				mapView,
				myPlacesView
			);

			AddViews (
				horizontalMenu,
				contentContainer
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.TrueScreenHeight);

			horizontalMenu.OnLabelClick += SwitchContent;
		}

		public override void LayoutSubviews ()
		{
			horizontalMenu.Frame = new Frame (Frame.W, Sizes.HorizontalMenuHeight);
			contentContainer.Frame = new Frame (0, Sizes.HorizontalMenuHeight, Frame.W, Frame.H - Sizes.HorizontalMenuHeight);
		}

		bool isMapOpen = true;
		void SwitchContent (object sender, System.EventArgs e)
		{
			if (isMapOpen) {
				myPlacesView.UpdateList ();
			}

			int transition1 = 0;
			int transition2 = Frame.W;

			if (isMapOpen) {
				transition1 = -Frame.W;
				transition2 = 0;
			}
			mapView.Animate ().TranslationX (transition1);
			myPlacesView.Animate ().TranslationX (transition2);
			isMapOpen = !isMapOpen;
		}

		public void UpdateViews ()
		{
			myPlacesView.UpdateList ();
		}
	}
}