using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;

namespace Need2Park
{
	class MyPlacesView : UIListView
	{
		MyPlacesListAdapter adapter;
		public MyPlacesView (Activity activity) : base (activity)
		{
			adapter = new MyPlacesListAdapter (activity);
			Adapter = adapter;


			var items = new List<ParkingLotInfo> ();

			items.Add (new ParkingLotInfo {
				Name = "test 1",
				Location = "Liivi 2 111",
				Description = "this is description asdasdasdasdasdasdasdads"
			});

			items.Add (new ParkingLotInfo {
				Name = "test 2 asdas dasdas dasd asd asd a",
				Location = "Krooks 12 3223",
				Description = "this is description asdasdasdasdasdasdasdads"
			});

			items.Add (new ParkingLotInfo {
				Name = "test1 aw dqwe q12e 1e 12e 12e 12e 12e 12e 12e 12e 12e 12e 12e 12e 12e 112e1 2e 3",
				Location = "asd asd asda sdasd asd asd as dasd as dasd as 12e 12e 12e 12e 12e 12e 12e 12e 12 e12 ed",
				Description = "this is description asdasdasdasdasdasdasdads"
			});

			items.Add (new ParkingLotInfo {
				Name = "test 3",
				Location = "asd asd asda sdasd asd asd as dasd as dasd asd",
				Description = "this is description asdasdasdasdasdasdasdads"
			});items.Add (new ParkingLotInfo {
				Name = "test 3",
				Location = "asd asd asda sdasd asd asd as dasd as dasd asd",
				Description = "this is description asdasdasdasdasdasdasdads"
			});items.Add (new ParkingLotInfo {
				Name = "test 3",
				Location = "asd asd asda sdasd asd asd as dasd as dasd asd"
			});



			adapter.AddItems (items);
		}
	}


}
