using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomInteriorGeneratorTag
{
    public struct Pair<T, U>
    {
        public T First { get; set; }
        public U Second { get; set; }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public override string ToString()
        {
            return $"({First}, {Second})";
        }
    };

    public struct Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    };

    public struct PointDouble
    {
        public double x { get; set; }
        public double y { get; set; }

        public PointDouble(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    };
}
