using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Parsers;

public class SmartWatchParser : IParser
{
    public bool CanParse(string s)
    {
        return s.StartsWith("SW") && s.Split(",").Length == 4;
    }

    public Device Parse(string s)
    {
        string[] split = s.Split(',');
        var id = split[0];
        var name = split[1];
        var turnedOn = bool.Parse(split[2]);
        var batteryPercentage = float.Parse(split[3].Replace("%", ""));
        
        return new SmartWatch
        {
            Id = id,
            Name = name,
            TurnedOn = turnedOn,
            BatteryPercentage = batteryPercentage
        };
    }
}