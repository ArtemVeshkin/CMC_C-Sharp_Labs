using System;
using System.Collections.Generic;
using System.Collections;

namespace Lab_1
{
    class V3MainCollection : IEnumerable<V3Data>
    {
        // Параметры для добавления стандартных экземпляров в список
        const int OnGridCount = 5;
        const int CollectionCount = 5;
        const float MaxPoint = 5;
        const string DefaultSrting = "default";
        readonly DateTime DefaultDateTime = new DateTime();
        readonly Grid1D DefaultGrid = new Grid1D(1, (int)MaxPoint);
        const double minValue = 0;
        const double maxValue = 1;
        const int nDefault = 5;

        private List<V3Data> Data = new List<V3Data>();

        public int Count { get => Data.Count; }

        public IEnumerator<V3Data> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(V3Data item)
        {
            Data.Add(item);
        }

        public bool Remove(string id, DateTime date)
        {
            return Data.RemoveAll((V3Data elem) => elem.Info == id && elem.Time == date) > 0;
        }

        public void AddDefaults()
        {
            // V3DataOnGrid
            for (int i = 0; i < OnGridCount; ++i)
            {
                V3DataOnGrid data = new V3DataOnGrid(DefaultSrting, DefaultDateTime, DefaultGrid, DefaultGrid);
                data.InitRandom(minValue, maxValue);
                Data.Add(data);
            }
            // V3DataCollection
            for (int i = 0; i < CollectionCount; ++i)
            {
                V3DataCollection data = new V3DataCollection(DefaultSrting, DefaultDateTime);
                data.InitRandom(nDefault, MaxPoint, MaxPoint, minValue, maxValue);
                Data.Add(data);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (V3Data elem in Data)
            {
                result += elem.ToString() + '\n';
            }

            return result;
        }
    }
}
