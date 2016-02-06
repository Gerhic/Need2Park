using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System;
using Android.Animation;
using Android.Views.Animations;

namespace Need2Park
{
	class UIImageView : ImageView
	{
		const int ALPHAANIMATIONTIME = 100;

		protected Frame frame;
		Color backgroundColor;
		int imageResource;

		public bool IsHidden {
			get {
				return Alpha == 0;
			}
		}

		public virtual Frame Frame {
			get {
				if (frame == null) {
					frame = new Frame ();
				}
				return frame;
			}
			set {
				frame = value;
				LayoutParameters = LayoutUtils.GetRelative (frame.X, frame.Y, frame.W, frame.H);
				LayoutSubviews ();
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

		public bool HasImageResource {
			get {
				return imageResource != 0;
			}
		}

		public int ImageResource {
			get {
				return imageResource;
			}
			set {
				imageResource = value;
				SetImageResource (imageResource);
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

		public UIImageView (Activity context) : base (context.ApplicationContext) // need to pass app context here to prevent memory leaks
		{

		}

		public virtual void LayoutSubviews ()
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

		public void AnimateHide (Action completed)
		{
			ObjectAnimator animator = AlphaOutAnimator;
			animator.SetDuration (ALPHAANIMATIONTIME);

			animator.Start ();

			animator.AnimationEnd += delegate {
				Visibility = ViewStates.Gone;
				completed ();
			};	
		}

		public void AnimateShow (Action completed)
		{
			Visibility = ViewStates.Visible;

			ObjectAnimator animator = AlphaInAnimator;
			animator.SetDuration (ALPHAANIMATIONTIME);

			animator.Start ();

			animator.AnimationEnd += delegate {
				completed ();
			};
		}

		public static Bitmap GetRoundedCornerBitmap (Bitmap bitmap, int cornerRadius)
		{
			Bitmap output = Bitmap.CreateBitmap (bitmap.Width, bitmap.Height, Bitmap.Config.Argb8888);

			Canvas canvas = new Canvas(output);

			Paint paint = new Paint();
			Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
			RectF rectF = new RectF(rect);
			float roundPx = cornerRadius;

			paint.AntiAlias = true;
			canvas.DrawARGB(0, 0, 0, 0);
			paint.SetColorFilter (new ColorFilter ());
			canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);
			paint.SetXfermode (new PorterDuffXfermode (PorterDuff.Mode.SrcIn));
			canvas.DrawBitmap (bitmap, rect, rect, paint);

			return output;
		}

		bool rotating;
		RotateAnimation	animation;

		public void AnimateInfiniteRotation ()
		{
			if (!rotating) {
				rotating = true;

				if (animation == null) {
					animation = new RotateAnimation (359, 0, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
					animation.Duration = 1000;
					animation.FillAfter = true;
					animation.RepeatCount = Animation.Infinite;
					animation.Interpolator = new LinearInterpolator ();
				}

				StartAnimation (animation);
			}
		}

		public void ClearRotationAnimation ()
		{
			if (rotating) {
				rotating = false;
				ClearAnimation ();
			}
		}

		protected override void Dispose (bool disposing)
		{
			ClearRotationAnimation ();
			if (animation != null) {
				animation.Dispose ();
			}
			base.Dispose (disposing);
		}
	}
}
