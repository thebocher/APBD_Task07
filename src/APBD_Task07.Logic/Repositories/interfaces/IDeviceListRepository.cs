using APBD_Task07.Logic.DTO;

namespace APBD_Task07.Logic.Repositories.interfaces;

public interface IDeviceListRepository
{
    List<DeviceDTO> GetDevices();
    
}