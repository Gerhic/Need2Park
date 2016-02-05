using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;

namespace Need2Park
{
	class MyPlacesListAdapter : BaseAdapter<ParkingLotInfo>
	{
		List<ParkingLotInfo> infoList;

		readonly Activity activity;

		public MyPlacesListAdapter (Activity activity)
		{
			this.activity = activity;
			infoList = new List<ParkingLotInfo> ();
		}

		#region implemented abstract members of BaseAdapter
		public override long GetItemId (int position)
		{
			return position;
		}
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ParkingLotCell cell = convertView as ParkingLotCell;
			if (cell == null) {
				cell = new ParkingLotCell (activity);
				cell.Click += (object sender, System.EventArgs e) => {
					if (cell.info != null) {
						PublicInfo.ActiveParkingLotInfo = cell.info;
						Intent parkingLotIntent = new Intent (activity, typeof(ParkingLotActivity));
						parkingLotIntent.AddFlags (ActivityFlags.ClearTop);
						activity.StartActivity (parkingLotIntent);
					}
				};
			}
			cell.UpdateInfo (infoList [position]);
			return cell;
		}
		public override int Count {
			get {
				return infoList.Count;
			}
		}
		public override ParkingLotInfo this [int index] {
			get {
				return infoList [index];
			}
		}
		#endregion

		public void AddMissingItems (List<ParkingLotInfo> items)
		{
			foreach (var item in items) {
				if (!infoList.Contains (item)) {
					infoList.Add (item);
				}
			}
			NotifyDataSetChanged ();
		}

		public void UpdateItems (List<ParkingLotInfo> accessInfo = null)
		{
			infoList.Clear ();
			if (accessInfo != null) {
				infoList.AddRange (accessInfo);
			}
			NotifyDataSetChanged ();
		}
	}
}