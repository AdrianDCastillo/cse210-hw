using System;
using System.Runtime.CompilerServices;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        int magicNumber = randomGenerator.Next(1, 100);
        Console.WriteLine($" What is the magic number? {magicNumber}");

        Console.Write("What is your guess? ");
        string guess = Console.ReadLine();
        int guessNumber = int.Parse(guess);
        String gameAgain = "";

        bool play = true;

        int attempts = 0;

        while (play)
        {
            attempts = attempts + 1;
            if (guessNumber < magicNumber)
            {
                Console.Write("Too low. Guess again: ");
                guess = Console.ReadLine();
                guessNumber = int.Parse(guess);

            }
            else if (guessNumber > magicNumber)
            {
                Console.Write("Too high. Guess again: ");
                guess = Console.ReadLine();
                guessNumber = int.Parse(guess);

            }
            else
            {
                Console.WriteLine($"You guessed it! It took you {attempts} attempts.");
                Console.Write("Do you want to play again? (yes/no) ");
                gameAgain = Console.ReadLine().ToLower();
                if (gameAgain == "yes")
                {
                    magicNumber = randomGenerator.Next(1, 100);
                    Console.WriteLine($" What is the magic number? {magicNumber}");
                    Console.Write("What is your guess? ");
                    guess = Console.ReadLine();
                    guessNumber = int.Parse(guess);
                    attempts = 0;
                }
                else
                {
                    play = false;
                    Console.WriteLine("Thanks for playing!");
                }
            }
        }
    }
}