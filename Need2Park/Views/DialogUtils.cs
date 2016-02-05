using System;
using Android.App;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	static class DialogUtils
	{
		public static void CreateDialog (Activity activity, string title, string message)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder (activity);
			builder.SetTitle (title);
			builder.SetMessage (message);
			builder.SetCancelable (true);
			builder.SetNegativeButton ("Ok",delegate {
				
			});
			builder.Create ().Show ();
		}
	}

}