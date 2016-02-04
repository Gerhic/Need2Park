using System;

namespace Need2Park
{
	static class Sizes
	{
		static int GetRealSize (float size)
		{
			return (int)(size * DeviceInfo.Density);
		}

		public static int ButtonHeight {
			get { 
				return GetRealSize (60);
			}
		}

		public static int MenuButtonPadding {
			get { 
				return GetRealSize (DeviceInfo.ScreenWidth / 10);
			}
		}

		public static float CornerRadius {
			get { 
				return 6 * DeviceInfo.Density;
			}
		}

		public static int HilightBarHeight {
			get { 
				return GetRealSize (4);
			}
		}

		public static int HorizontalMenuHeight {
			get { 
				return GetRealSize (60);
			}
		}
	}
}

