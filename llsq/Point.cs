using System;
namespace Trilateration
{
	public class Point
	{
		public double X {get;private set;}
		public double Y {get;private set;}
		public double Z {get;private set;}

		public Point(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override bool Equals(object o)
		{
			Point other = o as Point;
			double distance = Math.Sqrt(Math.Pow(this.X-other.X,2)
										+Math.Pow(this.Y-other.Y,2)
										+Math.Pow(this.Z-other.Z,2));
			if(distance<0.01)
				return true;
			return false;
		}
		
	}
}

