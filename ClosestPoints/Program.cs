using ClosestPoints.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*
 *                   | |
 *                   | |
 *                   | |
 *  _________________|_|_________________
 *  _________________|x|_________________
 *                   | |
 *                   | |
 *                   | |
 */

namespace ClosestPoints
{
    class Program
    {
        public enum Point_T
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        public class Point
        {
            private double _x;
            private double _y;
            private double _Distance;

            public double X
            {
                get
                {
                    return _x;
                }
                set
                {
                    //Console.Write("X value changed from " + _x);
                    _x = value;
                    //Console.WriteLine(" to " + _x);
                }
            }

            public double Y
            {
                get
                {
                    return _y;
                }
                set
                {
                    //Console.Write("Y value changed from " + _y);
                    _y = value;
                    //Console.WriteLine(" to " + _y);
                }
            }

            public double Distance
            {
                get
                {
                    return _Distance;
                }
                set
                {
                    _Distance = value;
                }
            }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        public class ContainerPoint
        {
            private Point _CornerPoint { get; set; }
            public Point CornerPoint
            {
                get
                {
                    return _CornerPoint;
                }
                set
                {
                    _CornerPoint = value;
                }
            }

            private Point_T _PointType { get; set; }
            public Point_T PointType
            {
                get
                {
                    return _PointType;
                }
                set
                {
                    _PointType = value;
                }
            }
        }

        static void Main(string[] args)
        {
            var SamplePoints = PlottedPoints;

            //Console.WriteLine("\nGenerated Points\n======================\n");

            //foreach(var point in list)
            //{
            //    //Console.WriteLine(string.Format("( {0}, {1} )", Math.Round(point.X, 3), Math.Round(point.Y, 3)));
            //    Console.WriteLine(string.Format("( {0}, {1} )", point.X.ToString("#.000"), point.Y.ToString("#.000")));

            //}

            SortedList<double, Point> ClosestPoints = new SortedList<double, Point>();

            Point Vertex = new Point(0, 0);
            int offset = 1;
            int NumberOfPoints = 100;
            int MaximumValues = SamplePoints.Count;

            while (ClosestPoints.Count < NumberOfPoints && ClosestPoints.Count != MaximumValues)
            {
                var FoundPoints = PointsWithinArea(SamplePoints, Vertex, offset);
                offset++;

                bool IsNeedClosesPoints = (FoundPoints.Count > NumberOfPoints);

                foreach (var point in FoundPoints)
                {
                    SamplePoints.Remove(point);

                    if(IsNeedClosesPoints)
                    {
                        point.Distance = Math.Sqrt(Math.Pow((point.X - Vertex.X), 2) + Math.Pow((point.Y - Vertex.Y), 2));

                        if (ClosestPoints.Count < NumberOfPoints)
                        {
                            ClosestPoints.Add(point.Distance, point);
                        }
                        else
                        {
                            // If point is less than any value in the current list of closest points, replace

                            if (ClosestPoints.GetValueOrDefault(ClosestPoints.Keys.Last()).Distance > point.Distance)
                            {
                                ClosestPoints.Remove(ClosestPoints.Keys.Last());
                                ClosestPoints.Add(point.Distance, point);
                            }
                        }
                    }
                    else
                    {
                        point.Distance = Math.Sqrt(Math.Pow((point.X - Vertex.X), 2) + Math.Pow((point.Y - Vertex.Y), 2));
                        ClosestPoints.Add(point.Distance, point);
                    }
                }

                if(ClosestPoints.Count == NumberOfPoints)
                {
                    break;
                }
            }

            Console.WriteLine("Closest points:\n");

            foreach(var point in ClosestPoints)
            {
                Console.WriteLine(string.Format("( {0}, {1} )", point.Value.X.ToString("#.000"), point.Value.Y.ToString("#.000")));
            }

            Console.WriteLine("\nFurthest points:\n");
            
            foreach (var point in SamplePoints)
            {
                Console.WriteLine(string.Format("( {0}, {1} )", point.X.ToString("#.000"), point.Y.ToString("#.000")));
            }
        }

        public static List<Point> PlottedPoints
        {
            get
            {
                int NumberOfPoints = int.Parse(Resources.NumberOfPoints);
                int MaximumCoordinate = int.Parse(Resources.MaximumCoordinate);

                List<Point> GeneratedPoints = new List<Point>();
                Random NumberGenerator = new Random();

                for (int i = 0; i < NumberOfPoints; i++)
                {
                    int XWholePart = NumberGenerator.Next(MaximumCoordinate * -1, MaximumCoordinate);
                    int YWholePart = NumberGenerator.Next(MaximumCoordinate * -1, MaximumCoordinate);
                    double RandomX = XWholePart + NumberGenerator.NextDouble();
                    double RandomY = YWholePart + NumberGenerator.NextDouble(); ;

                    GeneratedPoints.Add(new Point(RandomX, RandomY));
                }

                return GeneratedPoints;
            }
        }

        public static List<Point> PointsWithinArea(List<Point> CurrentPoints, Point Vertex, int Offset)
        {
            List<Point> PointsWithin = new List<Point>();

            foreach(Point point in CurrentPoints)
            {
                if(IsWithin(point, Vertex, Offset))
                {
                    PointsWithin.Add(point);
                }
            }

            return PointsWithin;
        }

        public static bool IsWithin(Point Coordinate, Point Vertex, int Offset)
        {
            double UpperXBar = Vertex.X + Offset;
            double LowerXBar = Vertex.X - Offset;

            double UpperYBar = Vertex.Y + Offset;
            double LowerYBar = Vertex.Y - Offset;

            if((Coordinate.X < UpperXBar && Coordinate.X > LowerXBar) && (Coordinate.Y < UpperYBar && Coordinate.Y > LowerYBar))
            {
                return true;
            }

            return false;
        }
    }
}
