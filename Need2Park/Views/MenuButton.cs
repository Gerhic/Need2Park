using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Threading.Tasks;

namespace Need2Park
{
	class MenuButton : UIView
	{
		public string Text {
			get { 
				return label.Text;
			}
			set { 
				label.Text = value;
			}
		}

		UILabel label;
		UIView underLine;

		public MenuButton (Activity activity) : base (activity)
		{
			label = new UILabel (activity);
			label.TextColor = CustomColors.LightColor;
			label.TextSize = Sizes.GetRealSize (10);
			label.Gravity = GravityFlags.CenterVertical;

			underLine = new UIView (activity);
			underLine.BackgroundColor = CustomColors.LightColor;

			AddViews (
				label,
				underLine
			);
		}

		public override void LayoutSubviews ()
		{
			label.Frame = Frame.Bounds;
			underLine.Frame = new Frame (
				0, 
				Sizes.MenuButtonHeight - Sizes.LoginSeparatorSize,
				Frame.W,
				Sizes.LoginSeparatorSize
			);
		}
	}

}
