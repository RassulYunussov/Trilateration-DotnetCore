using System;
using System.Collections.Generic;
using System.Linq;
using Trilateration;
namespace ConsoleApplication
{
    public class Program
    {
		public static void GetDistances()
		{
			 Point beacon1 = new Point(10.0, 10.0, 10.0);
			 Point beacon2 = new Point(0.0, 0.0, 0.0);
			 Point beacon3 = new Point(10.0, 0.0, 0.0);
			 Point beacon4 = new Point(0.0, 10.0, 0.0);
 			 List<Point> points = new List<Point>();
			 points.Add(beacon1);
			 points.Add(beacon2);
			 points.Add(beacon3);
			 points.Add(beacon4);
             Point searchPoint = new Point(5.0, 5.0, 5.0);

			 TrilaterationFunction tf = new TrilaterationFunction();
			 double[] d = tf.CalculateDistances(points,searchPoint);
			 
			 System.Console.WriteLine($"{d[0]} {d[1]} {d[2]} {d[3]}");
		}
        public static void Main(string[] args)
        {
			GetDistances();
			/*
            Point beacon1 = new Point(1.0, 1.0, 1.0);
			Point beacon2 = new Point(3.0, 1.0, 2.0);
			Point beacon3 = new Point(2.0, 2.0, 1.0);
			Point beacon4 = new Point(4.0, 3.0, 2.0);
			double [] distances = new double[]{1.0,1.0,2.0,3.0};

		    List<Point> points = new List<Point>();
			points.Add(beacon1);
			points.Add(beacon2);
			points.Add(beacon3);
			points.Add(beacon4);
			TrilaterationFunction tf = new TrilaterationFunction();
			System.Console.WriteLine($"Distances: {distances[0]} {distances[1]} {distances[2]} {distances[3]}");
				
			Point p = tf.CalculatePosition(points,distances);
			double [] calculatedDistances = tf.CalculateDistances(points,p);
			double [] residuals = tf.CalculateResiduals(distances,calculatedDistances);
			double [] fixedDistances = tf.GetFixedDistances(distances,calculatedDistances);
			for(int i = 0;i<10;i++)
			{
				System.Console.WriteLine($"Point: {p.X} {p.Y} {p.Z}");
				System.Console.WriteLine($"Calculated Distances: {calculatedDistances[0]} {calculatedDistances[1]} {calculatedDistances[2]} {calculatedDistances[3]}");
				System.Console.WriteLine($"Summary Residual {residuals.Select(r=>Math.Abs(r)).Sum()}");
				System.Console.WriteLine($"Fixed Distances: {fixedDistances[0]} {fixedDistances[1]} {fixedDistances[2]} {fixedDistances[3]}");
				System.Console.WriteLine();
				calculatedDistances = tf.CalculateDistances(points,p);
				residuals = tf.CalculateResiduals(distances,calculatedDistances);
				fixedDistances = tf.GetFixedDistances(distances,calculatedDistances);
				p = tf.CalculatePosition(points,fixedDistances);
			}
*/
			
        }
    }
}
