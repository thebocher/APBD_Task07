using System.Text.Json;
using System.Text.Json.Nodes;
using APBD_Task07.App.DeviceJsonParsers;
using APBD_Task07.Logic;
using APBD_Task07.Logic.Parsers;
using APBD_Task07.Model.Devices;
using APBD_Task07.Model.Interfaces;
using APBD_Task07.Logic.Service;
using APBD_Task07.Logic.Service.DeviceValidators;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Task07.App.Controllers;

[ApiController]
[Route("/api/devices")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly Dictionary<string, DeviceJsonParser> _jsonParsers = new()
    {
        {"SW", new SmartwatchJsonParser()}, 
        {"ED", new EmbeddedDeviceJsonParser()},
        {"PC", new PersonalComputerJsonParser()}
    };
    private readonly List<IParser> _textParsers = new()
    {
        new EmbeddedDeviceParser(), new SmartWatchParser(), new PersonalComputerParser()
    };

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [NonAction]
    private DeviceJsonParser GetDeviceJsonParser(string deviceType)
    {
        return _jsonParsers[deviceType];
    }

    [NonAction]
    private Device? DeserializeDevice(HttpRequest request, DeviceJsonParser? parser)
    {
        using var reader = new StreamReader(request.Body);
        string rawJson = reader.ReadToEnd();
        var json = JsonNode.Parse(rawJson);

        if (parser is null)
        {
            string? deviceType = (string) json["DeviceType"];
        
            if (deviceType == null)
            {
                throw new ArgumentException("Invalid device type");
            }
            parser = GetDeviceJsonParser(deviceType);
        }
        
        Device? result = parser.Parse(json);
        return result;
    }
    
    [HttpGet]
    public IResult GetAllDevices()
    {
        return Results.Ok(_deviceService.GetDevices());
    }

    [HttpGet("{id}")]
    public IResult GetDevice(string id)
    {
        try
        {
            var result = _deviceService.GetDeviceById(id);
            
            if (result == null)
                return Results.NotFound();
            
            return Results.Ok(result);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Results.NotFound();
        }
    }

    [HttpPost]
    public IResult AddDevice()
    {
        string contentType = Request.Headers["Content-Type"];
        Device? device;
        
        switch (contentType)
        {
            case "application/json":
                try
                {
                    device = DeserializeDevice(Request, null);

                    if (device == null)
                    {
                        return Results.BadRequest("Invalid device body");
                    }

                    device.Id = device.GenerateId();

                    _deviceService.AddDevice(device);
                    return Results.Created("", device);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }

            case "text/plain":
                var reader = new StreamReader(Request.Body);
                string body = reader.ReadToEnd();
                
                foreach (var parser in _textParsers)
                {
                    try
                    {
                        if (!parser.CanParse(body)) continue;
                    
                        device = parser.Parse(body);
                        _deviceService.AddDevice(device);
                    }
                    catch (Exception e)
                    {
                        return Results.BadRequest(e.Message);
                    }
                    return Results.Created();
                }

                break;
            default:
                return Results.BadRequest("invalid content type");
        }

        return Results.BadRequest();
    }

    [HttpPut("{id}")]
    public IResult UpdateDevice(string id)
    {
        try
        {
            var deviceType = id.Split("-")[0];
            var parser = GetDeviceJsonParser(deviceType);
            Device? device = DeserializeDevice(Request, parser);

            if (device == null)
            {
                return Results.BadRequest("Invalid device body");
            }
            
            if (_deviceService.UpdateDevice(id, device))
            {
                return Results.Ok();
            }
            
            return Results.NotFound();
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.StackTrace + " " + e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IResult DeleteDevice(string id)
    {
        try
        {
            var result = _deviceService.DeleteDevice(id);
            if (!result) return Results.NotFound();

            return Results.Ok();
        }
        catch
        {
            return Results.NotFound();
        }
        
    }
}