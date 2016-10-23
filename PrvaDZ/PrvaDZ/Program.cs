using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrvaDZ
{
    class Program
    {
      
        static void Main(string[] args)
        {
            IGenericList<float>listOfIntegers = new GenericList<float>();
            listOfIntegers.Add(1.3f); // [1]
            listOfIntegers.Add(2.5f); // [1 ,2]
            listOfIntegers.Add(3.9866f); // [1 ,2 ,3]
            Console.WriteLine(listOfIntegers.Contains(2.67f));
            listOfIntegers.Add(4.87f); // [1 ,2 ,3 ,4]
            listOfIntegers.Add(5.6f); // [1 ,2 ,3 ,4 ,5]
            listOfIntegers.RemoveAt(0); // [2 ,3 ,4 ,5]
            listOfIntegers.Remove(5.6f); //[2 ,3 ,4]
            ;
            Console.WriteLine(listOfIntegers.Count); // 3
            foreach (float item in listOfIntegers)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(listOfIntegers.Remove(100.78f)); // false
            Console.WriteLine(listOfIntegers.RemoveAt(5)); // false
            foreach (float item in listOfIntegers)
            {
                Console.WriteLine(item);
            }
            listOfIntegers.Clear(); // []
            Console.WriteLine(listOfIntegers.Count); // 0
            


        }
        
      }
}
