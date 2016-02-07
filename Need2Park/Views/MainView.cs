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
		public LocationView mapView;
		MyPlacesView myPlacesView;
		UIView contentContainer;

		public MainView (MainActivity activity) : base (activity)
		{
			BackgroundColor = CustomColors.LightColor;
			horizontalMenu = new HorizontalMenu (activity);

			contentContainer = new UIView (activity);

			mapView = new LocationView (activity);
			mapView.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			myPlacesView = new MyPlacesView (activity);

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
			myPlacesView.Frame = contentContainer.Frame.Bounds;
		}

		bool isMapOpen = true;
		void SwitchContent (object sender, System.EventArgs e)
		{
			if (isMapOpen) {
				myPlacesView.UpdateList ();
			}

			int mapTransition = 0;
			int myPlacesTransition = Frame.W;

			if (isMapOpen) {
				mapTransition = -Frame.W;
				myPlacesTransition = 0;
			}

			mapView.Animate ().TranslationX (mapTransition);
			myPlacesView.Animate ().TranslationX (myPlacesTransition);
			isMapOpen = !isMapOpen;
		}

		public void UpdateViews ()
		{
			myPlacesView.UpdateList ();
		}

		public override bool DispatchKeyEvent (KeyEvent e)
		{
			if (e.KeyCode == Keycode.Back && !isMapOpen) {
				horizontalMenu.RestoreInitalState ();
				return true;
			} else {
				return base.DispatchKeyEvent (e);
			}
		}
	}
}