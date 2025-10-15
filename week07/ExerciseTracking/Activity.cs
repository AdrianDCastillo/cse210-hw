using System;
using System.Globalization;

abstract class Activity
{
    private DateTime _date;
    private int _minutes;

    protected DateTime Date => _date;
    protected int Minutes => _minutes;

    protected Activity(DateTime date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    public abstract double GetDistance();   
    public abstract double GetSpeed();      
    public abstract double GetPace();       

    public virtual string GetSummary()
    {
        string day = _date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
        string type = GetType().Name;
        double distance = Math.Round(GetDistance(), 2);
        double speed = Math.Round(GetSpeed(), 2);
        double pace = Math.Round(GetPace(), 2);
        return $"{day} {type} ({_minutes} min): Distance {distance} km, Speed {speed} kph, Pace {pace} min per km";
    }
}
