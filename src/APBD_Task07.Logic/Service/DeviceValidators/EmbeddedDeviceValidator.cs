using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Exceptions;

namespace APBD_Task07.Logic.Service.DeviceValidators;

public class EmbeddedDeviceValidator : DeviceValidator
{
    public override bool CanValidate(string deviceId)
    {
        return deviceId.StartsWith("ED-");
    }

    public override void Validate(Device device)
    {
        base.Validate(device);
        
        var embeddedDevice = (EmbeddedDevice)device;

        if (!EmbeddedDevice.IsValidIPv4(embeddedDevice.IpAddress))
        {
            throw new ArgumentException("Invalid IP address");
        }

        if (!embeddedDevice.NetworkName.Contains("MD Ltd."))
        {
            throw new ConnectionException("Invalid network name");
        }
        
        
    }
}