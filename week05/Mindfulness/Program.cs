using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflecting Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            Activity activity = null;
            if (choice == "1")
            {
                activity = new BreathingActivity(
                    "Breathing Activity",
                    "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing."
                );
            }
            else if (choice == "2")
            {
                activity = new ReflectingActivity(
                    "Reflection Activity",
                    "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life."
                );
            }
            else if (choice == "3")
            {
                activity = new ListingActivity(
                    "Listing Activity",
                    "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."
                );
            }
            else if (choice == "4")
            {
                exit = true;
            }

            if (activity != null)
            {
                activity.DisplayStartingMessage();
                activity.Run();
                activity.DisplayEndingMessage();
            }
        }
    }
}

abstract class Activity
{
    private string _name;
    private string _description;
    private int _duration;

    protected Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine();
        Console.WriteLine(_description);
        Console.WriteLine();
        _duration = ReadPositiveSeconds("How long, in seconds, would you like your session? ");
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
        Console.WriteLine();
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Well done!");
        ShowSpinner(2);
        Console.WriteLine($"You have completed another {_duration} seconds of the {_name}.");
        ShowSpinner(3);
    }

    protected int GetDurationSeconds() => _duration;

    protected void ShowSpinner(int seconds)
    {
        char[] frames = new[] { '|', '/', '-', '\\' };
        DateTime end = DateTime.Now.AddSeconds(seconds);
        int i = 0;
        while (DateTime.Now < end)
        {
            Console.Write(frames[i % frames.Length]);
            Thread.Sleep(150);
            Console.Write("\b \b");
            i++;
        }
    }

    protected void ShowCountDown(int seconds)
    {
        for (int i = seconds; i >= 1; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }

    private int ReadPositiveSeconds(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();
            if (int.TryParse(s, out value) && value > 0) return value;
            Console.WriteLine("Please enter a positive whole number of seconds.");
        }
    }

    public abstract void Run();
}

class BreathingActivity : Activity
{
    public BreathingActivity(string name, string description) : base(name, description) { }

    public override void Run()
    {
        DateTime end = DateTime.Now.AddSeconds(GetDurationSeconds());
        bool inhale = true;
        while (DateTime.Now < end)
        {
            Console.WriteLine(inhale ? "Breathe in..." : "Breathe out...");
            ShowCountDown(4);
            inhale = !inhale;
            Console.WriteLine();
        }
    }
}

class ReflectingActivity : Activity
{
    private List<string> _prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> _questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectingActivity(string name, string description) : base(name, description) { }

    public override void Run()
    {
        string prompt = GetRandomPrompt();
        DisplayPrompt(prompt);

        Console.Write("When you have something in mind, press ENTER to continue.");
        Console.ReadLine();

        Console.Write("\nYou may begin in: ");
        ShowCountDown(5);
        Console.WriteLine();

        DateTime deadline = DateTime.Now.AddSeconds(GetDurationSeconds());
        DisplayQuestions(deadline);
    }

    private string GetRandomPrompt()
    {
        Random rnd = new Random();
        return _prompts[rnd.Next(_prompts.Count)];
    }

    private string GetRandomQuestion()
    {
        Random rnd = new Random();
        return _questions[rnd.Next(_questions.Count)];
    }

    private void DisplayPrompt(string prompt)
    {
        Console.WriteLine();
        Console.WriteLine("Consider the following prompt:\n");
        Console.WriteLine($"--- {prompt} ---\n");
    }

    private void DisplayQuestions(DateTime deadline)
    {
        while (DateTime.Now < deadline)
        {
            Console.Write($"> {GetRandomQuestion()} ");
            ShowSpinner(7);
            Console.WriteLine();
        }
    }
}

class ListingActivity : Activity
{
    private int _count;
    private List<string> _prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity(string name, string description) : base(name, description) { }

    public override void Run()
    {
        string prompt = GetRandomPrompt();

        Console.WriteLine();
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine($"--- {prompt} ---");
        Console.Write("You may begin in: ");
        ShowCountDown(5);
        Console.WriteLine();

        DateTime deadline = DateTime.Now.AddSeconds(GetDurationSeconds());
        List<string> items = GetListFromUser(deadline);
        _count = items.Count;

        Console.WriteLine();
        Console.WriteLine($"You listed {_count} items!");
    }

    private string GetRandomPrompt()
    {
        Random rnd = new Random();
        return _prompts[rnd.Next(_prompts.Count)];
    }

    private List<string> GetListFromUser(DateTime deadline)
    {
        var items = new List<string>();
        while (DateTime.Now < deadline)
        {
            Console.Write("> ");
            string line = ReadLineUntil(deadline);
            if (!string.IsNullOrWhiteSpace(line)) items.Add(line.Trim());
        }
        return items;
    }

    private string ReadLineUntil(DateTime deadline)
    {
        var buffer = new List<char>();
        while (DateTime.Now < deadline)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Count > 0)
                    {
                        buffer.RemoveAt(buffer.Count - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    buffer.Add(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
            Thread.Sleep(10);
        }
        return new string(buffer.ToArray());
    }
}
