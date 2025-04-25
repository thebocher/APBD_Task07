namespace APBD_Task07.Model.Interfaces;

public interface IDeviceTurnOnAndOff
{
    public void TurnOnDevice(string deviceId);
    public void TurnOffDevice(string deviceId);
}