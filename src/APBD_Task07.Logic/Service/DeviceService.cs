using APBD_Task07.Logic.DTO;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Logic.Service.DeviceValidators;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.Logic.Service;

public class DeviceService : IDeviceService
{
    private static readonly List<DeviceValidator> _deviceValidators =
    [
        new EmbeddedDeviceValidator(), new PersonalComputerValidator(), new SmartwatchValidator()
    ];
    
    private IEmbeddedDeviceRepository _embeddedDeviceRepository;
    private IPersonalComputerRepository _personalComputerRepository;
    private IDeviceRepository _deviceRepository;
    private ISmartwatchRepository _smartwatchRepository;

    public DeviceService(
        IEmbeddedDeviceRepository embeddedDeviceRepository,
        IPersonalComputerRepository personalComputerRepository,
        IDeviceRepository deviceRepository,
        ISmartwatchRepository smartwatchRepository)
    {
        _embeddedDeviceRepository = embeddedDeviceRepository;
        _personalComputerRepository = personalComputerRepository;
        _deviceRepository = deviceRepository;
        _smartwatchRepository = smartwatchRepository;
    }

    private IDeviceCrudRepository GetDeviceCrudRepository(string deviceId)
    {
        var type = deviceId.Split("-")[0];

        switch (type)
        {
            case "SW": return _smartwatchRepository;
            case "PC": return _personalComputerRepository;
            case "ED": return _embeddedDeviceRepository;
            default: throw new Exception("Invalid device type");
        }
    }

    private static DeviceValidator GetDeviceValidator(string deviceId)
    {
        return _deviceValidators.First(v => v.CanValidate(deviceId));
    }

    public List<DeviceDTO> GetDevices()
    {
        return _deviceRepository.GetDevices();
    }

    public Device? GetDeviceById(string deviceId)
    {
        return GetDeviceCrudRepository(deviceId).GetDevice(deviceId);
    }

    public bool AddDevice(Device device)
    {
        var validator = GetDeviceValidator(device.Id);
        validator.Validate(device);

        GetDeviceCrudRepository(device.Id).AddDevice(device);
        return true;
    }

    public bool DeleteDevice(String deviceId)
    {
        return GetDeviceCrudRepository(deviceId).DeleteDevice(deviceId);
    }

    public bool UpdateDevice(string deviceId, Device device)
    {
        var validator = GetDeviceValidator(deviceId);
        validator.Validate(device);
        
        return GetDeviceCrudRepository(deviceId).UpdateDevice(deviceId, device);
    }
}