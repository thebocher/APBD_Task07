namespace APBD_Task07.Model.Interfaces;

public interface IDeviceManager : IDeviceModification, IDeviceTurnOnAndOff, IDeviceSearching
{
    public void ShowAllDevices();
}