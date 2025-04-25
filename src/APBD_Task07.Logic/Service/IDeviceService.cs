using APBD_Task07.Logic.DTO;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Service;

public interface IDeviceService
{
    List<DeviceDTO> GetDevices();
    Device? GetDeviceById(string deviceId);
    bool AddDevice(Device device);
    bool DeleteDevice(string deviceId);
    bool UpdateDevice(string deviceId, Device device);
}