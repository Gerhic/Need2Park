using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Threading.Tasks;

namespace Need2Park
{
	class MenuPopupView : PopupView
	{
		UIView background;
		UIView container;
		int popupWidth;

		MenuButton LoginButton;
		UIView buttonsContainer;

		public MenuPopupView (Activity activity, IWindowManager WindowManager) : base (activity, WindowManager)
		{
			popupWidth = DeviceInfo.ScreenWidth;
			TranslationX = -1 * popupWidth;

			background = new UIView (activity);
			background.Frame = new Frame (
				(int)(popupWidth * 0.8), 
				0, 
				(int)(popupWidth * 0.2), 
				ViewGroup.LayoutParams.MatchParent
			);

			container = new UIView (activity);
			container.BackgroundColor = Color.Aqua;
			container.Frame = new Frame (
				(int)(popupWidth * 0.8), 
				DeviceInfo.ScreenHeight
			);

			LoginButton = new MenuButton (activity);
			LoginButton.Text = Strings.Login;
			LoginButton.Click += OnLoginClick;
			int y = 0;
			LoginButton.Frame = new Frame (
				0,
				y,
				ViewGroup.LayoutParams.MatchParent, 
				Sizes.ButtonHeight
			);
			y += Sizes.ButtonHeight;

			buttonsContainer = new UIView (activity);
			buttonsContainer.AddViews (
				LoginButton
			);
			buttonsContainer.Frame = new Frame (
				Sizes.MenuButtonPadding,
				(container.Frame.H - y) / 2,
				container.Frame.W - 2 * Sizes.MenuButtonPadding,
				y
			);

			container.AddViews (
				buttonsContainer
			);

			AddViews (background, container);
		}

		public void ShowAnimated ()
		{
			AddToActivity ();
			Animate ().TranslationX (0);
			background.Touch += CloseAnimated;
		}

		async void CloseAnimated (object sender, System.EventArgs e)
		{
			background.Touch -= CloseAnimated;
			Animate ().TranslationX (-1 * popupWidth);
			await Task.Delay (animationDurationMs);
			RemoveFromActivity ();
		}

		void OnLoginClick (object sender, System.EventArgs e)
		{
			LoginState.IsLoggedIn = !LoginState.IsLoggedIn;
			if (LoginState.IsLoggedIn) {
				LoginButton.Text = Strings.Logout;
			} else {
				LoginButton.Text = Strings.Login;
			}
		}
	}
}
