using System;
using System.Numerics;

namespace Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            V3MainCollection task1 = new V3MainCollection();
            task1.DataChanged += DataChangedHandler;
            task1.Add(new V3DataCollection("Elem1", new DateTime()));
            task1.Add(new V3DataCollection("Elem2", new DateTime()));
            task1.Add(new V3DataCollection("Elem3", new DateTime()));
            task1[0] = task1[1];
            task1.Remove("Elem2", new DateTime());
            task1[0].Info = "Changed Info";
            task1[0] = new V3DataCollection("Elem4", new DateTime());
            task1[0].Info = "aaa";
            task1[0].Info = "bbb";
        }

        static void DataChangedHandler(object source, DataChangedEventArgs args)
        {
            Console.WriteLine($"Happend event in {source.GetType()}: {args}\n");
        }
    }
}
