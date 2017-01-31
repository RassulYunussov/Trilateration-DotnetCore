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
		
	}
}

