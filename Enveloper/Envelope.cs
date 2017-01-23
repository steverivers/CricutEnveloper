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
        public Envelope(float length, float height, float scaling = 1.0f)
        {
            Length = length;
            Height = height;
            RoundTo = 0.125f;
            Offset = 0.18f;
            CutOffRadius = 0.05f;
            calcDimensions();
            LinearSegments = calcLinearSegments();
            ArcSegments = calcArcSegments();
            InnerScoreArea = calcInnerScoreArea();
        }

        #region 
        public float Length { get; private set; }
        public float Height { get; private set; }
        public float Offset { get; private set; }
        public float RoundTo { get; private set; }
        public float CutOffRadius { get; private set; }
        public float PaperSize { get { return length; } }
        #endregion

        float length;
        float b, c, d;
        float r;

        void calcDimensions(bool roundDimensions = false)
        {
            b = roundDimensions ? roundToNearestFraction((Length / 2) * (float)Math.Sqrt(2)) : (Length / 2) * (float)Math.Sqrt(2);
            c = roundDimensions ? roundToNearestFraction(Offset * 2) :  Offset * 2;
            d = roundDimensions ? roundToNearestFraction((Height / 2) * (float)Math.Sqrt(2)) : (Height / 2) * (float)Math.Sqrt(2);
            length = b + c + d;
            r = CutOffRadius;
        }

        #region generate segments
        public Dictionary<string, SortedList<int, PointF>> LinearSegments { get; private set; }
        Dictionary<string, SortedList<int, PointF>> calcLinearSegments()
        {
            
            var result = new Dictionary<string,SortedList<int, PointF>>();
            var segment = new SortedList<int, PointF>();
            //segments
            // topLeft, topRight, bottomRight and bottomLeft
            // Each segment has 5 points

            // TopLeft
            segment.Add(0,new PointF(0.0f + c / 2 - 0.3f * r, length - d - c / 2 - 0.7f * r));
            segment.Add(1,new PointF(0.0f,b));
            segment.Add(2, new PointF(0.0f, 0.0f));
            segment.Add(3, new PointF(b - 0.7f * r,0.0f));
            segment.Add(4, new PointF(b + c / 2 - 0.7f * r,c / 2 - 0.3f * r));
            result.Add("TopLeft",segment);

            //TopRight
            segment = new SortedList<int, PointF>();
            segment.Add(0, new PointF(length - d - c / 2 + 0.7f * r, c / 2 - 0.3f * r));
            segment.Add(1, new PointF(length - d + 0.7f * r, 0.0f));
            segment.Add(2, new PointF(length, 0.0f));
            segment.Add(3, new PointF(length, d - 0.7f * r));
            segment.Add(4, new PointF(length - c / 2 + 0.3f * r, d + c / 2 - 0.7f * r));
            result.Add("TopRight",segment);

            //BottomRight
            segment = new SortedList<int, PointF>();
            segment.Add(0, new PointF(length - c / 2 + 0.3f * r, length - b - c/2 + 0.7f*r));
            segment.Add(1, new PointF(length, length - b + 0.7f*r));
            segment.Add(2, new PointF(length, length));
            segment.Add(3, new PointF(length - b + 0.7f*r, length));
            segment.Add(4, new PointF(length - b - c/2 + 0.7f*r, length - c/2 + 0.3f*r));
            result.Add("BottomRight",segment);

            //BottomLeft
            segment = new SortedList<int, PointF>();
            segment.Add(0, new PointF(d + c/2 - 0.7f*r,length - c/2 + 0.3f*r));
            segment.Add(1, new PointF(d - 0.7f*r,length));
            segment.Add(2, new PointF(0.0f, length));
            segment.Add(3, new PointF(0.0f, length - d + 0.7f*r));
            segment.Add(4, new PointF(c/2 - 0.3f*r,length - d - c/2 + 0.7f*r));
            result.Add("BottomLeft",segment);

            return result;
        }

        public Dictionary<string,SortedList<int, PointF>> ArcSegments { get; private set; }
        Dictionary<string, SortedList<int, PointF>> calcArcSegments()
        {
            var result = new Dictionary<string, SortedList<int, PointF>>();
            SortedList<int, PointF> segment;

            //top
            segment = new SortedList<int, PointF>();
            segment.Add(0, LinearSegments["TopLeft"][4]);
            segment.Add(1, LinearSegments["TopRight"][0]);
            result.Add("Top", segment);

            //right
            segment = new SortedList<int, PointF>();
            segment.Add(0, LinearSegments["TopRight"][4]);
            segment.Add(1, LinearSegments["BottomRight"][0]);
            result.Add("Right", segment);

            //bottom
            segment = new SortedList<int, PointF>();
            segment.Add(0, LinearSegments["BottomRight"][4]);
            segment.Add(1, LinearSegments["BottomLeft"][0]);
            result.Add("Bottom", segment);

            //bottom
            segment = new SortedList<int, PointF>();
            segment.Add(0, LinearSegments["BottomLeft"][4]);
            segment.Add(1, LinearSegments["TopLeft"][0]);
            result.Add("Left", segment);

            return result;
        }

        public SortedList<int,PointF> InnerScoreArea { get; private set; }
        SortedList<int, PointF> calcInnerScoreArea()
        {
            var scoreArea = new  SortedList<int, PointF>();

            scoreArea = new SortedList<int, PointF>();
            scoreArea.Add(0,new PointF(b + c / 2, c / 2));
            scoreArea.Add(1, new PointF(length - c / 2, d + c / 2));
            scoreArea.Add(2, new PointF(d + c / 2, length - c / 2));
            scoreArea.Add(3, new PointF(c / 2, b + c / 2));
            return scoreArea;
        }

        #endregion

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
