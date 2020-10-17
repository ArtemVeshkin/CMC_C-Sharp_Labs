using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab_1
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
    }

    struct Grid1D
    {
        public float Step { get; set; }
        public int NSteps { get; set; }

        public Grid1D(float step, int nsteps)
        {
            Step   = step;
            NSteps = nsteps;
        }

        public override string ToString() =>
            $"Step: {Step}, NSteps: {NSteps}";
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

        public override string ToString() =>
            $"Info: {Info}, Time: {Time.ToShortTimeString()}";
    }
}
