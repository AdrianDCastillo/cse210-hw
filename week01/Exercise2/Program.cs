using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your grade percentage? ");
        string grade = Console.ReadLine();
        int gradePercentage = int.Parse(grade);

        string gradeLetter = "";
        string symbol = "";
        int lasDigit = gradePercentage % 10;

        if (gradePercentage >= 90)
        {
            gradeLetter = "A";
        }
        else if (gradePercentage >= 80)
        {
            gradeLetter = "B";
        }
        else if (gradePercentage >= 70)
        {
            gradeLetter = "C";
        }
        else if (gradePercentage >= 60)
        {
            gradeLetter = "D";
        }
        else
        {
            gradeLetter = "F";
        }


        if (gradeLetter == "F")
        {
            symbol = "";
        }

        else if (gradeLetter == "A")
        {
            if (lasDigit <= 2)
            {
                symbol = "-";
            }
            else
            {
                symbol = "";
            }

        }

        else
        {
            if (lasDigit >= 7)
            {
                symbol = "+";
            }
            else if (lasDigit < 3)
            {
                symbol = "-";
            }
            else
            {
                symbol = "";
            }
        }


        Console.WriteLine($"Your grade is: {gradeLetter} {symbol}");
        
        if(gradeLetter == "A" || gradeLetter == "B" || gradeLetter == "C")
        {
            Console.WriteLine("Congratulations! You passed the course!");
        }
        else
        {
            Console.WriteLine("Sorry, you did not pass. Better luck next time!");
        }
    }
}