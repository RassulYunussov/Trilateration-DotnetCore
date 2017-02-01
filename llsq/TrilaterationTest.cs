using System;
using System.Collections.Generic;
using System.Linq;
using Trilateration;
using Xunit;

namespace llsq
{
    public class TrilaterationTest
    {
        [Fact]
        public void ExactPositionTestFullPoints()
        {
            Point beacon1 = new Point(10.0, 10.0, 10.0);
			Point beacon2 = new Point(0.0, 0.0, 0.0);
			Point beacon3 = new Point(10.0, 0.0, 0.0);
			Point beacon4 = new Point(0.0, 10.0, 0.0);

            Point searchPoint = new Point(5.0, 5.0, 5.0);

            List<Point> points = new List<Point>();
			points.Add(beacon1);
			points.Add(beacon2);
			points.Add(beacon3);
			points.Add(beacon4);

            TrilaterationFunction tf = new TrilaterationFunction();
            double [] distances =  tf.CalculateDistances(points,searchPoint);

            Point calculatedPoint = tf.CalculatePosition(points,distances);

            Assert.Equal<Point>(searchPoint,calculatedPoint);
        }
        
        [Theory]
        [InlineData(new double[]{8.6,8.6,8.6,8.6})]
        [InlineData(new double[]{8.0,8.6,8.6,8.6})]
        [InlineData(new double[]{5.6,8.6,8.6,8.6})]
        [InlineData(new double[]{8.6,7.6,8.6,8.6})]
        [InlineData(new double[]{8.6,18.6,8.6,8.6})]
        [InlineData(new double[]{8.6,8.6,8.6,18.6})]
        [InlineData(new double[]{8.6,8.6,10.6,5.6})]
        public void InexactDistances(double[] distances)
        {
            Point beacon1 = new Point(10.0, 10.0, 10.0);
			Point beacon2 = new Point(0.0, 0.0, 0.0);
			Point beacon3 = new Point(10.0, 0.0, 0.0);
			Point beacon4 = new Point(0.0, 10.0, 0.0);

            Point searchPoint = new Point(5.0, 5.0, 5.0);
            List<Point> points = new List<Point>();
			points.Add(beacon1);
			points.Add(beacon2);
			points.Add(beacon3);
			points.Add(beacon4);

            TrilaterationFunction tf = new TrilaterationFunction();
            Point calculatedPoint = tf.CalculatePosition(points,distances);
			double [] calculatedDistances = tf.CalculateDistances(points,calculatedPoint);
			double [] residuals = tf.CalculateResiduals(distances,calculatedDistances);
			double [] fixedDistances = tf.GetFixedDistances(distances,calculatedDistances);
            int maxIterations = 10;
            int i = 0;
            while(i++<maxIterations)
			{
				calculatedDistances = tf.CalculateDistances(points,calculatedPoint);
				residuals = tf.CalculateResiduals(distances,calculatedDistances);
				fixedDistances = tf.GetFixedDistances(distances,calculatedDistances);
				calculatedPoint = tf.CalculatePosition(points,fixedDistances);
			}

            Assert.Equal<Point>(searchPoint,calculatedPoint);
        }
    }
}