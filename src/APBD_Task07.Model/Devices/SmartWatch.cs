using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Exceptions;
using APBD_Task07.Model.Interfaces;

public class SmartWatch : Device, IPowerNotifier
{
    private float _batteryPercentage;
    
    public float BatteryPercentage
    {
        get => _batteryPercentage;
        set
        {
            if (value < 0) 
                value = 0;

            if (value > 100)
                value = 100;
            
            if (value < 20)
                NotifyLowBattery();
            
            _batteryPercentage = value;
        }
    }

    public void NotifyLowBattery()
    {
        Console.WriteLine("Low battery");
    }

    public override void TurnOn()
    {
        if (BatteryPercentage < 11)
        {
            throw new EmptyBatteryException("Battery percentage is too low");
        }
        
        base.TurnOn();
        BatteryPercentage -= 10;
    }

    public override string ToString()
    {
        return $"SmartWatch({Id}, {Name}, {TurnedOn}, {BatteryPercentage})";
    }

    public override string ToFileRepresentation()
    {
        return $"{Id},{Name},{TurnedOn},{BatteryPercentage}";
    }

    public override void Update(Device device)
    {
        base.Update(device);
        
        var smartWatch2 = (SmartWatch)device;
        Name = smartWatch2.Name;
        TurnedOn = smartWatch2.TurnedOn;
        BatteryPercentage = smartWatch2.BatteryPercentage;
    }
}