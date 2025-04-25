using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Exceptions;

namespace APBD_Task07.Logic.Service.DeviceValidators;

public class PersonalComputerValidator : DeviceValidator
{
    public override bool CanValidate(string deviceId)
    {
        return deviceId.StartsWith("PC-");
    }

    public override void Validate(Device device)
    {
        base.Validate(device);
        
        var personalComputer = (PersonalComputer)device;

        if (string.IsNullOrEmpty(personalComputer.OperationalSystem))
        {
            throw new EmptySystemException("Empty system");
        }
    }
}