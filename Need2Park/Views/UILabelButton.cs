using System;
using Android.App;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Views.InputMethods;

namespace Need2Park
{
	class UILabelButton : UILabel
	{
		public UILabelButton (Activity activity) : base (activity)
		{
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down) {
				Alpha = 0.7f;
			}
			if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel) {
				Alpha = 1;
			}
			return base.OnTouchEvent (e);
		}
	}

}