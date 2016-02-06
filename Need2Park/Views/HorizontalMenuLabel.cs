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
			TextColor = CustomColors.DarkColor;
			TextSize = Sizes.GetRealSize (10);
		}
	}
}
