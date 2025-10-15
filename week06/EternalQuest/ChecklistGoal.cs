using System;

class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int target, int bonus)
        : base(name, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    public override int RecordEvent()
    {
        _amountCompleted++;
        if (_amountCompleted == _target) return Points + _bonus;
        return Points;
    }

    public override bool IsComplete() => _amountCompleted >= _target;

    public override string GetDetailsString()
    {
        string box = IsComplete() ? "[X]" : "[ ]";
        return $"{box} {Name} ({Description}) -- Completed {_amountCompleted}/{_target}";
    }

    public override string GetStringRepresentation()
    {
        return $"Checklist|{Name}|{Description}|{Points}|{_amountCompleted}|{_target}|{_bonus}";
    }

    public static ChecklistGoal FromString(string[] parts)
    {
        var goal = new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[5]), int.Parse(parts[6]));
        goal._amountCompleted = int.Parse(parts[4]);
        return goal;
    }
}
