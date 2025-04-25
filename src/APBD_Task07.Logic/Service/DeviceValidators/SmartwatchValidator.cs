using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Exceptions;

namespace APBD_Task07.Logic.Service.DeviceValidators;

public class SmartwatchValidator : DeviceValidator
{
    public override bool CanValidate(string deviceId)
    {
        return deviceId.StartsWith("SW-");
    }

    public override void Validate(Device device)
    {
        base.Validate(device);
        
        var smartwatch = (SmartWatch) device;

        if (smartwatch is { TurnedOn: true, BatteryPercentage: < 11 })
        {
            throw new EmptyBatteryException("Empty battery");
        }
    }
}