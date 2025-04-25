using System.Text.Json;
using System.Text.Json.Nodes;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.App.DeviceJsonParsers;

public abstract class DeviceJsonParser
{
    protected JsonSerializerOptions DeserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        
    };
    public abstract Device? Parse(JsonNode json);
}