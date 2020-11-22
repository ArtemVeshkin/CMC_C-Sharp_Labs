using System;
using System.Numerics;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1)
            V3DataOnGrid task1 = new V3DataOnGrid("..//..//..//data.txt");
            Console.WriteLine(task1.ToLongString("F3"));

            // 2)
            V3MainCollection task2 = new V3MainCollection();
            task2.AddDefaults();

            // 3)
            // RMin
            Console.WriteLine($"RMin(2, 2):\n{task2.RMin(new Vector2(2, 2))}\n");

            // RMinDataItem
            Console.WriteLine($"RMinDataItem(2, 2):\n{task2.RMinDataItem(new Vector2(2, 2))}\n");

            // DataCollectionExceptDataOnGrid
            Console.WriteLine("DataCollectionExceptDataOnGrid:");
            var task3 = task2.DataCollectionExceptDataOnGrid;
            foreach (Vector2 vec in task3)
            {
                Console.WriteLine(vec);
            }
        }
    }
}
