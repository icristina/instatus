using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    // http://blogs.msdn.com/b/jaybaz_ms/archive/2004/08/19/217226.aspx
    // http://blogs.msdn.com/b/jaybaz_ms/archive/2004/08/20/217789.aspx
    // http://www.codeproject.com/KB/cs/buildingrange.aspx
    public class Range<T> where T : IComparable<T>
    {
        public readonly T Start;
        public readonly T End;

        public Range(T start, T end)
        {
            // this.Start = start;
            // this.End = end;
            this.Start = Range.Min(start, end); // auto-correct start and end order
            this.End = Range.Max(start, end);
        }
    }

    public static class Range
    {
        public static bool Overlap<T>(Range<T> left, Range<T> right) where T : IComparable<T>
        {
            if (left.Start.CompareTo(left.Start) == 0)
            {
                return true;
            }
            else if (left.Start.CompareTo(right.Start) > 0)
            {
                return left.Start.CompareTo(right.End) <= 0;
            }
            else
            {
                return right.Start.CompareTo(left.End) <= 0;
            }
        }

        // http://stackoverflow.com/questions/1906525/c-generic-math-functions-min-max-etc
        public static T Max<T>(T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) > 0 ? left : right;
        }

        public static T Min<T>(T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) < 0 ? left : right;
        }

        public static Range<T> Intersection<T>(Range<T> left, Range<T> right) where T : IComparable<T> 
        {
            if (!Range.Overlap(left, right))
                return null;

            T start = Max(left.Start, right.Start);
            T end = Min(left.End, right.End);

            return new Range<T>(start, end);
        }
    }
}