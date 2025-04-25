using APBD_Task07.Model.Devices;

namespace APBD_Task07.Model.Interfaces;

public interface IDeviceModification
{
    public void AddDevice(Device device);
    public void RemoveDevice(string deviceId);
    public void EditDeviceData(String deviceId, Device data);
    public void SaveData();
}