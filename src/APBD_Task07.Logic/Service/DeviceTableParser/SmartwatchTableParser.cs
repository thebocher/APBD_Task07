using System.Data.Common;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Service.DeviceTableParser;

public class SmartwatchTableParser : IDeviceTableParser
{
    public bool CanParse(string deviceId)
    {
        return deviceId.StartsWith("SW");
    }

    public string GetJoinCommand()
    {
        return "JOIN Smartwatch sw ON sw.DeviceId = Device.id";
    }

    public Device ParseFromReader(DbDataReader reader)
    {
        return new SmartWatch()
        {
            Id = (string)reader["DeviceId"],
            Name = (string)reader["Name"],
            TurnedOn = (bool)reader["IsEnabled"],
            BatteryPercentage = (float)reader["BatteryPercentage"],
        };
    }

    public bool InsertToOwnTable(Device device, SqlConnection connection)
    {
        var smartWatch = (SmartWatch)device;
        var sql = "INSERT INTO Smartwatch(DeviceId, BatteryPercentage)"
                  + "VALUES (@id, @battery_percent)";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", smartWatch.Id);
        command.Parameters.AddWithValue("@battery_percent", smartWatch.BatteryPercentage);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool UpdateInOwnTable(string id, Device device, SqlConnection connection)
    {
        var smartWatch = (SmartWatch)device;
        var sql = @"UPDATE Smartwatch
                    SET BatteryPercentage = @battery_percent
                    WHERE DeviceId = @id";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@battery_percent", smartWatch.BatteryPercentage);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }
}