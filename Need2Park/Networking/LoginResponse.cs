using System;
using Android.App;
using Android.Views;
using Android.Graphics;

namespace Need2Park
{
	class LoginResponse
	{
		public string SessionId { get; set; }

		public string Name { get; set; }

		public string email { get; set; }

		public int StatusCode { get; set; }
	}

}