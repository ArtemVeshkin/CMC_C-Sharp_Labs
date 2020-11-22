using System;
using System.Numerics;

namespace Lab_2
{
    struct DataItem
    {
        public Vector2 Coord { get; set; }
        public double Value { get; set; }

        public DataItem(Vector2 coord, double value)
        {
            Coord = coord;
            Value = value;
        }

        public override string ToString() =>
            $"Coordinates: ({Coord.X}, {Coord.Y}), Value: {Value}";

        public string ToString(string format) =>
            $"Coordinates: ({Coord.X.ToString(format)}, {Coord.Y.ToString(format)}), Value: {Value.ToString(format)}";
    }

    struct Grid1D
    {
        public float Step { get; set; }
        public int NSteps { get; set; }

        public Grid1D(float step = 0, int nsteps = 0)
        {
            Step   = step;
            NSteps = nsteps;
        }

        public override string ToString() =>
            $"Step: {Step}, NSteps: {NSteps}";

        public string ToString(string format) =>
            $"Step: {Step.ToString(format)}, NSteps: {NSteps}";
    }

    abstract class V3Data
    {
        public string Info { get; set; }
        public DateTime Time { get; set; }

        public V3Data(string info, DateTime time)
        {
            Info = info;
            Time = time;
        }

        public abstract Vector2[] Nearest(Vector2 v);

        public abstract string ToLongString();

        public abstract string ToLongString(string format);

        public override string ToString() =>
            $"Info: {Info}, Time: {Time.ToShortTimeString()}";
    }
}
