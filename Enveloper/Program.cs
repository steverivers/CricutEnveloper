using System;
using System.Drawing;
using Svg;
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
    }
    public class Envelope
    {
        public Envelope()
        {
            Length = 6;
            Height = 4;
            RoundTo = 0.125f;
            Offset = 0.18f;
        }

        public Envelope(float length, float height, float scaling)
        {
            Length = length;
            Height = height;
            RoundTo = 0.125f;
            Offset = 0.18f;
            Scale = scaling;
        }

        public Dimensions getDim()
        {
            var dim = new Dimensions();
            dim.b = roundToNearestFraction(Scale*(Length / 2) * (float)Math.Sqrt(2));
            dim.c = roundToNearestFraction(Scale * Offset * 2);
            dim.d = roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2));
            dim.length = dim.b + dim.c + dim.d;
            return dim;
        }
        float Length { get; set; }
        float Height { get; set; }
        float Offset { get; set; }
        float RoundTo { get; set; }
        float Scale { get; set; }
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

                    var group = new SvgGroup();
                    FSvgDoc.Children.Add(group);

                    var outerPoly = new SvgPolygon();
                    outerPoly.Stroke = new SvgColourServer(Color.Red);
                    outerPoly.Fill = new SvgColourServer(Color.Black);
                    outerPoly.FillOpacity = 0.3f;

                    outerPoly.Points = new SvgPointCollection();
                    //top left to top right
                    outerPoly.Points.Add(new SvgUnit(0.0f));
                    outerPoly.Points.Add(new SvgUnit(0.0f));

                    outerPoly.Points.Add(new SvgUnit(envelope.b));
                    outerPoly.Points.Add(new SvgUnit(0.0f));

                    outerPoly.Points.Add(new SvgUnit(envelope.b + envelope.c/2));
                    outerPoly.Points.Add(new SvgUnit(envelope.c/2));

                    outerPoly.Points.Add(new SvgUnit(envelope.b + envelope.c));
                    outerPoly.Points.Add(new SvgUnit(0.0f));

                    outerPoly.Points.Add(new SvgUnit(envelope.length));
                    outerPoly.Points.Add(new SvgUnit(0.0f));

                    //top right to bottom right
                    outerPoly.Points.Add(new SvgUnit(envelope.length));
                    outerPoly.Points.Add(new SvgUnit(envelope.d));

                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.c/2));
                    outerPoly.Points.Add(new SvgUnit(envelope.d + envelope.c/2));

                    outerPoly.Points.Add(new SvgUnit(envelope.length));
                    outerPoly.Points.Add(new SvgUnit(envelope.d + envelope.c));

                    outerPoly.Points.Add(new SvgUnit(envelope.length));
                    outerPoly.Points.Add(new SvgUnit(envelope.length));

                    //bottom right to bottom left
                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.b));
                    outerPoly.Points.Add(new SvgUnit(envelope.length));

                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.b));
                    outerPoly.Points.Add(new SvgUnit(envelope.length));

                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.b - envelope.c/2));
                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.c/2));

                    outerPoly.Points.Add(new SvgUnit(envelope.d));
                    outerPoly.Points.Add(new SvgUnit(envelope.length));

                    outerPoly.Points.Add(new SvgUnit(0.0f));
                    outerPoly.Points.Add(new SvgUnit(envelope.length));

                    //bottom left to top left
                    outerPoly.Points.Add(new SvgUnit(0.0f));
                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.d));

                    outerPoly.Points.Add(new SvgUnit(0.0f + envelope.c/2));
                    outerPoly.Points.Add(new SvgUnit(envelope.length - envelope.d - envelope.c/2));

                    outerPoly.Points.Add(new SvgUnit(0.0f));
                    outerPoly.Points.Add(new SvgUnit(envelope.b));


                    group.Children.Add(outerPoly);

                    /*var innerRect = new SvgRectangle();
                    innerRect.Fill = new SvgColourServer(Color.Black);
                    innerRect.Height = new SvgUnit((float)(y*scale));
                    innerRect.Width = new SvgUnit((float)(x*scale));*/

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

                    group.Children.Add(innerPoly);

                    var folderPath = @"c:\users\srivers\envelopes\";
                    var fileName = $"{x}_x_{y}.svg";
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
