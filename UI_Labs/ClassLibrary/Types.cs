using System;
using System.Numerics;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ClassLibrary
{
    [Serializable]
    public struct DataItem : ISerializable
    {
        public Vector2 Coord { get; set; }
        public double Value { get; set; }

        public DataItem(Vector2 coord, double value)
        {
            Coord = coord;
            Value = value;
        }

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("Coord_X");
            float y = info.GetSingle("Coord_Y");
            Coord = new Vector2(x, y);
            Value = info.GetDouble("Value");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Coord_X", Coord.X);
            info.AddValue("Coord_Y", Coord.Y);
            info.AddValue("Value", Value);
        }

        public override string ToString() =>
            $"Coordinates: ({Coord.X}, {Coord.Y}), Value: {Value}";

        public string ToString(string format) =>
            $"Coordinates: ({Coord.X.ToString(format)}, {Coord.Y.ToString(format)}), Value: {Value.ToString(format)}";
    }

    [Serializable]
    public struct Grid1D
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

    [Serializable]
    public abstract class V3Data : INotifyPropertyChanged
    {
        private string info;
        private DateTime time;

        public string Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        public DateTime Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
