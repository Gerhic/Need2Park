using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content.Res;

namespace Need2Park
{
	static class DeviceInfo
	{
		public static int ScreenWidth;
		public static int ScreenHeight;

		public static int NavigationBarHeight;

		public static int StatusBarHeight;

		public static int TrueScreenHeight;

		public static float Density;

		public static long Memory;

		public static long RuntimeMemory;

		public static Android.Graphics.Point RealSize;

		public static void Measure(Activity context)
		{
		Resources resources = context.Resources;

			ScreenWidth = resources.DisplayMetrics.WidthPixels;
			RealSize = new Android.Graphics.Point ();
			context.Window.WindowManager.DefaultDisplay.GetRealSize (RealSize);

			ScreenHeight = resources.DisplayMetrics.HeightPixels;

			int navBarId = resources.GetIdentifier("navigation_bar_height", "dimen", "android");
			int statusbarId = resources.GetIdentifier("status_bar_height", "dimen", "android");

			NavigationBarHeight = resources.GetDimensionPixelSize(navBarId);

			StatusBarHeight = resources.GetDimensionPixelSize(statusbarId);

			TrueScreenHeight = ScreenHeight - NavigationBarHeight - StatusBarHeight;

			Density = resources.DisplayMetrics.Density;

			// memory
			const string ActivityService = global::Android.Content.Context.ActivityService;
			ActivityManager manager = (ActivityManager)context.GetSystemService(ActivityService);
			ActivityManager.MemoryInfo outInfo = new ActivityManager.MemoryInfo();
			manager.GetMemoryInfo(outInfo);
			Memory = outInfo.TotalMem;

			RuntimeMemory = Java.Lang.Runtime.GetRuntime().MaxMemory();

		}
		
	}

}
