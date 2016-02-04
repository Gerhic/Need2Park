using System;
using System.Collections.Generic;

namespace Need2Park
{
	public class ParkingLotInfo
	{
		public List<string> ImageUrls { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string ContactInfo{ get; set; }
		public string Description { get; set; }
		public int FreeSpotsCount { get; set; }

		public ParkingLotInfo ()
		{
			ImageUrls = new List<string> ();
		}
	}
}

