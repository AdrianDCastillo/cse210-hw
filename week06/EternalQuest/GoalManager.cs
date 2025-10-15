using System;
using System.Collections.Generic;
using System.IO;

class GoalManager
{
    private List<Goal> _goals;
    private int _score;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
    }

    public void Start()
    {
        while (true)
        {
            Console.WriteLine();
            DisplayPlayerInfo();
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goal Names");
            Console.WriteLine("3. List Goal Details");
            Console.WriteLine("4. Save Goals");
            Console.WriteLine("5. Load Goals");
            Console.WriteLine("6. Record Event");
            Console.WriteLine("7. Quit");
            Console.Write("Select a choice from the menu: ");
            string input = Console.ReadLine();
            Console.WriteLine();

            if (input == "1") CreateGoal();
            else if (input == "2") ListGoalNames();
            else if (input == "3") ListGoalDetails();
            else if (input == "4") SaveGoals();
            else if (input == "5") LoadGoals();
            else if (input == "6") RecordEvent();
            else if (input == "7") break;
        }
    }

    public void DisplayPlayerInfo()
    {
        Console.WriteLine($"You have {_score} points.");
    }

    public void ListGoalNames()
    {
        if (_goals.Count == 0) { Console.WriteLine("No goals created."); return; }
        for (int i = 0; i < _goals.Count; i++)
        {
            string head = _goals[i].GetDetailsString().Split('(')[0].Trim();
            Console.WriteLine($"{i + 1}. {head}");
        }
    }

    public void ListGoalDetails()
    {
        if (_goals.Count == 0) { Console.WriteLine("No goals created."); return; }
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    public void CreateGoal()
    {
        Console.WriteLine("The types of Goals are:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        Console.Write("Which type of goal would you like to create? ");
        string choice = Console.ReadLine();

        Console.Write("What is the name of your goal? ");
        string name = Console.ReadLine();
        Console.Write("What is a short description of it? ");
        string desc = Console.ReadLine();
        Console.Write("What is the amount of points associated with this goal? ");
        int points = int.Parse(Console.ReadLine());

        if (choice == "1")
        {
            _goals.Add(new SimpleGoal(name, desc, points));
        }
        else if (choice == "2")
        {
            _goals.Add(new EternalGoal(name, desc, points));
        }
        else if (choice == "3")
        {
            Console.Write("How many times does this goal need to be accomplished for a bonus? ");
            int target = int.Parse(Console.ReadLine());
            Console.Write("What is the bonus for accomplishing it that many times? ");
            int bonus = int.Parse(Console.ReadLine());
            _goals.Add(new ChecklistGoal(name, desc, points, target, bonus));
        }
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0) { Console.WriteLine("No goals to record."); return; }
        ListGoalNames();
        Console.Write("Which goal did you accomplish? ");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index < 0 || index >= _goals.Count) return;
        int earned = _goals[index].RecordEvent();
        _score += earned;
        Console.WriteLine($"Event recorded. You earned {earned} points. Total: {_score}");
    }

    public void SaveGoals()
    {
        Console.Write("Enter the filename to save to: ");
        string filename = Console.ReadLine();
        using (var sw = new StreamWriter(filename))
        {
            sw.WriteLine($"SCORE|{_score}");
            foreach (var g in _goals) sw.WriteLine(g.GetStringRepresentation());
        }
        Console.WriteLine("Goals saved.");
    }

    public void LoadGoals()
    {
        Console.Write("Enter the filename to load from: ");
        string filename = Console.ReadLine();
        if (!File.Exists(filename)) { Console.WriteLine("File not found."); return; }

        _goals.Clear();
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            string[] parts = line.Split('|');
            if (parts[0] == "SCORE") _score = int.Parse(parts[1]);
            else if (parts[0] == "Simple") _goals.Add(SimpleGoal.FromString(parts));
            else if (parts[0] == "Eternal") _goals.Add(EternalGoal.FromString(parts));
            else if (parts[0] == "Checklist") _goals.Add(ChecklistGoal.FromString(parts));
        }
        Console.WriteLine("Goals loaded.");
    }
}
