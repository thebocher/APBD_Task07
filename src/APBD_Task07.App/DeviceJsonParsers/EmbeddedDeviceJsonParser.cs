using System.Text.Json;
using System.Text.Json.Nodes;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.App.DeviceJsonParsers;

public class EmbeddedDeviceJsonParser : DeviceJsonParser
{
    public override EmbeddedDevice? Parse(JsonNode json)
    {
        return JsonSerializer.Deserialize<EmbeddedDevice>(json.ToString(), DeserializeOptions);
    }

}