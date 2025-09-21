using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Entry
{
    private readonly string _date;
    private readonly string _promptText;
    private readonly string _entryText;

    public Entry(string date, string promptText, string entryText)
    {
        _date = date;
        _promptText = promptText;
        _entryText = entryText;
    }

    public void Display()
    {
        Console.WriteLine($"Date: {_date}");
        Console.WriteLine($"Prompt: {_promptText}");
        Console.WriteLine("REsponse:");
        Console.WriteLine($"Entry: {_entryText}");
        Console.WriteLine(new string('-', 40));
    }

    private const string Sep = "~|~";

    public string ToFileLine()
    {
        return $"{_date}{Sep}{_promptText}{Sep}{_entryText}";
    }

    public static Entry FromFileLine(string line)
    {
        var parts = line.Split(new[] { Sep }, StringSplitOptions.None);
        if (parts.Length != 3)
            throw new FormatException("Invalid line format");
        return new Entry(parts[0], parts[1], parts[2]);  
    }

}

public class Journal
{
    private readonly List<Entry> _entries = new List<Entry>();

    public void AddEntry(Entry entry)
    {
        if (entry == null) return;
        _entries.Add(entry);
    }

    public void DisplayAll()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("No entries to display.");
            return;
        }
        foreach (var entry in _entries)
        {
            entry.Display();
        }

    }

    public void SaveToFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("Invalid file name.");
            return;
        }

        try
        {
            using var w = new StreamWriter(fileName, false, Encoding.UTF8);
            foreach (var e in _entries)
            {
                w.WriteLine(e.ToFileLine());

            }

            Console.WriteLine($"Journal saved to {fileName}.");
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error saving journal: {ex.Message}");
        }
    }

    public void LoadFromFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("Invalid filename.");
            return;
        }

        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            var loaded = new List<Entry>();
            foreach (var line in File.ReadAllLines(filename, Encoding.UTF8))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                loaded.Add(Entry.FromFileLine(line));
            }

            _entries.Clear();
            _entries.AddRange(loaded);

            Console.WriteLine($"Journal loaded from '{filename}'. Entries: {_entries.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file: {ex.Message}");
        }
    }
    
}

public class PromptGenerator
{
    private readonly List<string> _prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    private readonly Random _random = new Random();

    public string GetRandomPrompt()
    {
        var index = _random.Next(_prompts.Count);
        return _prompts[index];
    }
}


class Program
{
    static void Main(string[] args)
    {
        var journal = new Journal();
        var prompts = new PromptGenerator();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Journal Menu");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Quit");
            Console.Write("Select an option (1-5): ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    WriteNewEntry(journal, prompts);
                    break;
                case "2":
                    journal.DisplayAll();
                    break;
                case "3":
                    Console.Write("Enter filename to save (e.g., journal.txt): ");
                    journal.SaveToFile(Console.ReadLine());
                    break;
                case "4":
                    Console.Write("Enter filename to load (e.g., journal.txt): ");
                    journal.LoadFromFile(Console.ReadLine());
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    private static void WriteNewEntry(Journal journal, PromptGenerator prompts)
    {
        string prompt = prompts.GetRandomPrompt();
        Console.WriteLine($"Prompt: {prompt}");
        Console.WriteLine("Write your response. Press ENTER on an empty line to finish:");

        string response = ReadMultiline();
        string date = DateTime.Now.ToString("yyyy-MM-dd"); 

        var entry = new Entry(date, prompt, response);
        journal.AddEntry(entry);
        Console.WriteLine("Entry added.");
    }

    
    private static string ReadMultiline()
    {
        var sb = new StringBuilder();
        while (true)
        {
            string line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) break;
            sb.AppendLine(line);
        }
        return sb.ToString().TrimEnd();
    }
}