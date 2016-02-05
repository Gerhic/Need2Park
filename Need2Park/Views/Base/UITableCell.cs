using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;

namespace Need2Park
{
	public class UITableCell : UIView
	{
		private Frame frame;
		public override Frame Frame {
			get {
				return frame ?? new Frame ();
			}
			set {
				frame = value;
				LayoutParameters = new AbsListView.LayoutParams (Frame.W, Frame.H);

				LayoutSubviews ();
			}
		}

		public UITableCell (Activity context) : base (context)
		{
		}
	}
}
