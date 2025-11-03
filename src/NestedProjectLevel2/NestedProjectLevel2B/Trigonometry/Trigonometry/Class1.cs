using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trigonometry
{
    class Program
 {
      static void Main(string[] args)
        {
    bool exit = false;

            while (!exit)
     {
       Console.Clear();
   Console.WriteLine("=================================");
          Console.WriteLine("  TRIGONOMETRY CALCULATOR");
     Console.WriteLine("=================================");
       Console.WriteLine();
        Console.WriteLine("Select an operation:");
         Console.WriteLine("1. Sine (sin)");
  Console.WriteLine("2. Cosine (cos)");
                Console.WriteLine("3. Tangent (tan)");
    Console.WriteLine("4. Cosecant (csc)");
     Console.WriteLine("5. Secant (sec)");
        Console.WriteLine("6. Cotangent (cot)");
   Console.WriteLine("7. Arc Sine (asin)");
       Console.WriteLine("8. Arc Cosine (acos)");
    Console.WriteLine("9. Arc Tangent (atan)");
      Console.WriteLine("10. Convert Degrees to Radians");
      Console.WriteLine("11. Convert Radians to Degrees");
     Console.WriteLine("0. Exit");
 Console.WriteLine("=================================");
   Console.Write("Enter your choice: ");

      string choice = Console.ReadLine();

      if (choice == "0")
 {
   exit = true;
    Console.WriteLine("Goodbye!");
           continue;
                }

      if (choice == "10")
        {
        ConvertDegreesToRadians();
    }
         else if (choice == "11")
        {
            ConvertRadiansToDegrees();
          }
         else
          {
       PerformTrigOperation(choice);
        }

      Console.WriteLine();
    Console.Write("Press any key to continue...");
            Console.ReadKey();
      }
      }

        static void PerformTrigOperation(string choice)
    {
            Console.Write("Enter angle in degrees: ");
       if (!double.TryParse(Console.ReadLine(), out double degrees))
  {
     Console.WriteLine("Invalid input. Please enter a valid number.");
           return;
            }

    double radians = DegreesToRadians(degrees);
        double result = 0;
            string operation = "";

  switch (choice)
       {
             case "1":
        result = Math.Sin(radians);
      operation = "sin";
              break;
            case "2":
        result = Math.Cos(radians);
     operation = "cos";
         break;
case "3":
        result = Math.Tan(radians);
     operation = "tan";
     break;
          case "4":
        result = 1 / Math.Sin(radians);
       operation = "csc";
                 break;
   case "5":
       result = 1 / Math.Cos(radians);
     operation = "sec";
                 break;
   case "6":
  result = 1 / Math.Tan(radians);
 operation = "cot";
          break;
        case "7":
        if (degrees < -1 || degrees > 1)
         {
         Console.WriteLine("Error: Arc sine input must be between -1 and 1.");
      return;
         }
         result = Math.Asin(degrees);
         result = RadiansToDegrees(result);
         operation = "asin";
           Console.WriteLine($"{operation}({degrees}) = {result}°");
     return;
   case "8":
           if (degrees < -1 || degrees > 1)
   {
         Console.WriteLine("Error: Arc cosine input must be between -1 and 1.");
      return;
                }
   result = Math.Acos(degrees);
     result = RadiansToDegrees(result);
  operation = "acos";
        Console.WriteLine($"{operation}({degrees}) = {result}°");
       return;
      case "9":
        result = Math.Atan(degrees);
        result = RadiansToDegrees(result);
          operation = "atan";
           Console.WriteLine($"{operation}({degrees}) = {result}°");
                    return;
           default:
      Console.WriteLine("Invalid choice.");
          return;
      }

    Console.WriteLine($"{operation}({degrees}°) = {result}");
        }

        static void ConvertDegreesToRadians()
        {
       Console.Write("Enter angle in degrees: ");
            if (!double.TryParse(Console.ReadLine(), out double degrees))
     {
    Console.WriteLine("Invalid input. Please enter a valid number.");
     return;
       }

            double radians = DegreesToRadians(degrees);
            Console.WriteLine($"{degrees}° = {radians} radians");
        }

        static void ConvertRadiansToDegrees()
        {
    Console.Write("Enter angle in radians: ");
        if (!double.TryParse(Console.ReadLine(), out double radians))
            {
             Console.WriteLine("Invalid input. Please enter a valid number.");
    return;
          }

 double degrees = RadiansToDegrees(radians);
        Console.WriteLine($"{radians} radians = {degrees}°");
        }

        static double DegreesToRadians(double degrees)
        {
 return degrees * Math.PI / 180.0;
     }

  static double RadiansToDegrees(double radians)
        {
        return radians * 180.0 / Math.PI;
        }
    }
}
