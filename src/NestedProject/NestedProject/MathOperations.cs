using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedProject
{
    public class MathOperations
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public double Divide(int a, int b)
        {
            return (double)a / b;
        }

        public (int, int) GetNumbersFromUser()
        {
            Console.WriteLine("Enter the first number:");
            int num1 = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the second number:");
            int num2 = int.Parse(Console.ReadLine());

            return (num1, num2);
        }

        public async Task DelayedOperation()
        {
            await Task.Delay(10000);
        }
    }
}
