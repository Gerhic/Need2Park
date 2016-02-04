using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	class PopupView : UIView
	{
		protected int animationDurationMs = 300;
		WindowManagerLayoutParams windowManagerLayoutParams;
		IWindowManager WindowManager;

		bool isAddedToActivity;

		public PopupView (Activity activity, IWindowManager WindowManager) : base (activity)
		{
			this.WindowManager = WindowManager;
			windowManagerLayoutParams = new WindowManagerLayoutParams ();
			windowManagerLayoutParams.Width = ViewGroup.LayoutParams.MatchParent;
			windowManagerLayoutParams.Height = ViewGroup.LayoutParams.MatchParent;
			windowManagerLayoutParams.Type = WindowManagerTypes.Application;
			windowManagerLayoutParams.Format = Format.Transparent;
		}

		public void AddToActivity ()
		{
			if (!isAddedToActivity) {
				WindowManager.AddView (this, windowManagerLayoutParams);
				isAddedToActivity = true;
			}
		}

		public void RemoveFromActivity ()
		{
			if (isAddedToActivity) {
				WindowManager.RemoveView (this);
				isAddedToActivity = false;
			}
		}
	}


}
