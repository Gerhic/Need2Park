using System;
using Android.Widget;
using Android.Graphics;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Graphics.Drawables;
using Android.Runtime;

namespace Need2Park
{
	public class UITextField : EditText
	{
		public const string InputMethod = "input_method";

		public const string LEFT_MARGIN = "LeftMargin";
		public const string TOP_MARGIN = "TopMargin";
		public const string NEW_WIDTH = "NewWidth";

		const int NONE = 0;

		Frame frame;
		Color color;
		Color backgroundColor;
		Font font;

		public EventHandler<EventArgs> HardCancel;

		public EventHandler<EventArgs> WillShowKeyboard;

		public bool HasPaddingBeenModified {
			get {
				return PaddingLeft == 0;		
			}
		}

		public Frame Frame {
			get {
				return frame;
			}
			set {
				frame = value;
				LayoutParameters = LayoutUtils.GetRelative (frame.X, frame.Y, frame.W, frame.H);

				if (HasPaddingBeenModified) {
					SetPadding (frame.H / 5, NONE, NONE, NONE);
				}

				LayoutSubviews ();
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

				SetTextSize (ComplexUnitType.Px, font.Size);
			}
		}

		Activity context;

		public UITextField (Activity context) : base (context.ApplicationContext) // need to pass app context here to prevent memory leaks
		{
			this.context = context;

			BackgroundColor = Color.Transparent;

			Gravity = GravityFlags.CenterVertical;
		}

		public virtual void LayoutSubviews ()
		{

		}

		public virtual void Hide ()
		{
			Visibility = ViewStates.Gone;
		}

		public override bool OnKeyPreIme (Keycode keyCode, KeyEvent e)
		{
			var handler = HardCancel;

			if (handler != null) {
				handler (this, new EventArgs ());
				return true;
			}

			return base.OnKeyPreIme (keyCode, e);
		}

		public virtual void Show ()
		{
			Visibility = ViewStates.Visible;
		}

		public void ShowKeyboard ()
		{
			InputMethodManager manager = (InputMethodManager)context.GetSystemService (InputMethod);
			manager.ShowSoftInput (this, ShowFlags.Implicit);
			if (WillShowKeyboard != null) {
				WillShowKeyboard (this, new EventArgs ());
			}
		}

		public void HideKeyboard ()
		{
			InputMethodManager manager = (InputMethodManager)context.GetSystemService (InputMethod);
			manager.HideSoftInputFromWindow (this.WindowToken, 0);
		}

		public void SetSlightlyRoundWithBackgroundColor (Color color)
		{
			GradientDrawable background = new GradientDrawable ();
			background.SetCornerRadius (Sizes.CornerRadius);
			background.SetColor (color);

			Background = background;
		}

		public void SetCursorDrawable ()
		{
			IntPtr intPtr = JNIEnv.FindClass (typeof(EditText));
			IntPtr cursorDrawable = JNIEnv.GetFieldID (intPtr, "mCursorDrawableRes", "I");
			JNIEnv.SetField (Handle, cursorDrawable, Resource.Drawable.SearchCursor);
		}
	}
}

