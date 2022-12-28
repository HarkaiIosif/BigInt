using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigInt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Introduceti cele doua numare unul sub celalalt");
            Bint b1 = new Bint(Console.ReadLine());
            Bint b2=new Bint(Console.ReadLine());
            Console.WriteLine($"Suma:{b1+b2}");
            Console.WriteLine($"Diferenta:{b1-b2}");
            Console.WriteLine($"Produsul:{b1*b2}");
            Console.WriteLine($"b1 mai mare ca b2:{b1>b2}");
            Console.WriteLine($"b1 mai mic ca b2:{b1<b2}");
            Console.WriteLine($"b1 egal cu b2:{b1==b2}");
            Console.WriteLine($"b1 diferit de b2:{b1!=b2}");
            Console.WriteLine($"b1 mai mare sau egal cu b2:{b1>=b2}");
            Console.WriteLine($"b1 mai mic sau egal cu b2:{b1<=b2}");
            Console.WriteLine($"b1 la puterea b2:{b1^b2}");




        }
    }
}
