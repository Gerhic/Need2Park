using System;
using Android.Widget;
using Android.Graphics;
using Android.App;
using Android.Animation;
using Java.Interop;
using Android.Util;
using Android.Views;
using Android.Graphics.Drawables;

namespace Need2Park
{
	class LayoutUtils
	{
		public static RelativeLayout.LayoutParams GetRelative (int x, int y, int w, int h)
		{
			RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(w, h);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			return parameters;
		}
		public static RelativeLayout.LayoutParams GetRelativeMatchParent ()
		{
			return new RelativeLayout.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
	}
}

