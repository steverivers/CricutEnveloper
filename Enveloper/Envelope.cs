using System;
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

        public Envelope(float length, float height, float scaling)
        {
            Length = length;
            Height = height;
            RoundTo = 0.125f;
            Offset = 0.18f;
            Scale = scaling;
            CutOffRadius = 0.05f;
        }

        //public Dimensions getDim()
        //{
        //    var dim = new Dimensions();
        //    dim.b = roundToNearestFraction(Scale * (Length / 2) * (float)Math.Sqrt(2));
        //    dim.c = roundToNearestFraction(Scale * Offset * 2);
        //    dim.d = roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2));
        //    dim.length = dim.b + dim.c + dim.d;
        //    dim.r = CutOffRadius * Scale;
        //    return dim;
        //}

        public Dimensions getDim(bool roundDimensions = false)
        {
            var dim = new Dimensions();
            dim.b = roundDimensions ? roundToNearestFraction(Scale * (Length / 2) * (float)Math.Sqrt(2)) : Scale * (Length / 2) * (float)Math.Sqrt(2);
            dim.c = roundDimensions ? roundToNearestFraction(Scale * Offset * 2) : Scale * Offset * 2;
            dim.d = roundDimensions ? roundToNearestFraction(Scale * (Height / 2) * (float)Math.Sqrt(2)) : Scale * (Height / 2) * (float)Math.Sqrt(2);
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
            if ((floatPart % fraction != 0))
            {
                var t = (float)Math.Floor(floatPart / fraction);
                floatPart = (t + 1) * fraction;
            }
            return intPart + floatPart;
        }

    }
}
