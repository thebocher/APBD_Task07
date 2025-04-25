using APBD_Task07.Logic.DTO;
using APBD_Task07.Logic.Service.DeviceTableParser;
using APBD_Task07.Logic.Service.DeviceValidators;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Service;

public class DeviceService(string connectionString) : IDeviceService
{
    private static readonly List<IDeviceTableParser> _deviceTableParsers = [
        new EmbeddedDeviceTableParser(), new PersonalComputerTableParser(),
        new SmartwatchTableParser()
    ];

    private static readonly List<DeviceValidator> _deviceValidators =
    [
        new EmbeddedDeviceValidator(), new PersonalComputerValidator(), new SmartwatchValidator()
    ];

    private IDeviceTableParser GetDeviceTableParser(string deviceId)
    {
        return _deviceTableParsers
            .First(p => p.CanParse(deviceId));
    }

    private DeviceValidator GetDeviceValidator(string deviceId)
    {
        return _deviceValidators.First(v => v.CanValidate(deviceId));
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
    
    public List<DeviceDTO> GetDevices()
    {
        string sql = "SELECT * FROM Device";
        var devices = new List<DeviceDTO>();

        using (var conn = GetConnection())
        {
            SqlCommand command = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var device = new DeviceDTO
                        {
                            Id = (string)reader["Id"],
                            Name = (string)reader["Name"],
                            TurnedOn = (bool)reader["IsEnabled"],
                        };
                        devices.Add(device);
                    }
                }

                return devices;
            }
            finally
            {
                reader.Close();
            }
        }
    }

    public Device? GetDeviceById(string deviceId)
    {
        var tableParser = GetDeviceTableParser(deviceId);
        var sql = $"SELECT * FROM Device {tableParser.GetJoinCommand()} WHERE Device.Id=@id";

        using (var conn = GetConnection())
        {
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", deviceId);
            conn.Open();
            
            var reader = command.ExecuteReader();

            try
            {
                if (!reader.HasRows) return null;

                reader.Read();
                return tableParser.ParseFromReader(reader);
            } 
            finally
            {
                reader.Close();
            }
        }
    }

    public bool AddDevice(Device device)
    {
        var validator = GetDeviceValidator(device.Id);
        Console.WriteLine($"validate? {validator.GetType()}");
        validator.Validate(device);
        Console.WriteLine($"validated {validator.GetType()}");

        var tableParser = GetDeviceTableParser(device.Id);
        var devicesSql = "INSERT INTO Device (Id, Name, IsEnabled) VALUES (@id, @name, @turned_on)";
        
        using (var conn = GetConnection())
        {
            SqlCommand command = new SqlCommand(devicesSql, conn);
            command.Parameters.AddWithValue("@id", device.Id);
            command.Parameters.AddWithValue("@name", device.Name);
            command.Parameters.AddWithValue("@turned_on", device.TurnedOn);
            conn.Open();
            
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0) return false;
        
            return tableParser.InsertToOwnTable(device, conn);
        }
    }

    public bool DeleteDevice(String deviceId)
    {
        var sql = "DELETE FROM Device WHERE Device.Id=@id";
        
        using (var conn = GetConnection())
        {
            SqlCommand command = new SqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", deviceId);
            conn.Open();
            
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }
    }

    public bool UpdateDevice(string deviceId, Device device)
    {
        var validator = GetDeviceValidator(deviceId);
        validator.Validate(device);
        
        var tableParser = GetDeviceTableParser(deviceId);
        var devicesSql = @"UPDATE Device
                            SET Name = @name, 
                                IsEnabled = @turned_on
                            WHERE Device.Id=@id";

        using (var conn = GetConnection())
        {
            SqlCommand command = new SqlCommand(devicesSql, conn);
            command.Parameters.AddWithValue("@id", deviceId);
            command.Parameters.AddWithValue("@name", device.Name);
            command.Parameters.AddWithValue("@turned_on", device.TurnedOn);
            conn.Open();
            
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0) return false;

            return tableParser.UpdateInOwnTable(deviceId, device, conn);
        }
    }
}