using System;
using Android.App;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views.InputMethods;
using Android.Views.Animations;
using System.Threading.Tasks;

namespace Need2Park
{
	class LoaderView : UIView
	{
		UIView staticBall;
		UIView expandingBall;

		public LoaderView (Activity activity) : base (activity)
		{
			staticBall = new UIView (activity);
			expandingBall = new UIView (activity);

			AddViews (
				staticBall,
				expandingBall
			);
			Visibility = ViewStates.Gone;
		}

		public override void LayoutSubviews ()
		{
			staticBall.Frame = new Frame (
				Frame.W / 4,
				Frame.H / 4,
				Frame.W / 2,
				Frame.H / 2
			);
			expandingBall.Frame = new Frame (
				Frame.W / 4,
				Frame.H / 4,
				Frame.W / 2,
				Frame.H / 2
			);

			staticBall.SetRoundWithColor (Color.DarkSeaGreen, staticBall.Frame.W / 2);
			expandingBall.SetRoundWithColor (Color.DarkSeaGreen, expandingBall.Frame.W / 2);

		}

		protected int animationDurationMs = 800;

		bool shouldAnimate;

		public void ShowAnimation ()
		{
			Animate ().Alpha (1);
			Visibility = ViewStates.Visible;
			shouldAnimate = true;
			AnimateLoader ();
		}
		public void StopAnimating ()
		{
			Visibility = ViewStates.Gone;
			shouldAnimate = false;
		}

		async void AnimateLoader ()
		{
			if (shouldAnimate) {
				expandingBall.Animate ().Alpha (0);
				expandingBall.Animate ().ScaleX (2);
				expandingBall.Animate ().ScaleY (2);
				await Task.Delay (animationDurationMs);
				expandingBall.ScaleX = 1;
				expandingBall.ScaleY = 1;
				expandingBall.Alpha = 1;
				AnimateLoader ();
			}
		}
	}
}