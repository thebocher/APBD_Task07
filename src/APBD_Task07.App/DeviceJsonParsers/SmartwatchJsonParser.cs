using System.Text.Json;
using System.Text.Json.Nodes;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.App.DeviceJsonParsers;

public class SmartwatchJsonParser : DeviceJsonParser
{
    public override SmartWatch? Parse(JsonNode json)
    {
        return JsonSerializer.Deserialize<SmartWatch>(json.ToString(), DeserializeOptions);
    }
}