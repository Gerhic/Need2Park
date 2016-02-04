using System;

namespace Need2Park
{
	public class Frame
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int W { get; set; }

		public int H { get; set; }

		public int Bottom {
			get {
				return Y + H;
			}
		}

		public int Right {
			get {
				return X + W;
			}
		}

		public Frame Bounds {
			get {
				return new Frame (W, H);		
			}
		}

		public bool IsEmpty {
			get {
				return W == 0 && H == 0;
			}
		}

		public Frame ()
		{
			X = 0;
			Y = 0;
			W = 0;
			H = 0;
		}

		public Frame (int x, int y, int w, int h)
		{
			X = x;

			Y = y;

			W = w;

			H = h;
		}

		public Frame (int w, int h)
		{
			W = w;

			H = h;
		}

		public override string ToString ()
		{
			return string.Format ("[Frame: X={0}, Y={1}, W={2}, H={3}]", X, Y, W, H);
		}

		public static Frame Empty {
			get {
				return new Frame ();
			}
		}

		public int CenterX {
			get { 
				return X + W / 2;
			}
		}

	}
}

