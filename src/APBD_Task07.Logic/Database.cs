using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic;

public class Database
{
    public static List<Device> Devices { get; set; } = [];

    public static void Initialize()
    {
        Devices = [
            new EmbeddedDevice
            {
                Id = "ED1", IpAddress = "12.12.12.12", Name = "name", NetworkName = "network", TurnedOn = false
            }, 
            new SmartWatch
            {
                BatteryPercentage = 10, Id = "SW1", Name = "name", TurnedOn = false
            }, 
            new PersonalComputer
            {
                Id = "PC1", Name = "name", OperationalSystem = "os", TurnedOn = false
            }
        ];
    }
}