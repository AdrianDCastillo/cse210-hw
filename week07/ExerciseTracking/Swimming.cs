using System;

class Swimming : Activity
{
    private int _laps;

    public Swimming(DateTime date, int minutes, int laps) : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance() => (_laps * 50.0) / 1000.0;

    public override double GetSpeed() => (GetDistance() / Minutes) * 60.0;

    public override double GetPace() => Minutes / GetDistance();
}
