using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Threading.Tasks;

namespace Need2Park
{
	class MenuButton : UILabel
	{
		public static Color RoundBackgroundColor = CustomColors.LightColor;

		public override Frame Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;
				SetSlightlyRoundWithBackgroundColor (RoundBackgroundColor, Frame.H / 2);
			}
		}

		public MenuButton (Activity activity) : base (activity)
		{
			TextColor = CustomColors.DarkColor;
//			Font = Font.Get (FontStyle.Serif, 14);
			TextSize = Sizes.GetRealSize (14);
			Gravity = GravityFlags.Center;
		}
	}

}
