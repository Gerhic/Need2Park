using System;
using System.Collections.Generic;

namespace Need2Park
{
	public enum FontStyle
	{
		Serif,
		SerifLight,
		SerifThin,
		SerifCondensed
	}

	public class Font
	{
		public static int SmallestTitleSize = DeviceInfo.ScreenHeight / 47;

		public static int SmallTitleSize = DeviceInfo.ScreenHeight / 41;

		public static int TitleSize = DeviceInfo.ScreenHeight / 35;

		public static Dictionary<int, string> EnumStringValues = new Dictionary<int, string> {
			{ 0, "sans-serif" },
			{ 1, "sans-serif-light" },
			{ 2, "sans-serif-thin" },
			{ 3, "sans-serif-condensed" },
		};

		public static Font Get (FontStyle style, int size, bool bold = false)
		{
			return new Font () { Name = EnumStringValues[(int)style], Size = size, Bold = bold };
		}

		public string Name { get; set; }

		public int Size { get; set; }

		public bool Bold { get; set; }
	}
}

