using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	public class UIListView : ListView
	{
		public int DirectionUp = 0;
		public int DirectionDown = 1;
		public int DirectionNone = 2;
		public int CurrentDirection;

		int oldX;
		int oldY;
		int newX;
		int newY;

		int buffer = 10;

		Frame frame;

		public virtual Frame Frame {
			get {
				if (frame == null) {
					return new Frame ();
				}
				return frame;
			} set {
				frame = value;

				RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(value.W, value.H);
				parameters.LeftMargin = value.X;
				parameters.TopMargin = value.Y;

				LayoutParameters = parameters;

				LayoutSubviews ();
			}
		}

		public UIListView (Activity context) : base (context.ApplicationContext) // need to pass app context here to prevent memory leaks
		{
			buffer = (int)(DeviceInfo.Density * buffer);
		}

		public virtual void LayoutSubviews ()
		{

		}

		bool IsOnLeftEdgeOfScreen (int x)
		{
			int drawerArea = DeviceInfo.ScreenWidth / 30;

			return x <= drawerArea;
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (e.Action == MotionEventActions.Move) {
				if (oldX == 0) {
					oldX = (int)e.GetX ();
				}
				if (oldY == 0) {
					oldY = (int)e.GetY ();
				}
				newX = (int)e.GetX ();
				newY = (int)e.GetY ();

				if (newY > oldY + buffer) {
					CurrentDirection = DirectionUp;
				} else if (newY + buffer < oldY) {
					CurrentDirection = DirectionDown;
				} else {
					CurrentDirection = DirectionNone;
				}

				if (newY > oldY + buffer) {
					oldY = (int)e.GetY ();
				} 
				if (newY + buffer < oldY) {
					oldY = (int)e.GetY ();
				}
				oldX = (int)e.GetX ();
			} else if (e.Action == MotionEventActions.Up) {
				oldX = 0;
				oldY = 0;
			}
			return base.OnTouchEvent (e);
		}
	}
}
