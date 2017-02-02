using System;
using System.Collections.Generic;
namespace Trilateration
{
	public class TrilaterationFunction
	{
		public Point CalculatePosition(List<Point> points,double[] distances)
		{
			if (points.Count < 4||points.Count!=distances.Length)
			{
				throw new Exception("Number of points are insufficient for calculation or number of distances are not equal to number of points");
			}
			double[,] m;
			double[] b;
			double rx,ry,rz;
			
			getMatrixAndVector(points,distances, out m, out b);
			JacobiMethod(points, m, b, out rx, out ry, out rz); 
			return new Point(rx,ry,rz);
		}
		public double [] CalculateDistances(List<Point> points, Point p)
		{
			double[] calculatedDistances = new double[points.Count];
			for (int i = 0; i < points.Count; i++)
				calculatedDistances[i] = Math.Sqrt(Math.Pow(points[i].X - p.X, 2) + Math.Pow(points[i].Y- p.Y, 2) + Math.Pow(points[i].Z - p.Z, 2));
			return calculatedDistances;
		}
		public double [] CalculateResiduals(double [] distances,double [] newDistances)
		{
			double [] residuals = new double[distances.Length];
			for(int i = 0;i<distances.Length;i++)
				residuals[i] = Math.Max(distances[i],newDistances[i])-Math.Min(distances[i],newDistances[i]);
			return residuals;
		}
		public double[] GetFixedDistances(double [] distances,double [] calculatedDistances)
		{
			double[] fixedDistances = new double[distances.Length];
			for(int i = 0;i<distances.Length;i++)
				if(distances[i]>calculatedDistances[i])
				{
					fixedDistances[i] = distances[i]-(distances[i]-calculatedDistances[i])/100.0;
				}
			return fixedDistances;
		}
		private void getMatrixAndVector(List<Point> points,double[]distances, out double[,] m, out double[] b)
		{
			m = new double[points.Count-1,3];
			b = new double[points.Count-1];
			for (int i = 1; i < points.Count; i++)
			{
				m[i-1, 0] = points[i].X - points[0].X;
				m[i-1, 1] = points[i].Y - points[0].Y;
				m[i-1, 2] = points[i].Z - points[0].Z;
				b[i-1] = (Math.Pow(distances[0], 2) 
							- Math.Pow(distances[i], 2) 
							+ Math.Pow(points[i].X - points[0].X, 2) 
							+ Math.Pow(points[i].Y - points[0].Y, 2) 
							+ Math.Pow(points[i].Z - points[0].Z, 2)) / 2;
			}
		}
		private void JacobiMethod(List<Point> points, double[,] m, double[] b, out double x, out double y, out double z)
		{
			double[,] mt = new double[m.GetLength(1), m.GetLength(0)];
			double[,] multiplied = new double[mt.GetLength(0), m.GetLength(1)];
			mt = transpose(m);
			multiplied = multiply(mt, m);
			double det = determinant(multiplied);
			double[,] A = inverse(multiplied, det);
			double[,] d = multiply(A, mt);
			double[] q = multbyvector(d, b);
			x = q[0] + points[0].X;
			y = q[1] + points[0].Y;
			z = q[2] + points[0].Z;
		}
		private double[,] transpose(double[,] m)
		{
			double[,] mt = new double[m.GetLength(1), m.GetLength(0)];
			for (int row = 0; row < mt.GetLength(0); row++)
			{
				for (int col = 0; col < mt.GetLength(1); col++)
				{
					mt[row, col] = m[col, row];
				}
			}
			return mt;
		}
		private double[,] multiply(double[,] a, double[,] b)
		{
			double[,] multiplied = new double[a.GetLength(0), b.GetLength(1)];
			if (a.GetLength(0) == b.GetLength(1))
			{
				for (int i = 0; i < a.GetLength(0); i++)
					for (int j = 0; j < b.GetLength(1); j++)
					{
						double accumulator = 0;
						for (int k = 0; k < a.GetLength(1); k++)
							accumulator += a[i, k] * b[k, j];
						multiplied[i, j] = accumulator;
					}
			}
			return multiplied;
		}
		private double[] multbyvector(double[,] m, double[] v)
		{
			double[] r = new double[v.Length];
			for (int i = 0; i < m.GetLength(0); i++)
			{
				double accumulator = 0;
				for (int k = 0; k < m.GetLength(1); k++)
					accumulator += m[i, k] * v[k];
				r[i] = accumulator;
			}
			return r;
		}
		private double determinant(double[,] array)
		{
			double[,] res = new double[array.GetLength(0), array.GetLength(1)];
			for (int i = 0; i < array.GetLength(0); i++)
				for (int j = 0; j < array.GetLength(1); j++)
					res[i, j] = array[i, j];

			double det = 0;
			double total = 0;
			double[,] tempArr = new double[array.GetLength(0) - 1, array.GetLength(1) - 1];
			switch(array.GetLength(0))
			{
				case 1:
						det = res[0, 0];
						break;
			    case 2:
						det = res[0, 0] * res[1, 1] - res[0, 1] * res[1, 0];
						break;
				default:
						for (int i = 0; i < 1; i++)
							for (int j = 0; j < res.GetLength(1); j++)
							{
								if (j % 2 != 0) res[i, j] = res[i, j] * -1;
								tempArr = minor(res, i, j);
								det += determinant(tempArr)*res[i,j];
								total += det * res[i, j];
							}
						break;
			}
			return det;
		}
		private double[,] inverse(double[,] m, double d)
		{
			double[,] r = new double[m.GetLength(0), m.GetLength(1)];
			for (int i = 0; i < m.GetLength(0); i++)
			{
				for (int j = 0; j < m.GetLength(1); j++)
				{
					r[i, j] = (determinant(minor(m, i, j)) * Math.Pow(-1, i + j)) / d;
				}
			}
			return r;
		}
		private double[,] minor(double[,] m, int r, int c)
		{
			List<List<double>> arr = new List<List<double>>();
			double[,] array = new double[m.GetLength(0) - 1, m.GetLength(1) - 1];
			for (int i = 0; i < m.GetLength(0); i++)
			{
				List<double> row = new List<double>();
				for (int j = 0; j < m.GetLength(1); j++)
					row.Add(m[i, j]);
				arr.Add(row);
			}
			arr.RemoveAt(r);
			foreach (List<double> row in arr)
				row.RemoveAt(c);
			for (int j = 0; j < array.GetLength(1); j++)
			{
				int k = 0;
				foreach (List<double> row in arr)
				{
					
					array[k, j] = row[j];
					k++;
				}
			}
			return array;
		}
	}
}

