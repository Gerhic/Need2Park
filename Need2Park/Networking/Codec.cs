using System;
using System.Threading.Tasks;
using System.Json;
using System.Collections.Generic;

namespace Need2Park
{
	static class Codec
	{
		const string EMAIL = "email";
		const string PASSWORD = "password";

		public const string STATUS = "status";
		public const string EXCEPTION = "exception";

		public const string PARKINGLOT = "parkingLot";

		public static JsonValue EncodeLoginRequest (LoginRequest loginRequest)
		{
			return new JsonObject {
				{ EMAIL, loginRequest.Email },
				{ PASSWORD, loginRequest.Password }
			};
		}

		public static LoginResponse DecodeLoginResponse (JsonValue json)
		{
			LoginResponse response = new LoginResponse ();

			if (json == null) {
				response.StatusCode = Response.UnknownError;
				return response;
			}

			if (json.ContainsKey (STATUS)) {
				response.StatusCode = json [STATUS];
			} else if (json.ContainsKey (EXCEPTION)) {
				response.StatusCode = json [EXCEPTION];
			}

			if (json.ContainsKey (UserInfo.SESSIONID)) {
				response.SessionId = json [UserInfo.SESSIONID];
			}

			if (json.ContainsKey (UserInfo.NAME)) {
				response.Name = json [UserInfo.SESSIONID];
			}

			if (json.ContainsKey (EMAIL)) {
				response.email = json [EMAIL];
			}

//			if (!response.IsOk) {
//				if (json.ContainsKey (MESSAGE)) {
//					response.Message = json [MESSAGE];
//				} else if (json.ContainsKey ("msg")) {
//					response.Message = json ["msg"];
//				}
//			}

			return response;
		}

		public static List<ParkingLotInfo> DecodeParkingLotsRequest (JsonValue json)
		{
			List<ParkingLotInfo> parkingLotInfoList = new List<ParkingLotInfo> ();
			var jsonarray = json as JsonArray;

			if (jsonarray != null) {
				foreach (var jsonObject in jsonarray) {
					if (jsonObject is JsonPrimitive) {
						continue;
					}
					try {
						ParkingLotInfo parkingLotInfo = new ParkingLotInfo ();

						if (jsonObject.ContainsKey (ParkingLotInfo.RATE)) {
							parkingLotInfo.BaseRatePerMinute = jsonObject [ParkingLotInfo.RATE];
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.LOCATION)) {
							parkingLotInfo.Location = jsonObject [ParkingLotInfo.LOCATION];
						} else {
							continue;
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.NAME)) {
							parkingLotInfo.Name = jsonObject [ParkingLotInfo.NAME];
						} else {
							continue;
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.OCCUPIED)) {
							parkingLotInfo.Occupied = jsonObject [ParkingLotInfo.OCCUPIED];
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.PROVIDER)) {
							parkingLotInfo.Provider = jsonObject [ParkingLotInfo.PROVIDER];
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.PUBLICID)) {
							parkingLotInfo.PublicId = jsonObject [ParkingLotInfo.PUBLICID];
						}

						if (jsonObject.ContainsKey (ParkingLotInfo.SPOTS)) {
							parkingLotInfo.Spots = jsonObject [ParkingLotInfo.SPOTS];
						}

						parkingLotInfoList.Add (parkingLotInfo);
					} catch {
						Console.WriteLine ("# Failed to parse");
					}
				}
			}

			return parkingLotInfoList;
		}

		public static JsonValue EncodeParkingRequest (string publicId)
		{
			return new JsonObject {
				{ PARKINGLOT, publicId }
			};
		}
	}

}

