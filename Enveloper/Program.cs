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
            const float scale = 1.0f;
            const float strokeWidth = 0.0001f;

            for (float x = 4.0f; x < 7; x += 0.25f)
            {
                for (float y = 4.0f; y < 7; y += 0.25f)
                {
                    var oldEnvelope = new Envelope(x, y, scale).getDim();
                    Console.WriteLine($"{x} x {y} ==> {oldEnvelope.length}, {oldEnvelope.b}, {oldEnvelope.d}");

                    FSvgDoc = new SvgDocument
                    {
                        Width = new SvgUnit(SvgUnitType.Inch, 12),
                        Height = new SvgUnit(SvgUnitType.Inch, 12)
                    };
                    FSvgDoc.ViewBox = new SvgViewBox(-1, -1, 13, 13);
                    var outerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(outerGroup);

                    var envelope = new Envelope(x, y);
                    foreach(var segment in envelope.LinearSegments.Values)
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
                        var endPoint = new PointF(segment[0].X, segment[0].Y);
                        var moveTo = new SvgMoveToSegment(startPoint);
                        var arc = new SvgArcSegment(startPoint, envelope.CutOffRadius, envelope.CutOffRadius*envelope.Scale, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                        var arcPathData = new SvgPath();
                        arcPathData.Stroke = new SvgColourServer(Color.Red);
                        arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                        arcPathData.Fill = new SvgColourServer(Color.White);
                        arcPathData.PathData = new SvgPathSegmentList();
                        arcPathData.PathData.Add(moveTo);
                        arcPathData.PathData.Add(arc);
                        outerGroup.Children.Add(arcPathData);
                    }

                    #region top mid arc cut
                    /*var startPoint = new PointF(oldEnvelope.b + oldEnvelope.c / 2 - (0.7f * oldEnvelope.r), oldEnvelope.c / 2 - (0.3f * oldEnvelope.r));
                    var endPoint = new PointF(oldEnvelope.b + oldEnvelope.c / 2 + (0.7f*oldEnvelope.r), oldEnvelope.c / 2 - (0.3f *oldEnvelope.r));
                    var moveTo = new SvgMoveToSegment(startPoint);
                    var arc = new SvgArcSegment(startPoint, oldEnvelope.r, oldEnvelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                    var arcPathData = new SvgPath();
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion 

                    #region right mid arc cut
                    startPoint = new PointF(oldEnvelope.length - oldEnvelope.c / 2 + 0.3f*oldEnvelope.r, oldEnvelope.d + oldEnvelope.c / 2 - 0.7f*oldEnvelope.r);
                    endPoint = new PointF(oldEnvelope.length - oldEnvelope.c / 2 + 0.3f*oldEnvelope.r, oldEnvelope.d + oldEnvelope.c / 2 + 0.7f*oldEnvelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, oldEnvelope.r, oldEnvelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    #region bottom mid arc cut
                    startPoint = new PointF(oldEnvelope.d + oldEnvelope.c / 2 - 0.7f*oldEnvelope.r, oldEnvelope.length - oldEnvelope.c / 2 + 0.3f*oldEnvelope.r);
                    endPoint = new PointF(oldEnvelope.d + oldEnvelope.c / 2 + 0.7f*oldEnvelope.r, oldEnvelope.length - oldEnvelope.c / 2 + 0.3f*oldEnvelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, oldEnvelope.r, oldEnvelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    #region left mid arc cut
                    startPoint = new PointF(oldEnvelope.c / 2 - 0.3f*oldEnvelope.r, oldEnvelope.b + oldEnvelope.c / 2 - 0.7f*oldEnvelope.r);
                    endPoint = new PointF(oldEnvelope.c / 2 - 0.3f*oldEnvelope.r, oldEnvelope.b + oldEnvelope.c / 2 + 0.7f*oldEnvelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, oldEnvelope.r, oldEnvelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);*/
                    #endregion
    
                    var innerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(innerGroup);
                    var innerPoly = new SvgPolygon();
                    innerPoly.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    innerPoly.Stroke = new SvgColourServer(Color.Red);
                    innerPoly.Fill = new SvgColourServer(Color.Blue);
                    innerPoly.FillOpacity = 0.4f;

                    innerPoly.Points = new SvgPointCollection();
                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.b + oldEnvelope.c/2));
                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.length - oldEnvelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.d + oldEnvelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.d + oldEnvelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.length - oldEnvelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(oldEnvelope.b + oldEnvelope.c/2));

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
