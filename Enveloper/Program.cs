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
                    var envelope = new Envelope(x, y, scale).getDim();
                    Console.WriteLine($"{x} x {y} ==> {envelope.length}, {envelope.b}, {envelope.d}");

                    FSvgDoc = new SvgDocument
                    {
                        Width = new SvgUnit(SvgUnitType.Inch, 12),
                        Height = new SvgUnit(SvgUnitType.Inch, 12)
                    };
                    FSvgDoc.ViewBox = new SvgViewBox(-1, -1, 13, 13);
                    var outerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(outerGroup);
                    
                    #region left-mid to top left to top-mid
                    //left-mid to top left to top-mid
                    var outerPolyLineA = new SvgPolyline();
                    outerPolyLineA.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    outerPolyLineA.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineA.Fill = new SvgColourServer(Color.White);

                    outerPolyLineA.Points = new SvgPointCollection();
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f + envelope.c/2 - 0.3f*envelope.r));
                    outerPolyLineA.Points.Add(new SvgUnit(envelope.length - envelope.d - envelope.c/2 - 0.7f*envelope.r));

                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b));

                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b - 0.7f* envelope.r));
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b + envelope.c/2 - 0.7f*envelope.r));
                    outerPolyLineA.Points.Add(new SvgUnit((envelope.c/2) - 0.3f*envelope.r));
                    outerGroup.Children.Add(outerPolyLineA);
                    #endregion

                    #region top-mid to top right to right-mid
                    //top-mid to top right to right-mid
                    var outerPolyLineB = new SvgPolyline();
                    outerPolyLineB.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    outerPolyLineB.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineB.Fill = new SvgColourServer(Color.White);

                    outerPolyLineB.Points = new SvgPointCollection();
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length -envelope.d - envelope.c/2 + 0.7f*envelope.r));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.c / 2 - 0.3f*envelope.r));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length - envelope.d + 0.7f*envelope.r));
                    outerPolyLineB.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineB.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.d - 0.7f*envelope.r));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length - envelope.c/2 + 0.3f*envelope.r));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.d + envelope.c/2 - 0.7f*envelope.r));
                    outerGroup.Children.Add(outerPolyLineB);
                    #endregion

                    #region right-mid to bottom-right to bottom-mid
                    //right-mid to bottom-right to bottom-mid
                    var outerPolyLineC = new SvgPolyline();
                    outerPolyLineC.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    outerPolyLineC.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineC.Fill = new SvgColourServer(Color.White);

                    outerPolyLineC.Points = new SvgPointCollection();
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + 0.3f*envelope.r));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b - envelope.c / 2 + 0.7f*envelope.r));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b + 0.7f*envelope.r));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b + 0.7f*envelope.r));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b - envelope.c/2 + 0.7f*envelope.r));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + 0.3f*envelope.r));
                    outerGroup.Children.Add(outerPolyLineC);
                    #endregion

                    #region bottom-mid to bottom-left to left-mid
                    //bottom-mid to bottom-left to left-mid
                    var outerPolyLineD = new SvgPolyline();
                    outerPolyLineD.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    outerPolyLineD.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineD.Fill = new SvgColourServer(Color.White);

                    outerPolyLineD.Points = new SvgPointCollection();
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.d + envelope.c/2 - 0.7f*envelope.r));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + 0.3f*envelope.r));

                    outerPolyLineD.Points.Add(new SvgUnit(envelope.d - 0.7f*envelope.r));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineD.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineD.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length - envelope.d + 0.7f*envelope.r));

                    outerPolyLineD.Points.Add(new SvgUnit(envelope.c / 2 - 0.3f*envelope.r));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length -envelope.d - envelope.c / 2 + 0.7f*envelope.r));
                    outerGroup.Children.Add(outerPolyLineD);
                    #endregion

                    #region top mid arc cut
                    var startPoint = new PointF(envelope.b + envelope.c / 2 - (0.7f * envelope.r), envelope.c / 2 - (0.3f * envelope.r));
                    var endPoint = new PointF(envelope.b + envelope.c / 2 + (0.7f*envelope.r), envelope.c / 2 - (0.3f *envelope.r));
                    var moveTo = new SvgMoveToSegment(startPoint);
                    var arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
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
                    startPoint = new PointF(envelope.length - envelope.c / 2 + 0.3f*envelope.r, envelope.d + envelope.c / 2 - 0.7f*envelope.r);
                    endPoint = new PointF(envelope.length - envelope.c / 2 + 0.3f*envelope.r, envelope.d + envelope.c / 2 + 0.7f*envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
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
                    startPoint = new PointF(envelope.d + envelope.c / 2 - 0.7f*envelope.r, envelope.length - envelope.c / 2 + 0.3f*envelope.r);
                    endPoint = new PointF(envelope.d + envelope.c / 2 + 0.7f*envelope.r, envelope.length - envelope.c / 2 + 0.3f*envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
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
                    startPoint = new PointF(envelope.c / 2 - 0.3f*envelope.r, envelope.b + envelope.c / 2 - 0.7f*envelope.r);
                    endPoint = new PointF(envelope.c / 2 - 0.3f*envelope.r, envelope.b + envelope.c / 2 + 0.7f*envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 90.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    var innerGroup = new SvgGroup();
                    FSvgDoc.Children.Add(innerGroup);
                    var innerPoly = new SvgPolygon();
                    innerPoly.StrokeWidth = new SvgUnit(SvgUnitType.Inch, strokeWidth);
                    innerPoly.Stroke = new SvgColourServer(Color.Red);
                    innerPoly.Fill = new SvgColourServer(Color.Blue);
                    innerPoly.FillOpacity = 0.4f;

                    innerPoly.Points = new SvgPointCollection();
                    innerPoly.Points.Add(new SvgUnit(envelope.b + envelope.c/2));
                    innerPoly.Points.Add(new SvgUnit(envelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(envelope.length - envelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(envelope.d + envelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(envelope.d + envelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(envelope.length - envelope.c / 2));

                    innerPoly.Points.Add(new SvgUnit(envelope.c / 2));
                    innerPoly.Points.Add(new SvgUnit(envelope.b + envelope.c/2));

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
