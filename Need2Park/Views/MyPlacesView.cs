using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;
using Android.Graphics.Drawables;

namespace Need2Park
{
	class MyPlacesView : UIView
	{
		MyPlacesListAdapter adapter;

		UIListView listView;
		UIView container;

		public MyPlacesView (Activity activity) : base (activity)
		{
			listView = new UIListView (activity);
			listView.LayoutParameters = new ViewGroup.LayoutParams (
				ViewGroup.LayoutParams.MatchParent,
				ViewGroup.LayoutParams.MatchParent
			);

			listView.Divider = new ColorDrawable (CustomColors.DarkColor);
			listView.DividerHeight = Sizes.LoginSeparatorSize;

			adapter = new MyPlacesListAdapter (activity);
			listView.Adapter = adapter;

			container = new UIView (activity);
			container.AddView (listView);
			AddView (container);
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

		public override void LayoutSubviews ()
		{
			container.Frame = new Frame (
				Sizes.ListViewPadding,
				Sizes.ListViewPadding,
				Frame.W - 2 * Sizes.ListViewPadding,
				Frame.H - 2 * Sizes.ListViewPadding
			);
		}
	}
}
