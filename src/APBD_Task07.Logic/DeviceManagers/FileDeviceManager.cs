using APBD_Task07.Logic.Parsers;
using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Exceptions;
using APBD_Task07.Model.Interfaces;

namespace APBD_Task07.Logic.DeviceManagers;

public class FileDeviceManager : IDeviceManager
{
    private readonly List<Device> _devices;
    private readonly string _filePath;
    private static readonly CsvDeviceFileParser Parser = new([
        new EmbeddedDeviceParser(), new PersonalComputerParser(), new SmartWatchParser()
    ]); 
    
    public FileDeviceManager(string filePath)
    {
        _filePath = filePath;
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File does not exist");
        }

        _devices = Parser.Parse(filePath);
    }

    public void AddDevice(Device device)
    {
        _devices.Add(device);
    }

    public void RemoveDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        _devices.Remove(device);
    }

    public void EditDeviceData(String deviceId, Device data)
    {
        var device = GetDeviceById(deviceId);
        device.Update(data);
    }

    public void TurnOnDevice(string deviceId)
    {
        var device = GetDeviceById(deviceId);
        device.TurnOn();
    }

    public void TurnOffDevice(string deviceId)
    {
        GetDeviceById(deviceId).TurnOff();
    }

    public void ShowAllDevices()
    {
        foreach (var device in _devices)
        {
            Console.WriteLine(device);
        }
    }

    public Device GetDeviceById(string deviceId)
    {
        foreach (var device in _devices)
        {
            if (device.Id == deviceId) return device;
        }
        
        throw new DeviceNotFoundException();
    }

    public void SaveData()
    {
        var newData = "";

        foreach (var device in _devices)
        {
            newData += device.ToFileRepresentation() + "\n";
        }

        newData.TrimEnd();
        
        File.WriteAllText(_filePath, newData);
    }

    public override string ToString()
    {
        var devicesString = "";

        foreach (var device in _devices)
        {
            devicesString += device;
        }
        devicesString = devicesString.TrimEnd(',');

        return $"DeviceManager(\"{_filePath}\", [{devicesString}])";
    }
}