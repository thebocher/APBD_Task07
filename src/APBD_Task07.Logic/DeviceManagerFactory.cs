using APBD_Task07.Logic.DeviceManagers;
using APBD_Task07.Model.Interfaces;

namespace APBD_Task07.Logic;

public class DeviceManagerFactory
{
    public static IDeviceManager GetFileDeviceManager(string filePath)
    {
        return new FileDeviceManager(filePath);
    }
}