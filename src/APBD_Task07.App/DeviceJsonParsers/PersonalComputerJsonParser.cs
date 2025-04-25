using System.Text.Json;
using System.Text.Json.Nodes;
using APBD_Task07.Model.Devices;

namespace APBD_Task07.App.DeviceJsonParsers;

public class PersonalComputerJsonParser : DeviceJsonParser
{
    public override PersonalComputer? Parse(JsonNode json)
    {
        return JsonSerializer.Deserialize<PersonalComputer>(json.ToString(), DeserializeOptions);
    }
}