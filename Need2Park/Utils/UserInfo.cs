using System;
using System.Collections.Generic;

namespace Need2Park
{
	public class UserInfo
	{
		public const string SESSIONID = "sessionId";
		public const string NAME = "name";
		public const string EMAIL = "email";

		public string SessionId { get; set; }
		public string Name { get; set; }

		public string Email { get; set; }
		public string Phone { get; set; }
		public string ImageUrl { get; set; }
		public List<ParkingLotInfo> AccessInfo { get; set; }

		public ParkingLotInfo ParkingLotInUse { get; set; }

		public UserInfo ()
		{
			AccessInfo = new List<ParkingLotInfo> ();
		}
	}

}

