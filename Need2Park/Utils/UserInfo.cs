using System;
using System.Collections.Generic;

namespace Need2Park
{
	public class UserInfo
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string ImageUrl { get; set; }
		public List<ParkingLotInfo> AccessInfo { get; set; }

		public UserInfo ()
		{
			AccessInfo = new List<ParkingLotInfo> ();
		}
	}

}

