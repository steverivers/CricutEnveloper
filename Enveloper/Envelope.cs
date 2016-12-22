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

        List<SortedList<int, PointF>> segments;

        List<SortedList<int, PointF>> calcSegments()
        {
            var result = new List<SortedList<int, PointF>>();
            var segment = new SortedList<int, PointF>();
            //segments
            // topLeft, topRight, bottomRight and bottomLeft
            // Each segment has 5 points

            // topLeft
            segment.Add(0,new PointF(0.0f + dim.c / 2 - 0.3f * dim.r, dim.length - dim.d - dim.c / 2 - 0.7f * dim.r));
            segment.Add(1,new PointF(0.0f,dim.b));
            segment.Add(2, new PointF(0.0f, 0.0f));
            segment.Add(3, new PointF(dim.b - 0.7f * dim.r,0.0f));
            segment.Add(4, new PointF(dim.b + dim.c / 2 - 0.7f * dim.r,dim.c / 2 - 0.3f * dim.r));
            result.Add(segment);

            //topRight
            segment = new SortedList<int, PointF>();
            segment.Add(0, new PointF(dim.length - dim.d - dim.c / 2 + 0.7f * dim.r, dim.c / 2 - 0.3f * dim.r));
            segment.Add(1, new PointF(dim.length - dim.d + 0.7f * dim.r, 0.0f));
            segment.Add(2, new PointF(dim.length, 0.0f));
            segment.Add(3, new PointF(dim.length, dim.d - 0.7f * dim.r));
            segment.Add(4, new PointF(dim.length - dim.c / 2 + 0.3f * dim.r, dim.d + dim.c / 2 - 0.7f * dim.r));

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
