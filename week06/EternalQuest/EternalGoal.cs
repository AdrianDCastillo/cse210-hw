using System;

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points) { }

    public override int RecordEvent() => Points;

    public override bool IsComplete() => false;

    public override string GetStringRepresentation()
    {
        return $"Eternal|{Name}|{Description}|{Points}";
    }

    public static EternalGoal FromString(string[] parts)
    {
        return new EternalGoal(parts[1], parts[2], int.Parse(parts[3]));
    }
}
