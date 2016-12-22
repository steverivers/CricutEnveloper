using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enveloper
{
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

        public Envelope(float length, float height, float scaling = 1.0f)
        {
            Length = length;
            Height = height;
            RoundTo = 0.125f;
            Offset = 0.18f;
            Scale = scaling;
            CutOffRadius = 0.05f;
        }

        Dimensions dim;
        public Dimensions getDim(bool roundDimensions = false)
        {
            dim = new Dimensions();
            dim.b = roundDimensions ? roundToNearestFraction(Scale * (Length / 2) * (float)Math.Sqrt(2)) : Scale * (Length / 2) * (float)Math.Sqrt(2);
            dim.c = roundDimensions ? roundToNearestFraction(Scale * Offset * 2) : Scale * Offset * 2;
            dim.d = roundDimensions ? roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2)) : Scale * (Height / 2) * (float)Math.Sqrt(2);
            dim.length = dim.b + dim.c + dim.d;
            dim.r = CutOffRadius * Scale;
            return dim;
        }

        float length;
        float b, c, d;
        float r;
        void calcDimensions(bool roundDimensions = false)
        {
            b = roundDimensions ? roundToNearestFraction(Scale * (Length / 2) * (float)Math.Sqrt(2)) : Scale * (Length / 2) * (float)Math.Sqrt(2);
            c = roundDimensions ? roundToNearestFraction(Scale * Offset * 2) : Scale * Offset * 2;
            d = roundDimensions ? roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2)) : Scale * (Height / 2) * (float)Math.Sqrt(2);
            length = b + c + d;
            r = CutOffRadius * Scale;
        }

        List<SortedList<int, PointF>> segments;

        List<SortedList<int, PointF>> calcSegments()
        {
            var result = new List<SortedList<int, PointF>>();
            var segment = new SortedList<int, PointF>();
            //segments
            // topLeft, topRight, bottomRight and bottomLeft
            // Each segment has 5 points

            // topLeft
            segment.Add(0,new PointF(0.0f + c / 2 - 0.3f * r, length - d - c / 2 - 0.7f * r));
            segment.Add(1,new PointF(0.0f,b));
            segment.Add(2, new PointF(0.0f, 0.0f));
            segment.Add(3, new PointF(b - 0.7f * r,0.0f));
            segment.Add(4, new PointF(b + c / 2 - 0.7f * r,c / 2 - 0.3f * r));
            result.Add(segment);

            //topRight
            segment = new SortedList<int, PointF>();
            segment.Add(0, new PointF(length - d - c / 2 + 0.7f * r, c / 2 - 0.3f * r));
            segment.Add(1, new PointF(length - d + 0.7f * r, 0.0f));
            segment.Add(2, new PointF(length, 0.0f));
            segment.Add(3, new PointF(length, d - 0.7f * r));
            segment.Add(4, new PointF(length - c / 2 + 0.3f * r, d + c / 2 - 0.7f * r));

            return result;
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
            if ((floatPart % fraction != 0))
            {
                var t = (float)Math.Floor(floatPart / fraction);
                floatPart = (t + 1) * fraction;
            }
            return intPart + floatPart;
        }
        

    }
}
