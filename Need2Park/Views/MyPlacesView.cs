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
		}

		public void UpdateList ()
		{
			var user = LoginState.ActiveUser;
			if (user != null) {
				adapter.UpdateItems (user.AccessInfo);
			} else {
				adapter.UpdateItems ();
			}
		}
	}
}
