using System.Data.Common;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Service.DeviceTableParser;

public class PersonalComputerTableParser : IDeviceTableParser
{
    public bool CanParse(string deviceId)
    {
        return deviceId.StartsWith("PC");
    }

    public string GetJoinCommand()
    {
        return "JOIN PersonalComputer pc on Device.id=pc.DeviceId";
    }

    public Device ParseFromReader(DbDataReader reader)
    {
        return new PersonalComputer()
        {
            Id = (string)reader["DeviceId"],
            Name = (string)reader["Name"],
            TurnedOn = (bool)reader["IsEnabled"],
            OperationalSystem = (string)reader["OperationSystem"],
        };
    }

    public bool InsertToOwnTable(Device device, SqlConnection connection)
    {
        var personalComputer = (PersonalComputer)device;
        var sql = "INSERT INTO PersonalComputer(DeviceId, OperationSystem)"
                  + "VALUES (@id, @operational_system)";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", personalComputer.Id);
        command.Parameters.AddWithValue("@operational_system", personalComputer.OperationalSystem);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool UpdateInOwnTable(string id, Device device, SqlConnection connection)
    {
        var personalComputer = (PersonalComputer)device;
        var sql = @"UPDATE PersonalComputer 
                    SET OperationSystem = @operational_system
                    WHERE DeviceId = @id";
        
        var command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@operational_system", personalComputer.OperationalSystem);
        
        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }
}