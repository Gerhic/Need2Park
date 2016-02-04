using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	class HorizontalMenuLabel : UILabel
	{
		Color defaultTextColor;

		
		public HorizontalMenuLabel (Activity activity) : base (activity)
		{
			defaultTextColor = TextColor;
			Gravity = GravityFlags.Center;
			SetBold (false);
			TextColor = Color.White;
		}

		public void SetBold (bool b)
		{
//			if (b) {
//				TextColor = Color.White;
//			} else {
//				TextColor = defaultTextColor;
//			}
			Font = Font.Get (FontStyle.Serif, 10);
		}
	}



}
