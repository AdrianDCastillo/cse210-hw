using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        Console.WriteLine("Enter a list of numbers, type 0 when finished.");
        Console.Write("Enter number: ");
        int number = int.Parse(Console.ReadLine());

        while (number != 0)
        {
            numbers.Add(number);
            Console.Write("Enter number: ");
            number = int.Parse(Console.ReadLine());
        }
        int sum = 0;
        foreach (int num in numbers)
        {
            sum += num;
        }
        Console.WriteLine($"The sum is: {sum}");
        double saverage = 0;
        saverage = (double)sum / numbers.Count;
        Console.WriteLine($"The average is: {saverage}");
        int max = numbers[0];
        foreach (int num in numbers)
        {
            if (num > max)
            {
                max = num;
            }
        }
        Console.WriteLine($"The max is: {max}");

        bool foundPositive = false;
        int smallestPositive = 0;

        numbers.Sort();

        foreach (int n in numbers)
        {
            if (n > 0)
            {
                smallestPositive = n;
                foundPositive = true;
                break;
            }
        }
        
        if (foundPositive)
        {
            Console.WriteLine($"The smallest positive number is: {smallestPositive}");
        }
        else
        {
            Console.WriteLine("No positive numbers were entered.");
        } 

        
        Console.WriteLine("The sorted list is: ");

        foreach (int num in numbers)
        {
            Console.WriteLine($"{num} ");
        }
    }
}