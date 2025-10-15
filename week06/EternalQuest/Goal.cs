using System;

abstract class Goal
{
    private string _shortName;
    private string _description;
    private int _points;

    protected string Name => _shortName;
    protected string Description => _description;
    protected int Points => _points;

    protected Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    public abstract int RecordEvent();
    public abstract bool IsComplete();
    public virtual string GetDetailsString()
    {
        string box = IsComplete() ? "[X]" : "[ ]";
        return $"{box} {Name} ({Description})";
    }
    public abstract string GetStringRepresentation();
}