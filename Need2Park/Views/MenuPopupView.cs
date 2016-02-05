using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content;

namespace Need2Park
{
	class MenuPopupView : PopupView
	{
		UIView background;
		UIView container;
		int popupWidth;

		MenuButton LoginButton;
		UIView buttonsContainer;

		MainActivity activity;

		UILabel userNameLabel;

		public MenuPopupView (MainActivity activity, IWindowManager WindowManager) : base (activity, WindowManager)
		{
			this.activity = activity;
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

			userNameLabel = new UILabel (activity);
			userNameLabel.Gravity = GravityFlags.Center;
			userNameLabel.Font = Font.Get (FontStyle.Serif, 10);
			userNameLabel.TextColor = Color.Black;
			userNameLabel.SetSingleLine ();
			userNameLabel.Ellipsize = Android.Text.TextUtils.TruncateAt.End;
			userNameLabel.Frame = new Frame (
				Sizes.UserNameLabelPadding,
				buttonsContainer.Frame.Y - Sizes.UserNameLabelHeight - Sizes.UserNameLabelPadding,
				container.Frame.W - 2 * Sizes.UserNameLabelPadding,
				Sizes.UserNameLabelHeight
			);

			container.AddViews (
				buttonsContainer,
				userNameLabel
			);

			AddViews (background, container);

			UpdateView ();
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
			if (LoginState.ActiveUser == null) {
				Intent loginIntent = new Intent (activity, typeof(LoginActivity));
				loginIntent.AddFlags (ActivityFlags.ClearTop);
				activity.StartActivity (loginIntent);
			} else {
				var user = LoginState.ActiveUser;
				LoginState.ActiveUser = null;
				Networking.SendLogoutRequest (user);
				LoginButton.Text = "Login";
				activity.ClearUserCache ();
				userNameLabel.Visibility = ViewStates.Gone;
				activity.HandeLogout ();
			}
		}

		public void UpdateView ()
		{
			if (LoginState.ActiveUser != null) {
				userNameLabel.Visibility = ViewStates.Visible;
				userNameLabel.Text = "Logged in as: " + LoginState.ActiveUser.Name;
				LoginButton.Text = "Logout";
			} else {
				userNameLabel.Visibility = ViewStates.Gone;
				LoginButton.Text = "Login";
			}
		}
	}
}
