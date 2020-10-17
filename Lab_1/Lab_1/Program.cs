using System;
using System.Numerics;

namespace Lab_1
{
    class Program
    {
       static void Main(string[] args)
       {
            // 1)
            V3DataOnGrid testOnGrid = new V3DataOnGrid("Test1", new DateTime(), new Grid1D(1, 3), new Grid1D(1, 3));
            Console.WriteLine(testOnGrid.ToLongString());

            V3DataCollection testCollection = (V3DataCollection) testOnGrid;
            Console.WriteLine(testCollection.ToLongString());

            // 2)
            V3MainCollection testMainCollection = new V3MainCollection();
            testMainCollection.AddDefaults();
            Console.WriteLine(testMainCollection);

            // 3)
            Vector2 v = new Vector2(3, 1.5f);
            foreach (V3Data elem in testMainCollection)
            {
                Vector2[] nearests = elem.Nearest(v);
                foreach (Vector2 vec in nearests)
                {
                    Console.Write($"({vec.X}, {vec.Y}) ");
                }
                Console.WriteLine();
            }
       }
    }
}
