using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Parsers;

public class EmbeddedDeviceParser : IParser
{
    public bool CanParse(string s)
    {
        return s.StartsWith("ED") && s.Split(",").Length == 4;
    }

    public Device Parse(string s)
    {
        var split = s.Split(",");
        var id = split[0];
        var name = split[1];
        var turnedOn = bool.Parse(split[2]);
        var ip = split[3];
        var networkName = split[4];
        
        return new EmbeddedDevice
        {
            Id = id,
            Name = name,
            TurnedOn = turnedOn,
            IpAddress = ip,
            NetworkName = networkName
        };
    }
}