using System.Text.RegularExpressions;
using APBD_Task07.Model.Exceptions;

namespace APBD_Task07.Model.Devices;

public class EmbeddedDevice : Device
{
    public static bool IsValidIPv4(string ip)
    {
        string pattern = @"^(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])(\.(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])){3}$";
        return Regex.IsMatch(ip, pattern);
    }
    private string _ipAddress;
    public string IpAddress
    {
        get => _ipAddress;
        set
        {
            if (!IsValidIPv4(value))
                throw new ArgumentException("Invalid IP address");
            
            _ipAddress = value;
        }
    }
    public string NetworkName { get; set; }

    public override void TurnOn()
    {
        base.TurnOn();
        Connect();
    }
    
    public void Connect()
    {
        if (!NetworkName.Contains("MD Ltd."))
            throw new ConnectionException("Connection exception");
    }

    public override string ToString()
    {
        return $"EmbeddedDevice({Id}, {Name}, {TurnedOn}, {IpAddress}, {NetworkName})";
    }

    public override string ToFileRepresentation()
    {
        return $"{Id},{Name},{IpAddress},{NetworkName}";
    }

    public override void Update(Device device)
    {
        base.Update(device);
        
        var ede2 = (EmbeddedDevice)device;
        Name = ede2.Name;
        TurnedOn = ede2.TurnedOn;
        TurnedOn = ede2.TurnedOn;
        IpAddress = ede2.IpAddress;
        NetworkName = ede2.NetworkName;
    }
}