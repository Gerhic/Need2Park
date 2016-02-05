using System;
using Android.App;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views.InputMethods;

namespace Need2Park
{
	class LoginView : UIView
	{
		UITextField emailInput;
		UITextField passwordInput;
		UILabel loginButton;

		UIView inputContainer;
		UIView separator;

		bool isLoginInProgess;

		LoginActivity activity;

		public LoginView (LoginActivity activity) : base (activity)
		{
			this.activity = activity;
			BackgroundColor = Color.LightSeaGreen;

			inputContainer = new UIView (activity);
			inputContainer.SetRoundBordersWithColor (Color.White, Sizes.LoginInputHeight / 2, Sizes.LoginSeparatorSize);

			emailInput = new UITextField (activity);
			emailInput.Hint = "E-Mail";
			emailInput.Gravity = GravityFlags.Center;
			emailInput.SetSingleLine ();
			emailInput.InputType = Android.Text.InputTypes.TextVariationEmailAddress;

			passwordInput = new UITextField (activity);
			passwordInput.Hint = "Password";
			passwordInput.Gravity = GravityFlags.Center;
			passwordInput.SetSingleLine ();
			passwordInput.InputType = Android.Text.InputTypes.TextVariationPassword;

			separator = new UIView (activity);
			separator.BackgroundColor = Color.White;

			inputContainer.AddViews (
				emailInput,
				separator,
				passwordInput
			);

			loginButton = new UILabel (activity);
			loginButton.Text = "Log in";
			loginButton.Gravity = GravityFlags.Center;
			loginButton.BackgroundColor = Color.Black;
			loginButton.Font = Font.Get (FontStyle.Serif, 11);
			loginButton.Measure (0, 0);
			loginButton.TextColor = Color.White;

			int radius = (loginButton.MeasuredHeight + 4 * Sizes.LoginInputPadding) / 2;
			loginButton.SetCornerRadiusWithColor (Color.Black, 
				new float[] {
					0, 0,
					0, 0, 
					radius, radius, 
					radius, radius
				}
			);

			AddViews (
				inputContainer,
				loginButton
			);

			Frame = new Frame (DeviceInfo.ScreenWidth, DeviceInfo.ScreenHeight - DeviceInfo.StatusBarHeight);

			loginButton.Click += HandleLoginClicked;
		}

		public override void LayoutSubviews ()
		{
			int containerHeight = 2 * Sizes.LoginInputHeight + Sizes.LoginSeparatorSize;

			int loginButtonHeight = loginButton.MeasuredHeight + 4 * Sizes.LoginInputPadding;
			int loginButtonWidth = loginButton.MeasuredWidth + 8 * Sizes.LoginInputPadding;

			inputContainer.Frame = new Frame (
				Sizes.LoginHorizontalPadding,
				(Frame.H - containerHeight - loginButtonHeight) / 3,
				Frame.W - 2 * Sizes.LoginHorizontalPadding,
				containerHeight
			);

			emailInput.Frame = new Frame (
				Sizes.LoginInputPadding,
				0,
				inputContainer.Frame.W - 2 * Sizes.LoginInputPadding,
				Sizes.LoginInputHeight
			);

			separator.Frame = new Frame (
				Sizes.LoginInputPadding,
				emailInput.Frame.Bottom,
				inputContainer.Frame.W - 2 * Sizes.LoginInputPadding,
				Sizes.LoginSeparatorSize
			);

			passwordInput.Frame = new Frame (
				Sizes.LoginInputPadding,
				separator.Frame.Bottom,
				inputContainer.Frame.W - 2 * Sizes.LoginInputPadding,
				Sizes.LoginInputHeight
			);

			loginButton.Frame = new Frame (
				(Frame.W - loginButtonWidth) / 2,
				inputContainer.Frame.Bottom,
				loginButtonWidth,
				loginButtonHeight
			);
		}

		async void HandleLoginClicked (object sender, EventArgs e)
		{
			emailInput.ClearFocus ();
			passwordInput.ClearFocus ();

			HideKeyboard ();

			if (!isLoginInProgess) {
				if (string.IsNullOrWhiteSpace (emailInput.Text)
				   || string.IsNullOrWhiteSpace (passwordInput.Text)) {
					// bad input
				} else {
					isLoginInProgess = true;

					LoginRequest loginRequest = new LoginRequest {
						Email = emailInput.Text,
						Password = passwordInput.Text
					};

					LoginResponse response = await Networking.SendLoginRequest (loginRequest);

					if (string.IsNullOrEmpty (response.SessionId)) {
						DialogUtils.CreateDialog (activity, "Oops!", "Failed to log in with these credentials.");
					} else {
						UserInfo user = new UserInfo {
							Name = response.Name,
							Email = response.email,
							SessionId = response.SessionId
						};
						LoginState.ActiveUser = user;
						activity.CacheUser (user);
						List<ParkingLotInfo> parkingLotInfoList = await Networking.SendParkingLotsRequest ();
						if (parkingLotInfoList != null) {
							LoginState.ActiveUser.AccessInfo.AddRange (parkingLotInfoList);
						} else {
							// TODO handle failing in life
						}
						activity.Finish ();
					}
					isLoginInProgess = false;
				}
			}
		}

		const string inputMethod = "input_method";
		void HideKeyboard ()
		{
			InputMethodManager manager = (InputMethodManager)activity.GetSystemService (inputMethod);
			manager.HideSoftInputFromWindow (this.WindowToken, 0);
		}
	}
}