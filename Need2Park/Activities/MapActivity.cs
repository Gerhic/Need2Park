using System;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Preferences;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Gms.Maps;
using Android.Widget;
using Android.Views;
using Android.Runtime;

namespace Need2Park
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	class MapActivity : Activity, GoogleMap.IOnMapLoadedCallback//, IOnMapReadyCallback
	{
		UIView MapLayoutView; 

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			MapLayoutView = new UIView (this);

			MapLayoutView.Frame = new Frame (
				DeviceInfo.ScreenWidth,
				DeviceInfo.TrueScreenHeight
			);

			var mapFragment = MapFragment.NewInstance ();
			var ft = FragmentManager.BeginTransaction ();
			MapLayoutView.Id = View.GenerateViewId();
			ft.Add (MapLayoutView.Id, mapFragment);
			ft.Commit ();

//			MapLayoutView.AddView (
//				mapFragment
//			);

			SetContentView (MapLayoutView);

			SetUpMap ();
		}

		void SetUpMap ()
		{
//			if (mMap != null) {
//				FragmentManager.FindFragmentById<MapFragment> (Resource.Id.map).
//			}
		}

		public void OnMapLoaded ()
		{
			Console.WriteLine ("# OnMapLoaded");
		}
	}

}

