using System;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using System.Linq;
using System.ComponentModel;

namespace Lab_3
{
    enum ChangeInfo { ItemChanged, Add, Remove, Replace }

    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

    class V3MainCollection : IEnumerable<V3Data>
    {
        // Параметры для добавления стандартных экземпляров в список
        const int         OnGridCount = 2;
        const int         CollectionCount = 2;
        const float       MaxPoint = 2;
        const string      DefaultString = "default";
        readonly DateTime DefaultDateTime = new DateTime();
        readonly Grid1D   DefaultGrid = new Grid1D(1, (int)MaxPoint);
        const double      minValue = 0;
        const double      maxValue = 1;
        const int         nDefault = 5;

        private List<V3Data> Data = new List<V3Data>();

        public int Count { get => Data.Count; }

        public event DataChangedEventHandler DataChanged;

        private void PropertyChangedHandler(object source, PropertyChangedEventArgs args)
        {
            DataChanged?.Invoke(source, new DataChangedEventArgs(ChangeInfo.ItemChanged, $"Changed: {args.PropertyName}"));
        }

        public IEnumerator<V3Data> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public V3Data this[int index]
        { 
            get
            {
                if (index < 0 || index > Data.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                return Data[index];
            }
            set
            {
                int before = Data.Count();
                var prev = Data[index];
                Data[index] = value;
                int after = Data.Count();

                if (prev != value)
                {
                    DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Replace, $"Items in List: {before} -> {after}"));
                    value.PropertyChanged += PropertyChangedHandler;
                }
            }
        }

        public void Add(V3Data item)
        {
            int before = Data.Count();
            Data.Add(item);
            int after = Data.Count();
            DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Add, $"Items in List: {before} -> {after}"));
            item.PropertyChanged += PropertyChangedHandler;
        }

        public bool Remove(string id, DateTime date)
        {
            int before = Data.Count();

            foreach (var elem in Data.FindAll((V3Data elem) => elem.Info == id && elem.Time == date))
            {
                elem.PropertyChanged -= PropertyChangedHandler;
            }

            bool deletedSomething = Data.RemoveAll((V3Data elem) => elem.Info == id && elem.Time == date) > 0;
            int after = Data.Count();

            DataChanged?.Invoke(this, new DataChangedEventArgs(ChangeInfo.Remove, $"Items in List: {before} -> {after}"));

            return deletedSomething;
        }

        // Минимальное расстояние между v и точками из Data, в которых измерено поле
        public float RMin(Vector2 v)
        {
            return Data
                  .Select(DataCollectionCast)
                  .Where(x => x.Grid.Count() > 0)
                  .Select(x => x.Nearest(v))
                  .Select(x => Vector2.Distance(x.First(), v))
                  .Min();
        }

        // Результат всех измерений, который находится ближе всех к v
        public DataItem RMinDataItem(Vector2 v)
        {
            var queryToDataCollection = Data.Select(DataCollectionCast);
            var queryToDataItem       = from data   in queryToDataCollection  
                                        from vector in data
                                        select vector;

            return queryToDataItem.OrderBy(x => Vector2.Distance(x.Coord, v)).First();
        }

        // Перечисляет все точки измерения поля, такие, что они есть в элементах типа V3DataCollection,
        // но их нет в элементах типа V3DataOnGrid
        public IEnumerable<Vector2> DataCollectionExceptDataOnGrid
        {
            get
            {
                var dataOnGrid = Data.Where(x => x is V3DataOnGrid)
                                         .Select(DataCollectionCast);
                var dataCollection = Data.Where(x => x is V3DataCollection)
                                         .Select(DataCollectionCast);

                var vectorsDataOnGrid = from data in dataOnGrid
                                        from vector in data
                                        select vector.Coord;

                var vectorsDataCollection = from data in dataCollection
                                            from vector in data
                                            select vector.Coord;

                return vectorsDataCollection.Except(vectorsDataOnGrid).Distinct();
            }
        }

        public void AddDefaults()
        {
            // V3DataOnGrid
            for (int i = 0; i < OnGridCount; ++i)
            {
                V3DataOnGrid data = new V3DataOnGrid(DefaultString, DefaultDateTime, DefaultGrid, DefaultGrid);
                data.InitRandom(minValue, maxValue);
                Data.Add(data);
            }
            // V3DataCollection
            Data.Add((V3DataCollection)(Data[0] as V3DataOnGrid));

            for (int i = 0; i < CollectionCount; ++i)
            {
                V3DataCollection data = new V3DataCollection(DefaultString, DefaultDateTime);
                data.InitRandom(nDefault, MaxPoint, MaxPoint, minValue, maxValue);
                Data.Add(data);
            }

            // Для тестирования запросов LINQ
            Data.Add(new V3DataCollection(DefaultString, DefaultDateTime));
            Data.Add(new V3DataOnGrid("", new DateTime(), new Grid1D(0, 0), new Grid1D(0, 0)));
        }

        public override string ToString()
        {
            string result = "";
            foreach (V3Data elem in Data)
            {
                result += elem.ToLongString() + '\n';
            }

            return result;
        }
        
        public string ToLongString(string format)
        {
            string result = "";
            foreach (V3Data elem in Data)
            {
                result += elem.ToLongString(format) + '\n';
            }

            return result;
        }

        private V3DataCollection DataCollectionCast(V3Data elem)
        {
            return elem is V3DataOnGrid ? (V3DataCollection)(elem as V3DataOnGrid) : elem as V3DataCollection;
        }
    }
}
