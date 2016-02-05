using System;
using Android.Content;

namespace Need2Park
{
	public static class LoginState
	{
		public static UserInfo ActiveUser { get; set; }
		public static bool IsLoggedIn { get; set; }
	}
}

