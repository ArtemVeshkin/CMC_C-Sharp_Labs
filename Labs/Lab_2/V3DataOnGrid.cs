using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.IO;

namespace Lab_2
{
    class V3DataOnGridEnumerator : IEnumerator<DataItem>
    {
        private DataItem[,] Value;
        private int Pos1, Pos2;

        public V3DataOnGridEnumerator(Grid1D xgrid, Grid1D ygrid, double[,] value)
        {
            Pos1 =  0;
            Pos2 = -1;

            Value = new DataItem[xgrid.NSteps, ygrid.NSteps];
            for (int i = 0; i < xgrid.NSteps; ++i)
            {
                for (int j = 0; j < ygrid.NSteps; ++j)
                {
                    Value[i, j] = new DataItem(new Vector2(i * xgrid.Step, j * ygrid.Step), value[i, j]);
                }
            }
        }

        public DataItem Current
        {
            get
            {
                if (0 >= Pos1 && Pos1 <= Value.GetLength(0) && 0 >= Pos2 && Pos2 <= Value.GetLength(1))
                {
                    return Value[Pos1, Pos2];
                }

                throw new InvalidOperationException();
            }
        }

        public bool MoveNext()
        {
            if (Pos2 < Value.GetLength(1) - 1)
            {
                ++Pos2;
                return true;
            }
            else if (Pos1 < Value.GetLength(0) - 1 && Pos2 == Value.GetLength(1) - 1)
            {
                ++Pos1;
                Pos2 = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            Pos1 =  0;
            Pos2 = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        void IDisposable.Dispose() { }
    }


    class V3DataOnGrid : V3Data, IEnumerable<DataItem>
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

        public V3DataOnGrid(string filename) : base("", new DateTime())
        {
            // .: FORMAT :.
            // {string   Info}\n
            // {DateTime Time}\n
            // {float StepX}\n
            // {int NStepsX}\n
            // {float StepY}\n
            // {int NStepsY}\n
            // {double Value[0, 0]}\n
            // {double Value[0, 1]}\n
            // ...
            // {double Value[0, NStepsY]}\n
            // {double Value[1, 0]}\n
            // ...
            // {double Value[NStepsX, NStepsY]}\n

            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                // base class
                Info = sr.ReadLine();
                Time = Convert.ToDateTime(sr.ReadLine());

                float Step;
                int   NSteps;
                // XGrid
                Step   = (float) Convert.ToDouble(sr.ReadLine());
                NSteps = Convert.ToInt32(sr.ReadLine());
                XGrid  = new Grid1D(Step, NSteps);
                // YGrid
                Step   = (float)Convert.ToDouble(sr.ReadLine());
                NSteps = Convert.ToInt32(sr.ReadLine());
                YGrid  = new Grid1D(Step, NSteps);

                // Value[,]
                Value = new double[XGrid.NSteps, YGrid.NSteps];
                for (int i = 0; i < XGrid.NSteps; ++i)
                {
                    for (int j = 0; j < YGrid.NSteps; ++j)
                    {
                        Value[i, j] = Convert.ToDouble(sr.ReadLine());
                    }
                }

                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (fs != null) { fs.Close(); }
            }
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            return new V3DataOnGridEnumerator(XGrid, YGrid, Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

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

        public override string ToLongString(string format)
        {
            string res = this.ToString() + "\n";

            for (int i = 0; i < XGrid.NSteps; ++i)
            {
                for (int j = 0; j < YGrid.NSteps; ++j)
                {
                    res += $"({(i * XGrid.Step).ToString(format)}, {(j * YGrid.Step).ToString(format)}) " +
                           $": {Value[i, j].ToString(format)}\n";
                }
            }

            return res;
        }
    }
}
