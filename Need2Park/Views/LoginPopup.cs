using System;
using Android.App;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	class LoginPopup : PopupView
	{
		UIView backgroundView;
		LoginView loginView;

		public LoginPopup (Activity activity, IWindowManager WindowManager) : base (activity, WindowManager)
		{
			backgroundView = new UIView (activity);
			backgroundView.BackgroundColor = Color.Black;
			backgroundView.Alpha = 0.7f;

			loginView = new LoginView (activity);


			AddViews (
				backgroundView
			);
		}
	}
}

