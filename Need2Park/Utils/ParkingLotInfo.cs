using System;
using System.Collections.Generic;

namespace Need2Park
{
	public class ParkingLotInfo
	{
		public const string RATE = "baseRatePerMinute";
		public const string LOCATION = "location";
		public const string NAME = "name";
		public const string OCCUPIED = "occupied";
		public const string PROVIDER = "provider";
		public const string PUBLICID = "publicId";
		public const string SPOTS = "spots";

		public int BaseRatePerMinute { get; set; }
		public string Location { get; set; }
		public string Name { get; set; }
		public int Occupied { get; set; }
		public string Provider { get; set; }
		public string PublicId { get; set; }
		public int Spots { get; set; }

		public ParkingLotInfo ()
		{
		}
	}
}

