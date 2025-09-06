using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Whats is your first name? ");
        string name = Console.ReadLine();
        Console.Write("What is your last name? ");
        string lastName = Console.ReadLine();   
        Console.WriteLine(" ");
        Console.WriteLine(" ");
        Console.WriteLine($"Your name is {lastName}, {name} {lastName}");
    }
}                       