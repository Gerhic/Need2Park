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

		public static int ParkingLotCellHeight {
			get { 
				return GetRealSize (70);
			}
		}

		public static int ParkingLotCellLabelPadding {
			get { 
				return GetRealSize (10);
			}
		}

		public static int ParkingViewVerticalPadding {
			get { 
				return GetRealSize (60);
			}
		}

		public static int ParkingViewHorizontalPadding {
			get { 
				return GetRealSize (40);
			}
		}

		public static int ParkingViewLabelHeight {
			get { 
				return GetRealSize (70);
			}
		}

		public static int LoginHorizontalPadding {
			get { 
				return GetRealSize (40);
			}
		}

		public static int LoginInputHeight {
			get { 
				return GetRealSize (60);
			}
		}

		public static int LoginInputPadding {
			get { 
				return GetRealSize (4);
			}
		}

		public static int LoginSeparatorSize {
			get {
				return GetRealSize (1);
			}
		}

		public static int UserNameLabelHeight {
			get {
				return GetRealSize (30);
			}
		}

		public static int UserNameLabelPadding {
			get {
				return GetRealSize (10);
			}
		}
	}
}

