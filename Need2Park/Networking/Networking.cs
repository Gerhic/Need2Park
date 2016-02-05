using System;
using System.Threading.Tasks;
using System.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Need2Park
{
	static class Networking
	{
		const string BASE = "https://api.need2park.com";

		const string LOGIN = "/api/driversession";
		const string PARKINGLOTS = "/api/driverparkinglots";
		const string STARTPARKING = "/api/registration";
		const string LOGOUT = "/api/driversession";

		const string sessionIdHeaderKey = "driverSessionId";


		const int TIMEOUT_IN_MILLISECONDS = (int)(5 * 1000);

		public async static Task<LoginResponse> SendLoginRequest (LoginRequest loginRequest)
		{
			JsonValue json = Codec.EncodeLoginRequest (loginRequest);

			json = await Post (BASE, LOGIN, json);

			return Codec.DecodeLoginResponse (json);
		}

		public async static Task<List<ParkingLotInfo>> SendParkingLotsRequest ()
		{
			if (LoginState.ActiveUser != null) {
			var dict = new Dictionary<string, string> ();

			dict.Add (sessionIdHeaderKey, LoginState.ActiveUser.SessionId);

			var json = await Get (BASE, PARKINGLOTS, dict);

			Console.WriteLine ("### " + json.ToString ());

			return Codec.DecodeParkingLotsRequest (json);
			} else {
				return null;
			}
		}

		static async Task<JsonValue> Get (string BASE, string API, Dictionary<string, string> headers = null) //Cookie cookie)
		{
			JsonObject result = new JsonObject ();
			Console.WriteLine("GET: " + BASE + API);

			try {
				Uri uri = new Uri (BASE + API);

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (uri);

				request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

				request.Method = "GET";

//				if (cookie != null) {
//					CookieContainer cookieContainer = new CookieContainer();
//					cookie.Domain = uri.Host;
//					cookieContainer.Add(cookie);
//					request.CookieContainer = cookieContainer;
//				}

				if (headers != null) {
					foreach (var kvPair in headers) {
						request.Headers.Set (kvPair.Key, kvPair.Value);
					}
				}

				HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync ();//.WithTimeout (TIMEOUT_IN_MILLISECONDS);

				string returnString;

				try {
					var stream = response.GetResponseStream ();
					returnString = StreamToString (stream);
				} catch (Exception e) {
					returnString = "Exception: " + e;
				}

				try {
					return JsonValue.Parse (returnString);
				} catch (Exception e) {
//					result [Codec.EXCEPTION] = Response.ErrorParsingResponse;
//					result [Codec.MESSAGE] = e.Message;
					Console.WriteLine ("# Get Exception: " + e);
					return result;
				}

			} catch (Exception e) {
				Console.WriteLine ("Get Exception2: " + e);
				return null;
			}
		}

		async static Task<JsonValue> Post (string baseUrl, string api, JsonValue json, Dictionary<string, string> headers = null)
		{
			return await SendData ("POST", baseUrl, api, json, headers);
		}

		async static Task<JsonValue> SendData (string method, string baseUrl, string api, JsonValue json, Dictionary<string, string> headers) //, Cookie cookie = null)
		{
			JsonObject result = new JsonObject ();
			Stream dataStream;
			HttpWebResponse response;

			byte[] byteArray = null;

			if (json != null) {
				byteArray = Encoding.UTF8.GetBytes (json.ToString ());
			} else {
				byteArray = new byte[0];
			}

			string url = baseUrl + api;
			Uri uri = new Uri (url);

			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (uri);

			if (json != null) {
				request.ContentType = "application/json";
			}

			request.Method = method;
			request.ContentLength = byteArray.Length;
			request.Timeout = TIMEOUT_IN_MILLISECONDS;

			if (request.Method == "POST") {
				request.AllowAutoRedirect = false;
				if (json != null) {
					request.Accept = "application/json";
				}
			}

			if (headers != null) {
				foreach (var kvPair in headers) {
					request.Headers.Set (kvPair.Key, kvPair.Value);
				}
			}

//			if (cookie == null) {
//				request.CookieContainer = new CookieContainer ();
//				request.CookieContainer.Add (uri, new CookieCollection ());
//			} else {
//				CookieContainer cookieContainer = new CookieContainer();
//				cookie.Domain = uri.Host;
//				cookieContainer.Add(cookie);
//				request.CookieContainer = cookieContainer;
//			}

			try {
				dataStream = await request.GetRequestStreamAsync ();//.WithTimeout(TIMEOUT_IN_MILLISECONDS);
			} catch (Exception e) {
//				result [Codec.EXCEPTION] = Response.ErrorCreatingStream;
//				if (IsNameResolutionFailure (e) || IsNetworkingUnreachable (e)) {
//					result [Codec.MESSAGE] = COULDNOTCONNECT + INTERNETAVAILABILITY;
//				} else {
//					result [Codec.MESSAGE] = e.Message;
//				}
				Console.WriteLine ("# Exception GetRequestStreamAsync: " + e);
				return result;
			}

			try {
				if (json != null) {
					await dataStream.WriteAsync (byteArray, 0, byteArray.Length);
				}
			} catch (Exception e) {
//				result [Codec.EXCEPTION] = Response.ErrorWritingStream;
//				result [Codec.MESSAGE] = e.Message;
				Console.WriteLine ("# Exception dataStream.WriteAsync: " + e);
				return result;
			}

			try {
				response = (HttpWebResponse)await request.GetResponseAsync ();//.WithTimeout (TIMEOUT_IN_MILLISECONDS);
			} catch (WebException e) {
				response = (HttpWebResponse)e.Response;
//				int statusCode = 32000;
//				string message = "";
//				if (response != null) {
//					statusCode = (int)response.StatusCode;
//					message = StreamToString (response.GetResponseStream ());
//				}
//				result [Codec.EXCEPTION] = statusCode;
//				result [Codec.MESSAGE] = message;
				Console.WriteLine ("# WebException request.GetResponseAsync: " + e);
				return result;
			} catch (Exception e) {
//				result [Codec.EXCEPTION] = Response.ErrorGettingResponse;
//				result [Codec.MESSAGE] = e.Message;
				Console.WriteLine ("# Exception request.GetResponseAsync: " + e);
				return result;
			}

			if (response == null) {
//				result [Codec.EXCEPTION] = Response.ErrorGettingResponse;
				Console.WriteLine ("# Exception response == null");

				return result;
			}

			if (response.StatusCode != HttpStatusCode.OK) {
//				result [Codec.EXCEPTION] = ((int)response.StatusCode).ToString ();
//				result [Codec.MESSAGE] = response.StatusDescription;

				Console.WriteLine ("# Exception response.StatusCode != HttpStatusCode.OK");

				return result;
			}

			var stream = response.GetResponseStream ();
			var returnValue = StreamToString (stream);

			try {
				JsonValue parsedJson = JsonObject.Parse (returnValue);

//				if (cookie == null) {
//					foreach (Cookie responseCookie in response.Cookies) {
//						parsedJson[responseCookie.Name] = responseCookie.Value;
//					}
//				}

				return parsedJson;
			} catch (Exception e) {
//				result [Codec.EXCEPTION] = Response.ErrorParsingResponse;
//				result [Codec.MESSAGE] = ERRORPARSINGRESPONSE;
				Console.WriteLine ("# Exception JsonObject.Parse: " + e);
				return result;
			}
		}

		public static string StreamToString (Stream stream)
		{
			using (StreamReader reader = new StreamReader (stream, Encoding.UTF8)) {
				return reader.ReadToEnd ();
			}
		}

		public async static void SendStartParkingRequest (string publicId)
		{
			if (LoginState.ActiveUser != null) {
				JsonValue json = Codec.EncodeParkingRequest (publicId);

				Dictionary<string, string> dict = new Dictionary<string, string> ();
				dict.Add (sessionIdHeaderKey, LoginState.ActiveUser.SessionId);

				json = await Post (BASE, STARTPARKING, json, dict);
			} else {
				// TODO handle no user
			}
		}

		public async static void SendStopParkingRequest (string publicId)
		{
			if (LoginState.ActiveUser != null) {
				Dictionary<string, string> dict = new Dictionary<string, string> ();
				dict.Add (sessionIdHeaderKey, LoginState.ActiveUser.SessionId);

				await Delete (BASE, STARTPARKING, dict);
			} else {
				// TODO handle no user
			}
		}

		static async Task<JsonValue> Delete (string BASE, string API, Dictionary<string, string> headers = null) //Cookie cookie)
		{
			JsonObject result = new JsonObject ();
			Console.WriteLine("DELETE: " + BASE + API);

			try {
				Uri uri = new Uri (BASE + API);

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (uri);

				request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

				request.Method = "DELETE";

				//				if (cookie != null) {
				//					CookieContainer cookieContainer = new CookieContainer();
				//					cookie.Domain = uri.Host;
				//					cookieContainer.Add(cookie);
				//					request.CookieContainer = cookieContainer;
				//				}

				if (headers != null) {
					foreach (var kvPair in headers) {
						request.Headers.Set (kvPair.Key, kvPair.Value);
					}
				}

				HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync ();//.WithTimeout (TIMEOUT_IN_MILLISECONDS);

				string returnString;

				try {
					var stream = response.GetResponseStream ();
					returnString = StreamToString (stream);
				} catch (Exception e) {
					returnString = "Exception: " + e;
				}

				try {
					return JsonValue.Parse (returnString);
				} catch (Exception e) {
					//					result [Codec.EXCEPTION] = Response.ErrorParsingResponse;
					//					result [Codec.MESSAGE] = e.Message;
					Console.WriteLine ("# Get Exception: " + e);
					return result;
				}

			} catch (Exception e) {
				Console.WriteLine ("Get Exception2: " + e);
				return null;
			}
		}

		public async static void SendLogoutRequest (UserInfo user)
		{
			Dictionary<string, string> dict = new Dictionary<string, string> ();
			dict.Add (sessionIdHeaderKey, user.SessionId);

			await Delete (BASE, LOGOUT, dict);

		}
	}
}

