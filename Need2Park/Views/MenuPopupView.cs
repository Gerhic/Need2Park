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
		UIView closeTouchableArea;
		UIView container;
		int popupWidth;

		MenuButton LoginButton;
		UIView buttonsContainer;

		MainActivity activity;

		UILabel userNameLabel;

		UIView backgroundView;

		public MenuPopupView (MainActivity activity, IWindowManager WindowManager) : base (activity, WindowManager)
		{
			backgroundView = new UIView (activity);
			backgroundView.BackgroundColor = Color.Black;
			backgroundView.Alpha = 0;
			backgroundView.Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.ScreenHeight);

			this.activity = activity;
			popupWidth = DeviceInfo.ScreenWidth;

			closeTouchableArea = new UIView (activity);
			closeTouchableArea.Frame = new Frame (
				(int)(popupWidth * 0.8), 
				0, 
				(int)(popupWidth * 0.2), 
				ViewGroup.LayoutParams.MatchParent
			);

			container = new UIView (activity);
			container.BackgroundColor = CustomColors.DarkColor;
			container.Frame = new Frame (
				(int)(popupWidth * 0.8), 
				DeviceInfo.ScreenHeight
			);
			container.TranslationX = -1 * popupWidth;

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
			userNameLabel.Typeface = Typeface.CreateFromAsset(activity.Assets, "fonts/Lato-Bold.ttf");
			userNameLabel.TextSize = Sizes.GetRealSize (10);

			userNameLabel.TextColor = CustomColors.LightColor;
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

			AddViews (
				backgroundView,
				closeTouchableArea, 
				container
			);

			UpdateView ();
		}

		public void ShowAnimated ()
		{
			AddToActivity ();
			container.Animate ().TranslationX (0);
			backgroundView.Animate ().Alpha (0.5f);
			closeTouchableArea.Touch += CloseAnimated;
		}

		async void CloseAnimated (object sender, System.EventArgs e)
		{
			closeTouchableArea.Touch -= CloseAnimated;
			container.Animate ().TranslationX (-1 * popupWidth);
			backgroundView.Animate ().Alpha (0);
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
				userNameLabel.Text = "USER: " + LoginState.ActiveUser.Name;
				LoginButton.Text = "Logout";
			} else {
				userNameLabel.Visibility = ViewStates.Gone;
				LoginButton.Text = "Login";
			}
		}
	}
}
