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
		UIImageView backgroundImage;

		UITextField emailInput;
		UITextField passwordInput;
		UILabelButton loginButton;

		UIView inputContainer;
		UIView separator;

		bool isLoginInProgess;

		LoginActivity activity;

		LoaderView loaderView;

		public LoginView (LoginActivity activity) : base (activity)
		{
			this.activity = activity;

			backgroundImage = new UIImageView (activity);
			backgroundImage.ImageResource = Resource.Drawable.landing_backg;
			backgroundImage.SetScaleType (Android.Widget.ImageView.ScaleType.CenterCrop);
			backgroundImage.LayoutParameters = LayoutUtils.GetRelativeMatchParent ();

			inputContainer = new UIView (activity);
			inputContainer.SetRoundBordersWithColor (CustomColors.LightColor, Sizes.LoginInputHeight / 3, Sizes.LoginSeparatorSize);
			FocusableInTouchMode = true;
			emailInput = new UITextField (activity);
			emailInput.Hint = "E - M A I L";
			emailInput.Gravity = GravityFlags.Center;
			emailInput.SetSingleLine ();
			emailInput.InputType = Android.Text.InputTypes.TextVariationEmailAddress;
			emailInput.TextSize = Sizes.GetRealSize (9);
			emailInput.TextColor = CustomColors.LightColor;
			emailInput.SetHintTextColor (CustomColors.LightColorDim);
			emailInput.SetPadding (1, 1, 1, 1);
			emailInput.ClearFocus ();

			passwordInput = new UITextField (activity);
			passwordInput.Hint = "P A S S W O R D";
			passwordInput.Gravity = GravityFlags.Center;
			passwordInput.SetSingleLine ();
			passwordInput.InputType = Android.Text.InputTypes.TextVariationPassword;
			passwordInput.TextSize = Sizes.GetRealSize (9);
			passwordInput.TextColor = CustomColors.LightColor;
			passwordInput.SetPadding (1, 1, 1, 1);
			passwordInput.SetHintTextColor (CustomColors.LightColorDim);

			separator = new UIView (activity);
			separator.BackgroundColor = CustomColors.LightColor;

			inputContainer.AddViews (
				emailInput,
				separator,
				passwordInput
			);

			loginButton = new UILabelButton (activity);
			loginButton.Text = "L O G   I N";
			loginButton.Gravity = GravityFlags.Center;
			loginButton.BackgroundColor = CustomColors.DarkColor;
			loginButton.TextSize = Sizes.GetRealSize (9);
			loginButton.SetPadding (1, 1, 1, 1);
//			loginButton.Font = Font.Get (FontStyle.Serif, 11);
			loginButton.Measure (0, 0);
			loginButton.TextColor = CustomColors.LightColor;

			int radius = (int)(loginButton.MeasuredHeight * 0.7f);
			loginButton.SetCornerRadiusWithColor (CustomColors.DarkColor, 
				new float[] {
					0, 0,
					0, 0, 
					radius, radius, 
					radius, radius
				}
			);

			loaderView = new LoaderView (activity);

			AddViews (
				backgroundImage,
				inputContainer,
				loginButton,
				loaderView
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

			loaderView.Frame = new Frame (
				(Frame.W - Sizes.LoaderSize) / 2,
				(int)((Frame.H - Sizes.LoaderSize) * 0.8f),
				Sizes.LoaderSize,
				Sizes.LoaderSize
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
					loaderView.ShowAnimation ();

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

					loaderView.StopAnimating ();
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