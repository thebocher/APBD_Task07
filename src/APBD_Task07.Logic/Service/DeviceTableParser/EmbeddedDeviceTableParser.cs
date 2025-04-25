using System.Data.Common;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Service.DeviceTableParser;

public class EmbeddedDeviceTableParser : IDeviceTableParser
{
    public bool CanParse(string deviceId)
    {
        return deviceId.StartsWith("ED");
    }

    public string GetJoinCommand()
    {
        return "JOIN Embedded ed on Device.id=ed.DeviceId";
    }

    public Device ParseFromReader(DbDataReader reader)
    {
        return new EmbeddedDevice()
        {
            Id = (string)reader["DeviceId"],
            Name = (string)reader["Name"],
            TurnedOn = (bool)reader["IsEnabled"],
            IpAddress = (string)reader["IpAddress"],
            NetworkName = (string)reader["NetworkName"],
        };
    }

    public bool InsertToOwnTable(Device device, SqlConnection connection)
    {
        var embeddedDevice = (EmbeddedDevice)device;
        var sql = "INSERT INTO Embedded(DeviceId, IpAddress, NetworkName)"
                  + "VALUES (@id, @ip_address, @network_name)";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", embeddedDevice.Id);
        command.Parameters.AddWithValue("@ip_address", embeddedDevice.IpAddress);
        command.Parameters.AddWithValue("@network_name", embeddedDevice.NetworkName);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool UpdateInOwnTable(string id, Device device, SqlConnection connection)
    {
        var embeddedDevice = (EmbeddedDevice)device;
        var sql = @"UPDATE Embedded
                    SET IpAddress = @ip_address,
                        NetworkName = @network_name
                    WHERE DeviceId = @id";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@ip_address", embeddedDevice.IpAddress);
        command.Parameters.AddWithValue("@network_name", embeddedDevice.NetworkName);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }
}