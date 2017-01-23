using System;
using System.Drawing;
using Svg;
using Svg.Pathing;
using System.IO;

namespace Enveloper
{

 
    class Program
    {
        static void Main(string[] args)
        {

            SvgDocument FSvgDoc;
            const int maxPaperSizeInches = 12;
            const float strokeWidth = 0.0001f;

            for (float x = 4.0f; x < 7; x += 0.25f)
            {
                for (float y = 4.0f; y < 7; y += 0.25f)
                {
                    FSvgDoc = new SvgDocument
                    {
                        Width = new SvgUnit(SvgUnitType.Inch, maxPaperSizeInches),
                        Height = new SvgUnit(SvgUnitType.Inch, maxPaperSizeInches)
                    };
                    FSvgDoc.ViewBox = new SvgViewBox(-1, -1, maxPaperSizeInches + 1, maxPaperSizeInches + 1);
                    var outerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(outerGroup);

                    var envelope = new Envelope(x, y);
                    //Console.WriteLine($"{x} x {y} ==> {envelope.Length}, {envelope.Height}, {envelope.Length}");
                    foreach (var segment in envelope.LinearSegments.Values)
                    {
                        var polyLine = new SvgPolyline();
                        polyLine.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                        polyLine.Stroke = new SvgColourServer(Color.Red);
                        polyLine.Fill = new SvgColourServer(Color.White);
                        polyLine.Points = new SvgPointCollection();
                        foreach (var point in segment)
                        {
                            polyLine.Points.Add(new SvgUnit(point.Value.X));
                            polyLine.Points.Add(new SvgUnit(point.Value.Y));
                        }
                        outerGroup.Children.Add(polyLine);
                    }

                    foreach (var segment in envelope.ArcSegments.Values)
                    {
                        var startPoint = new PointF(segment[0].X, segment[0].Y);
                        var endPoint = new PointF(segment[1].X, segment[1].Y);
                        var moveTo = new SvgMoveToSegment(startPoint);
                        var arc = new SvgArcSegment(startPoint, envelope.CutOffRadius, envelope.CutOffRadius, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                        var arcPathData = new SvgPath();
                        arcPathData.Stroke = new SvgColourServer(Color.Red);
                        arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                        arcPathData.Fill = new SvgColourServer(Color.White);
                        arcPathData.PathData = new SvgPathSegmentList();
                        arcPathData.PathData.Add(moveTo);
                        arcPathData.PathData.Add(arc);
                        outerGroup.Children.Add(arcPathData);
                    }

                    var innerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(innerGroup);
                    var innerPoly = new SvgPolygon();
                    innerPoly.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    innerPoly.Stroke = new SvgColourServer(Color.Red);
                    innerPoly.Fill = new SvgColourServer(Color.Blue);
                    innerPoly.FillOpacity = 0.4f;

                    innerPoly.Points = new SvgPointCollection();
                    for(var i = 0; i<4; ++i)
                    {
                        innerPoly.Points.Add(new SvgUnit(envelope.InnerScoreArea[i].X));
                        innerPoly.Points.Add(new SvgUnit(envelope.InnerScoreArea[i].Y));
                    }
                    innerGroup.Children.Add(innerPoly);

                    var currentUser = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var folderPath = $"{currentUser}\\envelopes\\";
                    var fileName = $"Envelope_{x:.00}_x_{y:.00}.svg";
                    var di = new DirectoryInfo(folderPath);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    FSvgDoc.Write(folderPath + fileName);
                }
            }
            Console.ReadKey();
        }
    }
}
