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

		MenuButton profileButton;
		MenuButton historyButton;
		MenuButton infoButton;
		MenuButton loginButton;


		UIView buttonsContainer;

		MainActivity activity;

		UILabel userNameLabel;

		UIView backgroundView;

		public bool isOpen;

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

			loginButton = new MenuButton (activity);
			loginButton.Click += OnLoginClick;

			infoButton = new MenuButton (activity);
			infoButton.Text = Strings.Info;

			historyButton = new MenuButton (activity);
			historyButton.Text = Strings.History;

			profileButton = new MenuButton (activity);
			profileButton.Text = Strings.Profile;

			buttonsContainer = new UIView (activity);
			buttonsContainer.AddViews (
				loginButton,
				infoButton,
				profileButton,
				historyButton
			);

			userNameLabel = new UILabel (activity);
			userNameLabel.Gravity = GravityFlags.Center;
			userNameLabel.Typeface = Typeface.CreateFromAsset(activity.Assets, "fonts/Lato-Bold.ttf");
			userNameLabel.TextSize = Sizes.GetRealSize (9);

			userNameLabel.TextColor = CustomColors.LightColor;
			userNameLabel.SetSingleLine ();
			userNameLabel.Ellipsize = Android.Text.TextUtils.TruncateAt.End;


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
			isOpen = true;
			AddToActivity ();
			container.Animate ().TranslationX (0);
			backgroundView.Animate ().Alpha (0.5f);
			closeTouchableArea.Touch += CloseAnimated;
		}

		async void CloseAnimated (object sender, System.EventArgs e)
		{
			isOpen = false;
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
				CloseAnimated (new object (), null);
			} else {
				var user = LoginState.ActiveUser;
				LoginState.ActiveUser = null;
				Networking.SendLogoutRequest (user);
				loginButton.Text = Strings.Login;
				activity.ClearUserCache ();
				userNameLabel.Visibility = ViewStates.Gone;
				activity.HandeLogout ();
				LayoutSubviews ();
			}
		}

		public void UpdateView ()
		{
			if (LoginState.ActiveUser != null) {
				userNameLabel.Visibility = ViewStates.Visible;
				userNameLabel.Text = "USER: " + LoginState.ActiveUser.Name;
				loginButton.Text = Strings.Logout;
			} else {
				userNameLabel.Visibility = ViewStates.Gone;
				loginButton.Text = Strings.Login;
			}
			LayoutSubviews ();
		}

		public void HideMenu (bool animated = true)
		{
			if (animated) {
				CloseAnimated (new object (), null);
			} else {
				Close ();
			}
		}

		void Close ()
		{
			isOpen = false;
			closeTouchableArea.Touch -= CloseAnimated;
			container.TranslationX = -1 * popupWidth;
			backgroundView.Alpha = 0;
			RemoveFromActivity ();
		}

		public override bool DispatchKeyEvent (KeyEvent e)
		{
			if (e.KeyCode == Keycode.Back) {
//				context.DispatchKeyEvent (e);
				CloseAnimated (new object (), null);
				return true;
			} else {
				return base.DispatchKeyEvent (e);
			}
		}

		public override void LayoutSubviews ()
		{
			int y = 0;

			if (LoginState.ActiveUser != null) {
				profileButton.Frame = new Frame (
					0,
					y,
					container.Frame.W - 2 * Sizes.MenuButtonPadding, 
					Sizes.MenuButtonHeight
				);
				y += Sizes.MenuButtonHeight;
			} else {
				profileButton.Frame = new Frame ();
			}

			if (LoginState.ActiveUser != null) {
				historyButton.Frame = new Frame (
					0,
					y,
					container.Frame.W - 2 * Sizes.MenuButtonPadding, 
					Sizes.MenuButtonHeight
				);
				y += Sizes.MenuButtonHeight;
			} else {
				historyButton.Frame = new Frame ();
			}

			infoButton.Frame = new Frame (
				0,
				y,
				container.Frame.W - 2 * Sizes.MenuButtonPadding, 
				Sizes.MenuButtonHeight
			);
			y += Sizes.MenuButtonHeight;

			loginButton.Frame = new Frame (
				0,
				y,
				container.Frame.W - 2 * Sizes.MenuButtonPadding, 
				Sizes.MenuButtonHeight
			);
			y += Sizes.MenuButtonHeight;

			buttonsContainer.Frame = new Frame (
				Sizes.MenuButtonPadding,
				(container.Frame.H - y) / 2,
				container.Frame.W - 2 * Sizes.MenuButtonPadding,
				y
			);

			userNameLabel.Frame = new Frame (
				Sizes.MenuButtonPadding,
				buttonsContainer.Frame.Y - Sizes.UserNameLabelHeight - Sizes.UserNameLabelPadding,
				container.Frame.W - 2 * Sizes.MenuButtonPadding,
				Sizes.UserNameLabelHeight
			);
		}
	}
}