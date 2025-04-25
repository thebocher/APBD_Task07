using APBD_Task07.Model.Devices;

namespace APBD_Task07.Model.Interfaces;

public interface IDeviceSearching
{
    public Device GetDeviceById(string deviceId);
}