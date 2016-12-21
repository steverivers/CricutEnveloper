using System;
using System.Drawing;
using Svg;
using Svg.Pathing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Enveloper
{
    public struct Dimensions
    {
        public float length;
        public float b, c, d;
        public float r;
    }
    public class Envelope
    {
        public Envelope()
        {
            Length = 6;
            Height = 4;
            RoundTo = 0.125f;
            Offset = 0.18f;
            CutOffRadius = 0.05f;
        }

        public Envelope(float length, float height, float scaling)
        {
            Length = length;
            Height = height;
            RoundTo = 0.125f;
            Offset = 0.18f;
            Scale = scaling;
            CutOffRadius = 0.05f;
        }

        public Dimensions getDim()
        {
            var dim = new Dimensions();
            dim.b = roundToNearestFraction(Scale*(Length / 2) * (float)Math.Sqrt(2));
            dim.c = roundToNearestFraction(Scale * Offset * 2);
            dim.d = roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2));
            dim.length = dim.b + dim.c + dim.d;
            dim.r = CutOffRadius * Scale;
            return dim;
        }
        float Length { get; set; }
        float Height { get; set; }
        float Offset { get; set; }
        float RoundTo { get; set; }
        float Scale { get; set; }
        float CutOffRadius { get; set; }
        float roundToNearestFraction(float value, float fraction = 0.125f)
        {
            float intPart = (float)Math.Floor(value);
            float floatPart = value - intPart;
            if((floatPart % fraction != 0 ))
            {
                var t = (float)Math.Floor(floatPart / fraction);
                floatPart = (t + 1) * fraction;
            }
            return intPart + floatPart;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {

            SvgDocument FSvgDoc;
            const float scale = 100.0f;


            for (float x = 4.0f; x < 7; x += 0.25f)
            {
                for (float y = 4.0f; y < 7; y += 0.25f)
                {
                    var envelope = new Envelope(x, y, scale).getDim();
                    Console.WriteLine($"{x} x {y} ==> {envelope.length}, {envelope.b}, {envelope.d}");

                    FSvgDoc = new SvgDocument
                    {
                        Width = 1000,
                        Height = 1000
                    };
                    FSvgDoc.ViewBox = new SvgViewBox(-10, -10, 1000, 1000);

                    var outerGroup = new SvgGroup();
                    outerGroup.StrokeLineJoin = SvgStrokeLineJoin.Round;
                    FSvgDoc.Children.Add(outerGroup);
                    
                    #region left-mid to top left to top-mid
                    //left-mid to top left to top-mid
                    var outerPolyLineA = new SvgPolyline();
                    outerPolyLineA.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineA.Fill = new SvgColourServer(Color.White);

                    outerPolyLineA.Points = new SvgPointCollection();
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f + envelope.c/2 - envelope.r));
                    outerPolyLineA.Points.Add(new SvgUnit(envelope.length - envelope.d - envelope.c/2 - envelope.r));

                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b));

                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b));
                    outerPolyLineA.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineA.Points.Add(new SvgUnit(envelope.b + envelope.c/2 - envelope.r));
                    outerPolyLineA.Points.Add(new SvgUnit(envelope.c/2 - envelope.r));
                    outerGroup.Children.Add(outerPolyLineA);
                    #endregion

                    #region top-mid to top right to right-mid
                    //top-mid to top right to right-mid
                    var outerPolyLineB = new SvgPolyline();
                    outerPolyLineB.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineB.Fill = new SvgColourServer(Color.White);

                    outerPolyLineB.Points = new SvgPointCollection();
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length -envelope.d - envelope.c/2 + envelope.r));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.c / 2 - envelope.r));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length - envelope.d));
                    outerPolyLineB.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineB.Points.Add(new SvgUnit(0.0f));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.d));

                    outerPolyLineB.Points.Add(new SvgUnit(envelope.length - envelope.c/2 + envelope.r));
                    outerPolyLineB.Points.Add(new SvgUnit(envelope.d + envelope.c/2 - envelope.r));
                    outerGroup.Children.Add(outerPolyLineB);
                    #endregion

                    #region right-mid to bottom-right to bottom-mid
                    //right-mid to bottom-right to bottom-mid
                    var outerPolyLineC = new SvgPolyline();
                    outerPolyLineC.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineC.Fill = new SvgColourServer(Color.White);

                    outerPolyLineC.Points = new SvgPointCollection();
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + envelope.r));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b - envelope.c / 2 + envelope.r));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.b - envelope.c/2 + envelope.r));
                    outerPolyLineC.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + envelope.r));
                    outerGroup.Children.Add(outerPolyLineC);
                    #endregion

                    #region bottom-mid to bottom-left to left-mid
                    //bottom-mid to bottom-left to left-mid
                    var outerPolyLineD = new SvgPolyline();
                    outerPolyLineD.Stroke = new SvgColourServer(Color.Red);
                    outerPolyLineD.Fill = new SvgColourServer(Color.White);

                    outerPolyLineD.Points = new SvgPointCollection();
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.d + envelope.c/2 - envelope.r));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length - envelope.c / 2 + envelope.r));

                    outerPolyLineD.Points.Add(new SvgUnit(envelope.d));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineD.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length));

                    outerPolyLineD.Points.Add(new SvgUnit(0.0f));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length - envelope.d));

                    outerPolyLineD.Points.Add(new SvgUnit(envelope.c / 2 - envelope.r));
                    outerPolyLineD.Points.Add(new SvgUnit(envelope.length -envelope.d - envelope.c / 2 + envelope.r));
                    outerGroup.Children.Add(outerPolyLineD);
                    #endregion

                    #region top mid arc cut
                    var startPoint = new PointF(envelope.b + envelope.c / 2 - envelope.r, envelope.c / 2 - envelope.r);
                    var endPoint = new PointF(envelope.b + envelope.c / 2 + envelope.r, envelope.c / 2 - envelope.r);
                    var moveTo = new SvgMoveToSegment(startPoint);
                    var arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 180.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                    var arcPathData = new SvgPath();
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    #region right mid arc cut
                    startPoint = new PointF(envelope.length - envelope.c / 2 + envelope.r, envelope.d + envelope.c / 2 - envelope.r);
                    endPoint = new PointF(envelope.length - envelope.c / 2 + envelope.r, envelope.d + envelope.c / 2 + envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 180.0f, SvgArcSize.Small, SvgArcSweep.Negative, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    #region bottom mid arc cut
                    startPoint = new PointF(envelope.b + envelope.c / 2 - envelope.r, envelope.length - envelope.c / 2 + envelope.r);
                    endPoint = new PointF(envelope.b + envelope.c / 2 + envelope.r, envelope.length - envelope.c / 2 + envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 180.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
                    arcPathData = new SvgPath();
                    arcPathData.Stroke = new SvgColourServer(Color.Red);
                    arcPathData.Fill = new SvgColourServer(Color.White);
                    arcPathData.PathData = new SvgPathSegmentList();
                    arcPathData.PathData.Add(moveTo);
                    arcPathData.PathData.Add(arc);
                    outerGroup.Children.Add(arcPathData);
                    #endregion

                    #region left mid arc cut
                    startPoint = new PointF(envelope.c / 2 - envelope.r, envelope.b + envelope.c / 2 - envelope.r);
                    endPoint = new PointF(envelope.c / 2 - envelope.r, envelope.b + envelope.c / 2 + envelope.r);
                    moveTo = new SvgMoveToSegment(startPoint);
                    arc = new SvgArcSegment(startPoint, envelope.r, envelope.r, 180.0f, SvgArcSize.Small, SvgArcSweep.Positive, endPoint);
                    arcPathData = new SvgPath();
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

                    var folderPath = @"c:\users\srivers\envelopes\";
                    var fileName = $"{x:.00}_x_{y:.00}.svg";
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
