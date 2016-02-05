using System;
using Android.App;
using Android.Widget;
using Android.Graphics;
using Java.Interop;
using Android.Animation;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Content;

namespace Need2Park
{
	public class UIView : RelativeLayout
	{
		public const string LEFT_MARGIN = "LeftMargin";
		public const string TOP_MARGIN = "TopMargin";
		public const string NEW_WIDTH = "NewWidth";

		const int ALPHAANIMATIONTIME = 100;
		const int FRAMEANIMATIONTIME = 200;

		Frame frame;
		Color backgroundColor;
		Color borderColor;
		bool hidden;

		UIView leftBorder, rightBorder, topBorder, bottomBorder;

		bool disposed;

		protected Activity context;

		public virtual Frame Frame {
			get {
				if (frame == null) {
					return new Frame ();
				}
				return frame;
			} set {
				frame = value;

				RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams (value.W, value.H);
				parameters.LeftMargin = value.X;
				parameters.TopMargin = value.Y;

				LayoutParameters = parameters;
				LayoutSubviews ();
			}
		}

		public virtual Color BackgroundColor {
			get {
				return backgroundColor;
			} set {
				backgroundColor = value;
				SetBackgroundColor (backgroundColor);
			}
		}

		public bool HasParent {
			get {
				return Parent != null;		
			}
		}

		public Rect HitRect {
			get {
				Rect cellRect = new Rect ();
				GetHitRect (cellRect);

				return cellRect;
			}
		}

		public Color BorderColor {
			get {
				return borderColor;		
			}
			set {
				borderColor = value;

				if (leftBorder != null) {
					leftBorder.BackgroundColor = borderColor;
				}
				if (rightBorder != null) {
					rightBorder.BackgroundColor = borderColor;
				}
				if (topBorder != null) {
					topBorder.BackgroundColor = borderColor;
				}
				if (bottomBorder != null) {
					bottomBorder.BackgroundColor = borderColor;
				}
			}
		}

		public bool Hidden {
			get {
				return hidden;
			} set {
				hidden = value;

				if (hidden) {
					Hide ();
				} else {
					Show ();
				}
			}
		}

		public ObjectAnimator AlphaOutAnimator {
			get {
				return ObjectAnimator.OfFloat (this, "Alpha", Alpha, 0);
			}
		}

		public ObjectAnimator AlphaInAnimator {
			get {
				return ObjectAnimator.OfFloat (this, "Alpha", Alpha, 1);
			}
		}

		public UIView (Activity context) : base (context.ApplicationContext) // need to pass app context here to prevent memory leaks
		{
			this.context = context;
		}


		public virtual void LayoutSubviews ()
		{

		}

		public void SetSlightlyRoundWithBackgroundColor (Color color)
		{
			var cornerRadius = 6 * DeviceInfo.Density;
			GradientDrawable background = new GradientDrawable ();
			background.SetCornerRadius (cornerRadius);
			background.SetColor (color);

			Background = background;

			RemoveBorders ();
		}

		public void SetRoundWithColor (Color color, int currentSize)
		{
			GradientDrawable background = new GradientDrawable ();
			background.SetCornerRadius (currentSize);
			background.SetColor (color);

			Background = background;

			RemoveBorders ();
		}

		public void SetMultiColorBackground (int[] colours)
		{
			GradientDrawable drawable = new GradientDrawable (GradientDrawable.Orientation.LeftRight, colours);
			Background = drawable;
		}

		public void AddBorders (Activity context, bool left, bool right, bool top, bool bottom)
		{
			if (left) {
				leftBorder = new UIView (context) { BackgroundColor = borderColor };
				AddView (leftBorder);
			}

			if (right) {
				rightBorder = new UIView (context) { BackgroundColor = borderColor };
				AddView (rightBorder);
			}

			if (top) {
				topBorder = new UIView (context) { BackgroundColor = borderColor };
				AddView (topBorder);
			}

			if (bottom) {
				bottomBorder = new UIView (context) { BackgroundColor = borderColor };
				AddView (bottomBorder);
			}
		}

		public void SetBorderFrames (int w)
		{
			if (leftBorder != null) {
				leftBorder.Frame = new Frame (w, Frame.H);
			}

			if (rightBorder != null) {
				rightBorder.Frame = new Frame (Frame.W - w, 0, w, Frame.H);
			}

			if (topBorder != null) {
				topBorder.Frame = new Frame (Frame.W, w);
			}

			if (bottomBorder != null) {
				bottomBorder.Frame = new Frame (0, Frame.H - w, Frame.W, w);
			}
		}

		public void RemoveBorders ()
		{
			RemoveView (leftBorder);
			RemoveView (rightBorder);
			RemoveView (topBorder);
			RemoveView (bottomBorder);
		}

		public void AnimateHide (Action completed)
		{
			ObjectAnimator animator = AlphaOutAnimator;
			animator.SetDuration (ALPHAANIMATIONTIME);

			animator.Start ();

			animator.AnimationEnd += delegate {
				completed ();
				Visibility = ViewStates.Gone;
			};	
		}

		public void AnimateShow (Action completed)
		{
			ObjectAnimator animator = AlphaInAnimator;

			animator.SetDuration (ALPHAANIMATIONTIME);
			animator.Start ();

			animator.AnimationEnd += delegate {
				completed ();
				Visibility = ViewStates.Visible;
			};
		}

		public void AnimateY (int y)
		{
			ObjectAnimator xAnim = ObjectAnimator.OfFloat (this, TOP_MARGIN, Frame.Y, y);

			xAnim.SetDuration (FRAMEANIMATIONTIME);

			xAnim.Start ();
		}

		public void AnimateX (Frame newFrame)
		{
			ObjectAnimator xAnim = ObjectAnimator.OfInt (this, LEFT_MARGIN, Frame.X, newFrame.X);

			xAnim.SetDuration (FRAMEANIMATIONTIME);

			xAnim.Start ();
		}

		public void AnimateWidth (Frame newFrame, Action completed)
		{
			ObjectAnimator wAnim = ObjectAnimator.OfInt (this, NEW_WIDTH, Frame.W, newFrame.W);

			wAnim.SetDuration (FRAMEANIMATIONTIME);

			wAnim.Start ();

			wAnim.AnimationEnd += delegate {
				completed ();
			};
		}

		public void AnimateXAndWidth (Frame newFrame, Action completed)
		{
			ObjectAnimator xAnim = ObjectAnimator.OfFloat (this, LEFT_MARGIN, (float)Frame.X, (float)newFrame.X);
			ObjectAnimator wAnim = ObjectAnimator.OfInt (this, NEW_WIDTH, Frame.W, newFrame.W);

			AnimatorSet set = new AnimatorSet ();
			set.SetDuration (FRAMEANIMATIONTIME);
			set.PlayTogether (new ObjectAnimator[] { xAnim, wAnim });

			set.Start ();

			set.AnimationEnd += delegate {
				completed ();
			};
		}

		public virtual void Hide ()
		{
			Alpha = 0;
			Visibility = ViewStates.Gone;
		}

		public virtual void Show ()
		{
			Alpha = 1;
			Visibility = ViewStates.Visible;
		}

		public void UpdateY (int y)
		{
			Frame = new Frame (Frame.X, y, Frame.W, Frame.H);
		}

		public void UpdateX (int x)
		{
			Frame = new Frame (x, Frame.Y, Frame.W, Frame.H);
		}

		public void UpdateHeight (int height)
		{
			Frame = new Frame (Frame.X, Frame.Y, Frame.W, height);
		}

		public void AddViews (params object[] items)
		{
			foreach (object item in items) {
				AddView (item as View);
			}
		}

		public void RemoveViews (params object[] items)
		{
			foreach (object item in items) {
				RemoveView (item as View);
			}
		}	

		public void UpdateFrameBy (int x, int y, int w, int h)
		{
			Frame = new Frame (Frame.X + x, Frame.Y + y, Frame.W + w, Frame.H + h);
		}

		public override void AddView (View child)
		{
			try {
				base.AddView (child);
			} catch {
				Console.WriteLine ("!!!! Caught exception: Failed to add View: " + child.GetType ());
			}
		}

		public int SizeHeightToFit ()
		{
			return SizeHeightToFitWithMin (0);
		}

		public int SizeHeightToFitWithMin (int min)
		{
			Measure (0, 0);

			int height = MeasuredHeight;

			if (MeasuredHeight < min) {
				height = min;
			}

			Frame = new Frame (Frame.X, Frame.Y, Frame.W, height);
			return height;
		}

		protected override void Dispose (bool disposing)
		{
			if (disposed) {
				return;
			}

			DisposeChildren (this);

			disposed = true;

			base.Dispose (disposing);
		}

		public static void DisposeChildren (ViewGroup vg)
		{
			if (vg.Handle == IntPtr.Zero) {
				// see http://developer.xamarin.com/guides/android/advanced_topics/garbage_collection/#Disposing_of_Peer_instances
				return;
			}
			for (int i = 0; i < vg.ChildCount; i++) {
				vg.GetChildAt (i).Dispose ();
			}
		}

		public void SetRoundBordersWithColor (Color color, int currentSize, int strokeSize)
		{
			GradientDrawable outerBackground = new GradientDrawable ();
			outerBackground.SetCornerRadius (currentSize);
			outerBackground.SetStroke (strokeSize, color);

			Background = outerBackground;
		}
	}
}

