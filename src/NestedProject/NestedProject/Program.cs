using System;
using NestedProject;

public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Choose an operation:");
        Console.WriteLine("1. Addition");
        Console.WriteLine("2. Subtraction");
        Console.WriteLine("3. Multiplication");
        Console.WriteLine("4. Division");
        Console.WriteLine("5. Concatenate Strings");
        Console.WriteLine("6. Delayed Operation");

        int choice = int.Parse(Console.ReadLine());

        MathOperations mathOperations = new MathOperations();
        StringOperations stringOperations = new StringOperations();
        var continueExecution = true;
        do
        {
            if (choice >= 1 && choice <= 4)
            {
                var (num1, num2) = mathOperations.GetNumbersFromUser();

                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"Result: {mathOperations.Sum(num1, num2)}");
                        break;
                    case 2:
                        Console.WriteLine($"Result: {mathOperations.Subtract(num1, num2)}");
                        break;
                    case 3:
                        Console.WriteLine($"Result: {mathOperations.Multiply(num1, num2)}");
                        break;
                    case 4:
                        if (num2 != 0)
                        {
                            Console.WriteLine($"Result: {mathOperations.Divide(num1, num2)}");
                        }
                        else
                        {
                            Console.WriteLine("Cannot divide by zero.");
                        }
                        break;
                }
            }
            else if (choice == 5)
            {
                Console.WriteLine("Enter the first string:");
                string str1 = Console.ReadLine();

                Console.WriteLine("Enter the second string:");
                string str2 = Console.ReadLine();

                Console.WriteLine($"Concatenated Result: {stringOperations.Concatenate(str1, str2)}");
            }
            else if (choice == 6)
            {
                Console.WriteLine("Starting delayed operation...");
                await mathOperations.DelayedOperation();
                Console.WriteLine("Delayed operation completed.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }


            Console.WriteLine("Continue? y/n");

            var response = Console.ReadLine();

            if (string.Equals(response, "n", StringComparison.OrdinalIgnoreCase))
            {
                continueExecution = false;
            }
            else
            {
                continueExecution = true;
            }


        } while (continueExecution);


    }
}
