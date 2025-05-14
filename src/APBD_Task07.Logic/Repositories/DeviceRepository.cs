using APBD_Task07.Logic.DTO;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Repositories;

public class DeviceRepository(string connectionString) : IDeviceRepository
{
    public List<DeviceDTO> GetDevices()
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        string sql = "SELECT * FROM Device";
        using var cmd = new SqlCommand(sql, conn);
        var result = cmd.ExecuteReader();
        
        var devices = new List<DeviceDTO>();

        while (result.Read())
        {
            devices.Add(new DeviceDTO
            {
                Id = (string) result["Id"],
                Name = (string) result["Name"],
                TurnedOn = (bool) result["IsEnabled"]
            });
        }

        return devices;
    }

    public byte[]? GetRowVersion(string id, SqlConnection conn, SqlTransaction transaction)
    {
        var sql = "SELECT RowVersion FROM Device WHERE Id = @Id";
        using var cmd = new SqlCommand(sql, conn, transaction);
        cmd.Parameters.AddWithValue("@Id", id);
        byte[]? result = (byte[]) cmd.ExecuteScalar();
        return result;

    }

}