﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Collections.Specialized;

namespace ClassLibrary
{
    [Serializable]
    public class V3DataCollection : V3Data, IEnumerable<DataItem>, INotifyCollectionChanged 
    {
        public List<DataItem> Grid { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public V3DataCollection(string info, DateTime time) : base(info, time) 
        {
            Grid = new List<DataItem>();
        }

        public void Add(DataItem elem)
        {
            Grid.Add(elem);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<DataItem> GetEnumerator()
        {
            return Grid.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void InitRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
        {
            Random rnd = new Random();
            for (int i = 0; i < nItems; ++i)
            {
                float x = (float)rnd.NextDouble() * xmax;
                float y = (float)rnd.NextDouble() * ymax;
                double value = rnd.NextDouble() * (maxValue - minValue) + minValue;

                Add(new DataItem(new Vector2(x, y), value));
            }
        }

        public override Vector2[] Nearest(Vector2 v)
        {
            Vector2[] curNearest = new Vector2[Grid.Count];
            int curNearestSize = 1;
            double minDist = Math.Pow(v.X - Grid[0].Coord.X, 2) + Math.Pow(v.Y - Grid[0].Coord.Y, 2);

            curNearest[0] = Grid[0].Coord;

            foreach (DataItem data in Grid.Skip(1))
            {
                double curDist = Math.Pow(v.X - data.Coord.X, 2) + Math.Pow(v.Y - data.Coord.Y, 2);

                if (curDist < minDist)
                {
                    minDist = curDist;
                    curNearestSize = 1;
                    curNearest[0] = data.Coord;
                }
                else if (curDist == minDist)
                {
                    curNearest[curNearestSize] = data.Coord;
                    ++curNearestSize;
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
            $"Class: {this.GetType()}, {base.ToString()}, Items in Grid: {Grid.Count}";

        public override string ToLongString()
        {
            string res = this.ToString() + "\n";

            foreach (DataItem elem in Grid)
            {
                res += $"({elem.Coord.X}, {elem.Coord.Y}) : {elem.Value}\n";
            }

            return res;
        }

        public override string ToLongString(string format)
        {
            string res = this.ToString() + "\n";

            foreach (DataItem elem in Grid)
            {
                res += $"({elem.Coord.X.ToString(format)}, {elem.Coord.Y.ToString(format)}) : {elem.Value.ToString(format)}\n";
            }

            return res;
        }
    }
}
