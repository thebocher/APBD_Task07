using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Service.DeviceValidators;

public abstract class DeviceValidator
{
    public abstract bool CanValidate(string deviceId);

    public virtual void Validate(Device device)
    {
        if (device.Id?.GetType() != typeof(string))
            throw new ArgumentException("Invalid device id");
        
        if (device.Name.GetType() != typeof(string))
            throw new ArgumentException("Invalid device name");
        
        if (device.TurnedOn.GetType() != typeof(bool))
            throw new ArgumentException("Invalid device turned on");
    }
}