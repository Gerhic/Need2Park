using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Gms.Maps;
using System.Threading.Tasks;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Graphics.Drawables;

namespace Need2Park
{
	public class LocationView : UIView
	{
		MapFragment mapFragment;

		CameraUpdate cameraUpdate;

		MainActivity activity;

		public LocationView (MainActivity activity) : base (activity)
		{
			this.activity = activity;
			MapsInitializer.Initialize (activity);

			GoogleMapOptions mapOptions = new GoogleMapOptions()
				.InvokeMapType(GoogleMap.MapTypeNormal)
				.InvokeZoomControlsEnabled(false)
				.InvokeCompassEnabled(true);
			


			mapFragment = MapFragment.NewInstance ();

			mapFragment = MapFragment.NewInstance(mapOptions);

			var ft = activity.FragmentManager.BeginTransaction ();
			Id = View.GenerateViewId();
			ft.Add (Id, mapFragment);
			ft.Commit ();

			DoMapStuff ();
		}

		public override void LayoutSubviews ()
		{
//			mapImage.Frame = Frame.Bounds;
		}

		protected int animationDurationMs = 300;
		bool stop;
		async void DoMapStuff ()
		{
			if (stop) {
				return;
			}

			GoogleMap map = mapFragment.Map;
			if (map != null) {
				LatLng lanLng;

				LocationManager locationManager = (LocationManager) activity.GetSystemService (Android.Content.Context.LocationService);
				Criteria criteria = new Criteria();

				Location location = locationManager.GetLastKnownLocation (locationManager.GetBestProvider (criteria, false));

				if (location != null) {
					lanLng = new LatLng (location.Latitude, location.Longitude);
				} else {
					lanLng = new LatLng (58.366473, 26.690286);
				}

				CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
				builder.Target(lanLng);
				builder.Zoom(17);
				builder.Bearing(0);
				builder.Tilt(0);
				CameraPosition cameraPosition = builder.Build();
				cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
				map.MoveCamera(cameraUpdate);
				map.MyLocationEnabled = true;
				stop = true;
				AddMarkers ();
			}

			await Task.Delay (animationDurationMs);
			DoMapStuff ();
		}

		public void AddMarkers ()
		{
			GoogleMap map = mapFragment.Map;
			if (map != null) {
				map.Clear ();
				if (LoginState.ActiveUser != null) {
					foreach (var item in LoginState.ActiveUser.AccessInfo) {
						var tokens = item.Location.Split (new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
						if (tokens.Length >= 2) {
							double lat = double.NaN;
							double.TryParse (tokens [0], out lat);
							double lng = double.NaN;
							double.TryParse (tokens [1], out lng);
							if (lat != double.NaN && lng != double.NaN) {
								LatLng location = new LatLng (lat, lng);
								var marker = new MarkerOptions ();
								marker.SetPosition (location);
								marker.SetTitle (item.Name);
								marker.SetSnippet ("Free slots: " + item.Spots);

								Bitmap b = BitmapFactory.DecodeResource (Resources, Resource.Drawable.map_tag);
								Bitmap bhalfsize = Bitmap.CreateScaledBitmap (b, b.Width / 3, b.Height / 3, false);

								marker.InvokeIcon (BitmapDescriptorFactory.FromBitmap (bhalfsize));
								marker.Flat (false);

								map.AddMarker (marker);
							}
						}
					}
				}
			}
		}
	}
}
