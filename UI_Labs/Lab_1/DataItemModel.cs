using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Numerics;
using ClassLibrary;

namespace Lab
{
    class DataItemModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private float x, y;
        private double value;

        public V3DataCollection Collection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public float X
        {
            get { return x; }
            set
            {
                x = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public float Y
        {
            get { return y; }
            set
            {
                y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public DataItemModel(V3DataCollection collection)
        {
            Collection = collection;
            X = Y = 0;
            Value = 1;
        }

        public string Error { get => "DataItemModel error text"; }

        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "Value":
                        if (Value <= 0)
                        {
                            msg = "Value should be > 0";
                        }
                        goto case "X";
                    case "X":
                    case "Y":
                        if (Collection.Grid.Contains(new DataItem(new Vector2(X, Y), Value)))
                        {
                            msg = "Collection already contains this element";
                        }
                        break;
                }
                return msg;
            }
        }

        public void Add()
        {
            Collection.Add(new DataItem(new Vector2(X, Y), Value));
        }
    }
}
