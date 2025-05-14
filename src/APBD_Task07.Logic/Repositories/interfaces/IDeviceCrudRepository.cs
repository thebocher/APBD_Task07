using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Repositories.interfaces;

public interface IDeviceCrudRepository
{
    void AddDevice(Device device);
    Device? GetDevice(string deviceId);
    bool DeleteDevice(string deviceId);
    bool UpdateDevice(string deviceId, Device device);
}