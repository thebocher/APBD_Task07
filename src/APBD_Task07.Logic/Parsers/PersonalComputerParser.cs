using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Parsers;

public class PersonalComputerParser : IParser
{
    public bool CanParse(string s)
    {
        return s.StartsWith("P") && s.Split(",").Length == 4;
    }

    public Device Parse(string s)
    {
        var split = s.Split(',');
        var id = split[0];
        var name = split[1];
        var turnedOn = bool.Parse(split[2]);
        var os = split[3];
        
        return new PersonalComputer
        {
            Id = id,
            Name = name,
            TurnedOn = turnedOn,
            OperationalSystem = os
        };
    }
}