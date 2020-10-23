using System;
using System.Collections.Generic;
using System.Numerics;

namespace Lab_1
{
    class V3DataOnGrid : V3Data
    {
        public Grid1D XGrid { get; set; }
        public Grid1D YGrid { get; set; }

        public double[,] Value { get; set; }

        public V3DataOnGrid(string info, DateTime time, Grid1D xgrid, Grid1D ygrid) : base(info, time)
        {
            XGrid = xgrid;
            YGrid = ygrid;

            Value = new double[xgrid.NSteps, ygrid.NSteps];
        }

        public void InitRandom(double minValue, double maxValue)
        {
            Random rnd = new Random();
            for (int i = 0; i < XGrid.NSteps; ++i)
            {
                for (int j = 0; j < YGrid.NSteps; ++j)
                {
                    Value[i, j] = rnd.NextDouble() * (maxValue - minValue) + minValue;
                }
            }
        }

        public static explicit operator V3DataCollection(V3DataOnGrid v3dataongrid)
        {
            V3DataCollection v3datacollection = new V3DataCollection(v3dataongrid.Info, v3dataongrid.Time);

            List<DataItem> Grid = new List<DataItem>();
            for (int i = 0; i < v3dataongrid.XGrid.NSteps; ++i)
            {
                for (int j = 0; j < v3dataongrid.YGrid.NSteps; ++j)
                {
                    Vector2 coord = new Vector2(i * v3dataongrid.XGrid.Step, j * v3dataongrid.YGrid.Step);
                    Grid.Add(new DataItem(coord, v3dataongrid.Value[i, j]));
                }
            }

            v3datacollection.Grid = Grid;

            return v3datacollection;
        }

        public override Vector2[] Nearest(Vector2 v)
        {
            Vector2[] curNearest = new Vector2[4];
            int curNearestSize = 1;
            double minDist = Math.Pow(v.X, 2) + Math.Pow(v.Y, 2);

            curNearest[0] = new Vector2(0, 0);

            for (int i = 0; i < XGrid.NSteps; ++i)
            {
                for (int j = 0; j < YGrid.NSteps; ++j)
                {
                    double curDist = Math.Pow(v.X - i * XGrid.Step, 2) + Math.Pow(v.Y - j * YGrid.Step, 2);

                    if (curDist < minDist)
                    {
                        minDist = curDist;
                        curNearestSize = 1;
                        curNearest[0] = new Vector2(i * XGrid.Step, j * YGrid.Step);
                    }
                    else if (curDist == minDist)
                    {
                        curNearest[curNearestSize] = new Vector2(i * XGrid.Step, j * YGrid.Step);
                        ++curNearestSize;
                    }
                }
            }

            Vector2[] result = new Vector2[curNearestSize];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = curNearest[i];
            }

            return result;
        }

        public override string ToString() =>
            $"Class: {this.GetType()}, {base.ToString()}, X: {XGrid.ToString()}, Y: {YGrid.ToString()}";

        public override string ToLongString()
        {
            string res = this.ToString() + "\n";

            for (int i = 0; i < XGrid.NSteps; ++i)
            {
                for (int j = 0; j < YGrid.NSteps; ++j)
                {
                    res += $"({i * XGrid.Step}, {j * YGrid.Step}) : {Value[i, j]}\n";
                }
            }

            return res;
        }
    }
}
