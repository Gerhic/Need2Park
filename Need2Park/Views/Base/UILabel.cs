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
	public class UILabel : TextView
	{
		public const string LEFT_MARGIN = "LeftMargin";
		public const string TOP_MARGIN = "TopMargin";
		public const string NEW_WIDTH = "NewWidth";

		Frame frame;
		Color color;
		Color backgroundColor;
		Font font;

		public virtual Frame Frame {
			get {
				if (frame == null) {
					return new Frame ();
				}
				return frame;
			}
			set {
				frame = value;
				LayoutParameters = LayoutUtils.GetRelative (frame.X, frame.Y, frame.W, frame.H);
			}
		}

		public Color TextColor {
			get {
				return color;
			}
			set {
				color = value;
				SetTextColor (color);
			}
		}

		public Color BackgroundColor {
			get {
				return backgroundColor;
			}
			set {
				backgroundColor = value;
				SetBackgroundColor (backgroundColor);
			}
		}

		public Font Font {
			get {
				return font;
			}
			set {
				font = value;
				if (font.Bold) {
					SetTypeface (Typeface.Create (font.Name, TypefaceStyle.Bold), TypefaceStyle.Bold);
				} else {
					SetTypeface (Typeface.Create (font.Name, TypefaceStyle.Normal), TypefaceStyle.Normal);
				}

				if (font.Size != 0) {
					SetTextSize (ComplexUnitType.Pt, font.Size);
				}
			}
		}

		public ObjectAnimator AlphaOutAnimator {
			get {
				return ObjectAnimator.OfFloat (this, "Alpha", 1, 0);
			}
		}

		public ObjectAnimator AlphaInAnimator {
			get {
				return ObjectAnimator.OfFloat (this, "Alpha", 0, 1);
			}
		}

		public UILabel (Activity context) : base (context.ApplicationContext) // need to pass app context here to prevent memory leaks
		{
			
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
			
		public void AnimateX (Frame newFrame, Action completed)
		{
			ObjectAnimator xAnim = ObjectAnimator.OfInt (this, LEFT_MARGIN, Frame.X, newFrame.X);

			xAnim.SetDuration (200);

			xAnim.Start ();

			xAnim.AnimationEnd += delegate {
				completed ();
			};
		}

		public void SetSlightlyRoundWithBackgroundColor (Color color, int radius = 0)
		{
			GradientDrawable background = new GradientDrawable ();
			if (radius == 0) {
				background.SetCornerRadius (MeasuredHeight / 10);
			} else {
				background.SetCornerRadius (radius);
			}
			background.SetColor (color);

			Background = background;
		}
	}
}

