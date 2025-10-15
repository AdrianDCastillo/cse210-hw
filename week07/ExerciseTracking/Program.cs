using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var activities = new List<Activity>
        {
            new Running(new DateTime(2025, 11, 3), 30, 4.8),
            new Cycling(new DateTime(2025, 11, 4), 45, 28.5),
            new Swimming(new DateTime(2025, 11, 5), 40, 64)
        };

        foreach (var a in activities)
        {
            Console.WriteLine(a.GetSummary());
        }
    }
}
